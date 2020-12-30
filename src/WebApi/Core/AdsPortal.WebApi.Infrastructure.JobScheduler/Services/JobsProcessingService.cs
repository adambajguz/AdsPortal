namespace AdsPortal.Infrastructure.JobScheduler.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Configurations;
    using AdsPortal.Application.Interfaces.JobScheduler;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Domain.Enums;
    using AdsPortal.Infrastructure.JobScheduler.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using TTimer = System.Threading.Timer;

    public sealed class JobsProcessingService : IHostedService, IJobSchedulerRunnerService
    {
        private int _processing = 0;
        private readonly SemaphoreSlim _sync = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private TTimer? Timer { get; set; }

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly JobSchedulerConfiguration _configuration;
        private readonly ILogger _logger;

        public Guid InstanceId { get; } = Guid.NewGuid();

        public JobsProcessingService(IServiceScopeFactory serviceProvider,
                                     IOptions<JobSchedulerConfiguration> configuration,
                                     ILogger<JobsProcessingService> logger)
        {
            _serviceScopeFactory = serviceProvider;
            _configuration = configuration.Value;
            _logger = logger;
        }

        private async void TickTimer(object state)
        {
            CancellationToken cancellationToken = (CancellationToken)state;

            await CheckAndProcessJobs(cancellationToken);
        }

        private async Task CheckAndProcessJobs(CancellationToken cancellationToken)
        {
            int maxConcurent = _configuration.MaxConcurent;
            int concurentBatchSize = _configuration.ConcurentBatchSize;

            int toTake = Math.Min((maxConcurent * concurentBatchSize) - _processing, concurentBatchSize);

            if (toTake <= 0)
                return;

            using (IServiceScope jobScope = _serviceScopeFactory.CreateScope())
            {
                IAppRelationalUnitOfWork uow = jobScope.ServiceProvider.GetRequiredService<IAppRelationalUnitOfWork>();

                DateTime now = DateTime.UtcNow;

                if (await _sync.WaitAsync(_configuration.Tick / 2, cancellationToken))
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

                        await uow.SaveChangesAsync(cancellationToken);
                    }
                    finally
                    {
                        _sync.Release();
                    }

                    //Execute if there are tasks
                    if (queuedJobs is List<Job> jobs && jobs.Count > 0)
                    {
                        _logger.LogDebug("Executing {Count} new job(s) in batch ({Processing} processing).", toTake, _processing);

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
            var buckets = new TaskCompletionSource<Task>[tasks.Count()];
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

            foreach (Task inputTask in tasks)
                inputTask.ContinueWith(continuation, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return results;
        }

        private async Task ExecuteJob(Job job, CancellationToken cancellationToken)
        {
            Type? type = Type.GetType(job.Operation);
            if (type is null)
            {
                _logger.LogError("Unknown job of type {Type}", job.Operation);
                return;
            }

            using (IServiceScope jobScope = _serviceScopeFactory.CreateScope())
            {
                IAppRelationalUnitOfWork jobUow = jobScope.ServiceProvider.GetRequiredService<IAppRelationalUnitOfWork>();

                if (jobScope.ServiceProvider.GetService(type) is not IJob jobInstance)
                {
                    _logger.LogError("Unknown job of type {Type}", job.Operation);
                    return;
                }

                _logger.LogTrace("Running job {No}", job.JobNo);

                DateTime startTime = DateTime.UtcNow;
                DateTime? finishTime = null;

                job.StartedOn = startTime;
                job.Status = JobStatuses.Running;
                jobUow.Jobs.Update(job);
                await jobUow.SaveChangesAsync(cancellationToken);

                using (CancellationTokenSource jobCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    if (job.TimeoutAfter is TimeSpan timeout)
                    {
                        jobCts.CancelAfter(timeout);
                    }

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
                            _logger.LogError("Job {Id} {JobNo} timed out after {Time}, and thus was cancelled.", job.Id, job.JobNo, job.TimeoutAfter);
                        }
                        else
                        {
                            _logger.LogError("Job {Id} {JobNo} was cancelled.", job.Id, job.JobNo);
                        }
                    }
                    catch (Exception ex)
                    {
                        job.Exception = JsonConvert.SerializeObject(ex);

                        _logger.LogCritical(ex, "Error occured during job {Id} {JobNo} execution.", job.Id, job.JobNo);
                    }
                    finally
                    {
                        job.StartedOn = startTime;
                        job.FinishedOn = finishTime ?? DateTime.UtcNow;
                        job.Status = finishStatus;
                        jobUow.Jobs.Update(job);

                        await jobUow.SaveChangesAsync(cancellationToken);
                    }
                }

                _logger.LogTrace("Finished job {No}", job.JobNo);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting {Service}.", nameof(JobsProcessingService));

            Timer ??= new TTimer(TickTimer!,
                                 _cts.Token,
                                 _configuration.StartupDelay,
                                 TimeSpan.FromMilliseconds(_configuration.Tick));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping {Service}.", nameof(JobsProcessingService));

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogDebug("Disposing {Service}.", nameof(JobsProcessingService));

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
