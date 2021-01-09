namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Enums;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Configurations;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TTimer = System.Threading.Timer;

    public sealed class JobsProcessingService : IHostedService, IJobSchedulerRunnerService
    {
        private bool _everTicked;
        private int _processing = 0;
        private readonly SemaphoreSlim _sync = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private TTimer? Timer { get; set; }

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly JobSchedulerConfiguration _configuration;
        private readonly IArgumentsSerializer _serializer;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger _logger;

        public Guid InstanceId { get; } = Guid.NewGuid();

        public JobsProcessingService(IServiceScopeFactory serviceProvider,
                                     IOptions<JobSchedulerConfiguration> configuration,
                                     IArgumentsSerializer serializer,
                                     IHostApplicationLifetime lifetime,
                                     ILogger<JobsProcessingService> logger)
        {
            _serviceScopeFactory = serviceProvider;
            _configuration = configuration.Value;
            _serializer = serializer;
            _lifetime = lifetime;
            _logger = logger;
        }

        private async void TickTimer(object state)
        {
            CancellationToken cancellationToken = (CancellationToken)state;

            if (!_everTicked)
            {
                _everTicked = true;

                _logger.LogInformation("{Service} started. Running first iteration...", nameof(JobsProcessingService));
            }

            try
            {
                await CheckAndProcessJobs(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("{Service} job timer tick cancelled.", nameof(JobsProcessingService));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Fatal error occured in {Service} while interacting job queue", nameof(JobsProcessingService));
                _lifetime.StopApplication();
            }
        }

        private async Task CheckAndProcessJobs(CancellationToken cancellationToken)
        {
            int maxConcurent = _configuration.MaxConcurent;
            int concurentBatchSize = _configuration.ConcurentBatchSize;

            int toTake = Math.Min(maxConcurent * concurentBatchSize - _processing, concurentBatchSize);

            if (toTake <= 0)
            {
                return;
            }

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

                        Interlocked.Add(ref _processing, queuedJobs.Count);

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
                        Guid batchGuid = Guid.NewGuid();
                        _logger.LogDebug("Executing {Count} new job(s) in batch {BatchId} ({Processing} processing)", queuedJobs.Count, batchGuid, _processing);

                        foreach (var bucket in Interleaved(jobs, batchGuid, cancellationToken))
                        {
                            await bucket;
                        }

                        _logger.LogDebug("Finished {Count} job(s) in batch {BatchId} ({Processing} processing)", queuedJobs.Count, batchGuid, _processing);
                    }
                }
            }
        }

        //https://devblogs.microsoft.com/pfxteam/processing-tasks-as-they-complete/
        private Task<Task>[] Interleaved(IReadOnlyList<Job> jobs, Guid batchGuid, CancellationToken cancellationToken)
        {
            var buckets = new TaskCompletionSource<Task>[jobs.Count()];
            var results = new Task<Task>[buckets.Length];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new TaskCompletionSource<Task>();
                results[i] = buckets[i].Task;
            }

            int nextTaskIndex = -1;

            foreach (Job job in jobs)
            {
                ExecuteJob(job, batchGuid, cancellationToken).ContinueWith(completed =>
                {
                    Interlocked.Decrement(ref _processing);

                    var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
                    bucket.TrySetResult(completed);
                },
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
            }

            return results;
        }

        private async Task ExecuteJob(Job job, Guid batchGuid, CancellationToken cancellationToken)
        {
            Type? type = Type.GetType(job.Operation);
            if (type is null)
            {
                _logger.LogError("Unknown job of type {Type} in batch {BatchId}", job.Operation, batchGuid);
                return;
            }

            using (IServiceScope jobScope = _serviceScopeFactory.CreateScope())
            {
                IAppRelationalUnitOfWork jobUow = jobScope.ServiceProvider.GetRequiredService<IAppRelationalUnitOfWork>();

                if (jobScope.ServiceProvider.GetService(type) is not IJob jobInstance)
                {
                    _logger.LogError("Invalid job of type {Type} in batch {BatchId}", job.Operation, batchGuid);
                    return;
                }

                _logger.LogTrace("Running job {No} in batch {BatchId}", job.JobNo, batchGuid);

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
                        object? args = _serializer.Deserialize(job.Arguments);

                        startTime = DateTime.UtcNow;
                        await jobInstance.Handle(args, jobCts.Token);
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
                            _logger.LogError("Job {Id} {JobNo} in batch {BatchId} timed out after {Time}, and thus was cancelled", job.Id, job.JobNo, batchGuid, job.TimeoutAfter);
                        }
                        else
                        {
                            _logger.LogError("Job {Id} {JobNo} in batch {BatchId} was cancelled", job.Id, job.JobNo, batchGuid);
                        }
                    }
                    catch (Exception ex)
                    {
                        job.Exception = _serializer.Serialize(ex);
                        finishStatus = JobStatuses.Error;

                        _logger.LogError(ex, "Exception occured during job {Id} {JobNo} execution  in batch {BatchId}", job.Id, job.JobNo, batchGuid);
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

                _logger.LogTrace("Finished job {No} in batch {BatchId}", job.JobNo, batchGuid);
            }
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting {Service}", nameof(JobsProcessingService));

            Timer ??= new TTimer(TickTimer!,
                                 _cts.Token,
                                 _configuration.StartupDelay.Add(TimeSpan.FromSeconds(1)),
                                 TimeSpan.FromMilliseconds(_configuration.Tick));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stopping {Service}", nameof(JobsProcessingService));

            Timer?.Change(Timeout.Infinite, 0);

            // cancel
            _cts.Cancel();

            // then wait until all is done
            _sync.WaitAsync(CancellationToken.None);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Timer?.Dispose();
            Timer = null;
            _cts.Dispose();
        }
    }
}
