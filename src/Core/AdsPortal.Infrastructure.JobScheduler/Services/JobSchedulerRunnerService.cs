namespace AdsPortal.Infrastructure.JobScheduler.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.JobScheduler;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Enums;
    using AdsPortal.Infrastructure.JobScheduler.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using Serilog;
    using TTimer = System.Threading.Timer;

    public sealed class JobSchedulerRunnerService : IHostedService, IJobSchedulerRunnerService
    {
        private const int MaxConcurent = 8;
        private const int MaxConcurentBatch = 64;

        private int _processing = 0;
        private readonly SemaphoreSlim _sync = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private TTimer? Timer { get; set; }

        private IServiceScopeFactory ServiceScopeFactory { get; }

        public Guid InstanceId { get; } = Guid.NewGuid();

        public JobSchedulerRunnerService(IServiceScopeFactory serviceProvider)
        {
            ServiceScopeFactory = serviceProvider;
        }

        private async void TickTimer(object state)
        {
            CancellationToken cancellationToken = (CancellationToken)state;

            await CheckAndProcessJobs(cancellationToken);
        }

        private async Task CheckAndProcessJobs(CancellationToken cancellationToken)
        {
            int toTake = Math.Min((MaxConcurent * MaxConcurentBatch) - _processing, MaxConcurentBatch);

            if (toTake <= 0)
                return;

            using (IServiceScope jobScope = ServiceScopeFactory.CreateScope())
            {
                IAppRelationalUnitOfWork uow = jobScope.ServiceProvider.GetRequiredService<IAppRelationalUnitOfWork>();

                DateTime now = DateTime.UtcNow;

                if (await _sync.WaitAsync(40, cancellationToken))
                {
                    List<Job>? queuedJobs = null;

                    //Only one task can access db
                    try
                    {
                            queuedJobs = await uow.Jobs.AllAsync(filter: x => (x.Status == JobStatuses.Queued || x.Status == JobStatuses.Taken) &&
                                                                              (x.PostponeTo == null || x.PostponeTo >= now) &&
                                                                              x.Instance != InstanceId,
                                                                 orderBy: (order) => order.OrderByDescending(x => x.Priority).ThenBy(x => x.JobNo),
                                                                 take: toTake,
                                                                 cancellationToken: cancellationToken);

                            Interlocked.Add(ref _processing, toTake);

                            foreach (var job in queuedJobs)
                            {
                                job.Status = JobStatuses.Taken;
                                job.Instance = InstanceId;
                                uow.Jobs.Update(job);
                            }

                            await uow.SaveChangesAsync();
                    }
                    finally
                    {
                        _sync.Release();
                    }

                    //Execute if there are tasks
                    if (queuedJobs is List<Job> jobs && jobs.Count > 0)
                    {
                        Log.ForContext<JobSchedulingService>().Debug("Executing {Count} new job(s) in batch ({Processing} processing).", toTake, _processing);

                        IEnumerable<Task> tasks = jobs.Select(x => ExecuteJob(x, cancellationToken));
                        foreach (var bucket in Interleaved(tasks))
                        {
                            await bucket;
                        }

                        //await Task.WhenAll(tasks).ConfigureAwait(false);
                    }
                }
            }
        }

        //https://devblogs.microsoft.com/pfxteam/processing-tasks-as-they-complete/
        private Task<Task>[] Interleaved(IEnumerable<Task> tasks)
        {
            var inputTasks = tasks.ToList();

            var buckets = new TaskCompletionSource<Task>[inputTasks.Count];
            var results = new Task<Task>[buckets.Length];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new TaskCompletionSource<Task>();
                results[i] = buckets[i].Task;
            }

            int nextTaskIndex = -1;
            Action<Task> continuation = completed =>
            {
                Interlocked.Decrement(ref _processing);

                var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
                bucket.TrySetResult(completed);
            };

            foreach (Task inputTask in inputTasks)
                inputTask.ContinueWith(continuation, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return results;
        }

        private async Task ExecuteJob(Job job, CancellationToken cancellationToken)
        {
            Type? type = Type.GetType(job.Operation);
            if (type is null)
            {
                Log.ForContext<JobSchedulingService>().Error("Unknown job of type {Type}", job.Operation);
                return;
            }

            using (IServiceScope jobScope = ServiceScopeFactory.CreateScope())
            {
                IAppRelationalUnitOfWork jobUow = jobScope.ServiceProvider.GetRequiredService<IAppRelationalUnitOfWork>();
                IJob? jobInstance = jobScope.ServiceProvider.GetService(type) as IJob;

                if (jobInstance is null)
                {
                    Log.ForContext<JobSchedulingService>().Error("Unknown job of type {Type}", job.Operation);
                    return;
                }

                Log.ForContext<JobSchedulingService>().Verbose("Running job {No}", job.JobNo);

                DateTime startTime = DateTime.UtcNow;
                DateTime? finishTime = null;

                job.StartedOn = startTime;
                job.Status = JobStatuses.Running;
                jobUow.Jobs.Update(job);
                await jobUow.SaveChangesAsync();

                using (CancellationTokenSource jobCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    TimeSpan timeout = TimeSpan.FromSeconds(5000); //TODO: timeout pre job
                    jobCts.CancelAfter(timeout); // TODO: set from settings, job class attribute or when schedulling a command

                    JobStatuses finishStatus = JobStatuses.Error;
                    try
                    {
                        startTime = DateTime.UtcNow;
                        await jobInstance.Handle(job.Arguments, jobCts.Token);
                        finishTime = DateTime.UtcNow;
                        finishStatus = JobStatuses.Success;
                    }
                    catch (TaskCanceledException)
                    {
                        finishTime = DateTime.UtcNow;
                        bool timedOut = !_cts.IsCancellationRequested && jobCts.IsCancellationRequested;
                        finishStatus = timedOut ? JobStatuses.TimedOut : JobStatuses.Cancelled;

                        if (timedOut)
                        {
                            Log.ForContext<JobSchedulingService>().Error("Job {Id} {JobNo} timed out after {Time}, and thus was cancelled.", job.Id, job.JobNo, timeout);
                        }
                        else
                        {
                            Log.ForContext<JobSchedulingService>().Error("Job {Id} {JobNo} was cancelled.", job.Id, job.JobNo);
                        }
                    }
                    catch (Exception ex)
                    {
                        job.Exception = JsonConvert.SerializeObject(ex);

                        Log.ForContext<JobSchedulingService>().Fatal(ex, "Error occured during job {Id} {JobNo} execution.", job.Id, job.JobNo);
                    }
                    finally
                    {
                        job.StartedOn = startTime;
                        job.FinishedOn = finishTime ?? DateTime.UtcNow;
                        job.Status = finishStatus;
                        jobUow.Jobs.Update(job);

                        await jobUow.SaveChangesAsync();
                    }
                }

                Log.ForContext<JobSchedulingService>().Verbose("Finished job {No}", job.JobNo);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.ForContext<JobSchedulingService>().Debug("Starting {Service}.", nameof(JobSchedulerRunnerService));

            Timer ??= new TTimer(TickTimer!, _cts.Token, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(50)); //TODO add appsettings section

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.ForContext<JobSchedulingService>().Debug("Stopping {Service}.", nameof(JobSchedulerRunnerService));

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Log.ForContext<JobSchedulingService>().Debug("Disposing {Service}.", nameof(JobSchedulerRunnerService));

            // dispose\stop timer here
            Timer?.Dispose();
            Timer = null;

            // then cancel
            _cts.Cancel();

            // then wait until all is done
            _sync.WaitAsync();
        }
    }
}
