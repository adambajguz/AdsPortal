namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Jobs
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
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class PeriodicCleanupJob : IJob
    {
        private static readonly JobStatuses[] JobFinishedStatuses = new[] { JobStatuses.TimedOut, JobStatuses.Success, JobStatuses.MaxRetriesReached };

        private readonly IAppRelationalUnitOfWork _uow;
        private readonly IJobSchedulingService _jobScheduling;
        private readonly JobSchedulerConfiguration _configuration;
        private readonly ILogger _logger;

        public PeriodicCleanupJob(IAppRelationalUnitOfWork uow, IJobSchedulingService jobScheduling, IOptions<JobSchedulerConfiguration> configuration, ILogger<PeriodicCleanupJob> logger)
        {
            _uow = uow;
            _jobScheduling = jobScheduling;
            _configuration = configuration.Value;
            _logger = logger;
        }

        public async ValueTask Handle(Guid jobId, object? args, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            DateTime maxAge = now.AddHours((-1) * _configuration.MaxAge);

            await Remove(maxAge, cancellationToken);

            await _jobScheduling.ScheduleSingleAsync<PeriodicCleanupJob>(priority: int.MaxValue,
                                                                         postponeTo: DateTime.UtcNow.AddHours(_configuration.CleanupPeriod),
                                                                         runAfter: jobId,
                                                                         cancellationToken: cancellationToken);
        }

        private async Task Remove(DateTime maxAge, CancellationToken cancellationToken)
        {
            List<Job> toRemove = await _uow.Jobs.AllAsync(filter => JobFinishedStatuses.Contains(filter.Status) && filter.FinishedOn <= maxAge,
                                                          orderBy => orderBy.OrderByDescending(x => x.JobNo),
                                                          cancellationToken: cancellationToken);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogInformation("{Count} jobs will be removed (FinishedOn <= {VisibleTo}): {Jobs}", toRemove.Count, maxAge, toRemove.Select(x => x.JobNo));
            }
            else
            {
                _logger.LogInformation("{Count} jobs will be removed (FinishedOn <= {VisibleTo})", toRemove.Count, maxAge);
            }

            List<Guid> removedGuids = new();

            foreach (Job job in toRemove)
            {
                if (removedGuids.Contains(job.Id))
                {
                    continue;
                }

                LinkedList<Job> recursiveRemove = new();

                Job? tmp = job;
                while (tmp != null)
                {
                    recursiveRemove.AddFirst(tmp);
                    removedGuids.Add(tmp.Id);
                    tmp = tmp.RunAfter;
                }

                if (recursiveRemove.Count > 1)
                {
                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug("Recursive removal of {Count} job(s): {Jobs}", recursiveRemove.Count, toRemove.Select(x => x.JobNo));
                    }
                    else
                    {
                        _logger.LogDebug("Recursive removal of {Count} job(s): {Jobs}", recursiveRemove.Count);
                    }
                }

                int counter = 0;
                foreach (var r in recursiveRemove)
                {
                    var related = await _uow.Jobs.AllAsync(x => x.RunAfterId == r.Id);

                    _uow.Jobs.Remove(r);
                    counter += await _uow.SaveChangesAsync(cancellationToken);
                }

                if (recursiveRemove.Count > 1)
                {
                    _logger.LogDebug("Recursive removal of {Count} job(s) finished. Saved {Saved} records.", recursiveRemove.Count, counter);
                }
                else
                {
                    _logger.LogDebug("Removed job {Id}. Saved {Saved} records.", job.Id, counter);
                }
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogInformation("Removal of {Count} jobs (FinishedOn <= {VisibleTo}) finished: {Jobs}", toRemove.Count, maxAge, toRemove.Select(x => x.JobNo));
            }
            else
            {
                _logger.LogInformation("Removal of {Count} jobs (FinishedOn <= {VisibleTo}) finished", toRemove.Count, maxAge);
            }
        }
    }
}
