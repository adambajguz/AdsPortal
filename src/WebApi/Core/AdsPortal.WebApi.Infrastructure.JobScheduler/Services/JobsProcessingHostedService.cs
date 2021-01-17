namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Enums;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Configurations;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TTimer = System.Threading.Timer;

    public sealed class JobsProcessingHostedService : IJobsProcessingHostedService
    {
        private bool _everTicked;
        private int _processing = 0;
        private static readonly SemaphoreSlim _sync = new(1, 1);
        private readonly CancellationTokenSource _cts = new();

        private TTimer? Timer { get; set; }

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly JobSchedulerConfiguration _configuration;
        private readonly IArgumentsSerializer _serializer;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger _logger;

        public Guid InstanceId { get; } = Guid.NewGuid();
        private Guid[]? KnownInstancesIds { get; set; }

        public JobsProcessingHostedService(IServiceScopeFactory serviceProvider,
                                           IOptions<JobSchedulerConfiguration> configuration,
                                           IArgumentsSerializer serializer,
                                           IHostApplicationLifetime lifetime,
                                           ILogger<JobsProcessingHostedService> logger)
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

                _logger.LogInformation("{Service} ({InstanceId}) started. Running first iteration...", nameof(JobsProcessingHostedService), InstanceId);
            }

            try
            {
                //_logger.LogTrace("{Service} ({InstanceId}) job timer tick.", nameof(JobsProcessingHostedService), InstanceId);
                await CheckAndProcessJobs(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("{Service} ({InstanceId}) job timer tick cancelled.", nameof(JobsProcessingHostedService), InstanceId);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Fatal error occured in {Service} ({InstanceId}) while interacting with job queue", nameof(JobsProcessingHostedService), InstanceId);
                _lifetime.StopApplication();
            }
        }

        private async Task CheckAndProcessJobs(CancellationToken cancellationToken)
        {
            int maxTries = _configuration.MaxTries <= 0 ? 1 : _configuration.MaxTries;

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
                    KnownInstancesIds ??= jobScope.ServiceProvider.GetServices<IHostedService>()
                                                                  .Select(x => x as IJobsProcessingHostedService)
                                                                  .Where(x => x is not null)
                                                                  .Select(x => x!.InstanceId)
                                                                  .ToArray();

                    List<Job>? queuedJobs = null;

                    //Only one task can access db
                    try
                    {
                        queuedJobs = await uow.Jobs.AllAsync(filter: x => (x.Status == JobStatuses.Queued ||
                                                                           x.Status == JobStatuses.Cancelled ||
                                                                           x.Status == JobStatuses.Error ||
                                                                           (!KnownInstancesIds.Contains(x.Instance ?? Guid.Empty) && (x.Status == JobStatuses.Taken || x.Status == JobStatuses.Running))) &&
                                                                          x.Tries <= maxTries &&
                                                                          (x.PostponeTo == null || x.PostponeTo <= now),
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
                        _logger.LogDebug("Executing {Count} new job(s) in batch {BatchId} in {InstanceId} service instance ({Processing} processing)", queuedJobs.Count, batchGuid, InstanceId, _processing);

                        foreach (var bucket in Interleaved(jobs, batchGuid, cancellationToken))
                        {
                            await bucket;
                        }

                        _logger.LogDebug("Finished {Count} job(s) in batch {BatchId} in {InstanceId} service instance ({Processing} processing)", queuedJobs.Count, batchGuid, InstanceId, _processing);
                    }
                }
            }
        }

        //https://devblogs.microsoft.com/pfxteam/processing-tasks-as-they-complete/
        private Task<Task>[] Interleaved(IReadOnlyList<Job> jobs, Guid batchGuid, CancellationToken cancellationToken)
        {
            var buckets = new TaskCompletionSource<Task>[jobs.Count];
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

                    if (completed.IsFaulted)
                    {
                        _logger.LogError(completed.Exception, "Fatal error occured during job processing in {InstanceId} service instance", InstanceId);
                    }
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
                _logger.LogError("Unknown job of type {Type} in batch {BatchId} in {InstanceId} service instance", job.Operation, batchGuid, InstanceId);
                return;
            }

            using (IServiceScope jobScope = _serviceScopeFactory.CreateScope())
            {
                IAppRelationalUnitOfWork jobUow = jobScope.ServiceProvider.GetRequiredService<IAppRelationalUnitOfWork>();

                if (jobScope.ServiceProvider.GetService(type) is not IJob jobInstance)
                {
                    _logger.LogError("Invalid job of type {Type} in batch {BatchId} in {InstanceId} service instance", job.Operation, batchGuid, InstanceId);
                    return;
                }

                _logger.LogTrace("Running job {No} in batch {BatchId} in {InstanceId} service instance", job.JobNo, batchGuid, InstanceId);

                jobUow.Jobs.EnsureTracked(job);

                job.StartedOn = DateTime.UtcNow;
                job.Status = JobStatuses.Running;
                ++job.Tries;

                jobUow.Jobs.Update(job);
                await jobUow.SaveChangesAsync(cancellationToken);

                using (CancellationTokenSource jobCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    if (job.TimeoutAfter is TimeSpan timeout)
                    {
                        jobCts.CancelAfter(timeout);
                    }

                    try
                    {
                        object? args = _serializer.Deserialize(job.Arguments);
                        await jobInstance.Handle(args, jobCts.Token);

                        job.Status = JobStatuses.Success;
                        _logger.LogTrace("Finished job {No} in batch {BatchId} in {InstanceId} service instance after {TryCount} tries.", job.JobNo, batchGuid, InstanceId, job.Tries);
                    }
                    catch (TaskCanceledException)
                    {
                        bool timedOut = !_cts.IsCancellationRequested && jobCts.IsCancellationRequested;
                        job.Status = timedOut ? JobStatuses.TimedOut : JobStatuses.Cancelled;

                        if (timedOut)
                        {
                            _logger.LogError("Job {Id} {JobNo} in batch {BatchId} in {InstanceId} service instance timed out after {Time}, and thus was cancelled", job.Id, job.JobNo, batchGuid, InstanceId, job.TimeoutAfter);
                        }
                        else
                        {
                            _logger.LogError("Job {Id} {JobNo} in batch {BatchId} in {InstanceId} service instance was cancelled", job.Id, job.JobNo, batchGuid, InstanceId);
                        }
                    }
                    catch (Exception ex)
                    {
                        DateTime retryDate = DateTime.UtcNow.AddSeconds(5 * job.Tries);

                        job.Exception ??= string.Empty;
                        job.Exception += $"<{job.Tries}, {job.PostponeTo?.ToString() ?? "null"}>" + _serializer.Serialize(ex);
                        job.PostponeTo = retryDate;
                        job.Status = JobStatuses.Error;

                        _logger.LogError(ex, "Exception occured during job {Id} {JobNo} execution in batch {BatchId} in {InstanceId} service instance. Try no. {TryCount} scheduled for {RetryDate}.", job.Id, job.JobNo, batchGuid, InstanceId, job.Tries + 1, retryDate);
                    }
                    finally
                    {
                        job.FinishedOn = DateTime.UtcNow;

                        jobUow.Jobs.Update(job);
                        await jobUow.SaveChangesAsync(CancellationToken.None);
                    }
                }
            }
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting {Service} with instance id {InstanceId}", nameof(JobsProcessingHostedService), InstanceId);

            Timer ??= new TTimer(TickTimer!,
                                 _cts.Token,
                                 _configuration.StartupDelay.Add(TimeSpan.FromSeconds(1)),
                                 TimeSpan.FromMilliseconds(_configuration.Tick));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stopping {Service} with instance id {InstanceId}", nameof(JobsProcessingHostedService), InstanceId);

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
