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
    using Serilog;
    using TTimer = System.Threading.Timer;

    public sealed class JobSchedulerRunnerService : IHostedService, IJobSchedulerRunnerService
    {
        private readonly SemaphoreSlim _sync = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private TTimer? Timer { get; set; }
        private IAppRelationalUnitOfWork Uow { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private IServiceScope Scope { get; }

        public JobSchedulerRunnerService(IServiceScopeFactory serviceProvider)
        {
            ServiceScopeFactory = serviceProvider;

            Scope = ServiceScopeFactory.CreateScope();
            Uow = Scope.ServiceProvider.GetService<IAppRelationalUnitOfWork>();
        }

        private async void TickTimer(object state)
        {
            CancellationToken cancellationToken = (CancellationToken)state;
            if (await _sync.WaitAsync(0, cancellationToken))
            {
                try
                {
                    await CheckAndProcessJobs(16, cancellationToken);
                }
                finally
                {
                    _sync.Release();
                }
            }
        }

        private async Task CheckAndProcessJobs(int jobPoolSize, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;

            int count = await Uow.Jobs.GetCountAsync(filter: x => x.Status == JobStatuses.Queued && (x.PostponeTo == null || x.PostponeTo >= now));

            List<Job> queuedJobs = await Uow.Jobs.AllAsync(filter: x => x.Status == JobStatuses.Queued && (x.PostponeTo == null || x.PostponeTo >= now),
                                                           orderBy: (order) => order.OrderBy(x => x.JobNo).ThenBy(x => x.Priority),
                                                           noTracking: true,
                                                           take: jobPoolSize,
                                                           cancellationToken: cancellationToken);

            if (queuedJobs.Count > 0)
            {
                Log.ForContext<JobSchedulingService>().Debug("{Count} Jobs in queue", count);

                IEnumerable<Task> tasks = queuedJobs.Select(x => ExecuteJob(x, cancellationToken));

                await Task.WhenAll(tasks);
            }
        }

        private async Task ExecuteJob(Job job, CancellationToken cancellationToken)
        {
            Type? type = Type.GetType(job.Operation);
            if (type is null)
            {
                Log.ForContext<JobSchedulingService>().Error("Unknown job of type {Type}", job.Operation);
                return;
            }

            using (var jobScope = ServiceScopeFactory.CreateScope())
            {
                IAppRelationalUnitOfWork jobUow = jobScope.ServiceProvider.GetService<IAppRelationalUnitOfWork>();
                IJob? jobInstance = (IJob?)jobScope.ServiceProvider.GetService(type);

                if (jobInstance is null)
                {
                    Log.ForContext<JobSchedulingService>().Error("Unknown job of type {Type}", job.Operation);
                    return;
                }

                DateTime startTime = DateTime.UtcNow;
                DateTime? finishTime = null;

                job.StartedOn = startTime;
                job.Status = JobStatuses.Running;
                jobUow.Jobs.Update(job);
                await jobUow.SaveChangesAsync();

#pragma warning disable CA1031 // Do not catch general exception types
                using (CancellationTokenSource jobCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    TimeSpan timeout = TimeSpan.FromSeconds(500);
                    jobCts.CancelAfter(timeout); // TODO: set from settings, job calss attribute or when schedulling a command

                    JobStatuses finishStatus = JobStatuses.Error;
                    try
                    {
                        startTime = DateTime.UtcNow;
                        await jobInstance.Handle(job.OperationArguments, jobCts.Token).ConfigureAwait(false);
                        finishTime = DateTime.UtcNow;
                        finishStatus = JobStatuses.Success;
                    }
                    catch (TaskCanceledException)
                    {
                        finishTime = DateTime.UtcNow;
                        bool timedOut = !_cts.IsCancellationRequested && jobCts.IsCancellationRequested;
                        finishStatus = timedOut ? JobStatuses.TimedOut : JobStatuses.Cancelled;

                        if (timedOut)
                            Log.ForContext<JobSchedulingService>().Error("Job {Id} {JobNo} timed out after {Time}, and thus was cancelled.", job.Id, job.JobNo, timeout);
                        else
                            Log.ForContext<JobSchedulingService>().Error("Job {Id} {JobNo} was cancelled.", job.Id, job.JobNo);
                    }
                    catch (Exception ex)
                    {
                        Log.ForContext<JobSchedulingService>().Error(ex, "Error occured during job {Id} {JobNo} execution.", job.Id, job.JobNo);
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
#pragma warning restore CA1031 // Do not catch general exception types
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.ForContext<JobSchedulingService>().Debug("Starting {Service}.", nameof(JobSchedulerRunnerService));

            Timer ??= new TTimer(TickTimer!, _cts.Token, TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(20)); //TODO add appsettings section

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

            Scope.Dispose();
        }
    }
}
