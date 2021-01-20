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
    using Microsoft.Extensions.Logging;

    public class PeriodicCleanupJob : IJob
    {
        private static readonly JobStatuses[] JobFinishedStatuses = new[] { JobStatuses.TimedOut, JobStatuses.Success, JobStatuses.MaxRetriesReached };

        private readonly IAppRelationalUnitOfWork _uow;
        private readonly IJobSchedulingService _jobScheduling;
        private readonly ILogger _logger;

        public PeriodicCleanupJob(IAppRelationalUnitOfWork uow, IJobSchedulingService jobScheduling, ILogger<PeriodicCleanupJob> logger)
        {
            _uow = uow;
            _jobScheduling = jobScheduling;
            _logger = logger;
        }

        public async ValueTask Handle(Guid jobId, object? args, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            DateTime maxAge = now;//.AddHours(-12);

            List<Job> toRemove = await _uow.Jobs.AllAsync(filter => JobFinishedStatuses.Contains(filter.Status) && filter.FinishedOn <= maxAge,
                                                          cancellationToken: cancellationToken);
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogInformation("Removing {Count} old jobs (FinishedOn <= {VisibleTo}): {Jobs}.", toRemove.Count, maxAge, toRemove.Select(x => x.JobNo));
            }
            else
            {
                _logger.LogInformation("Removing {Count} old jobs (FinishedOn <= {VisibleTo}).", toRemove.Count, maxAge);
            }

            _uow.Jobs.RemoveMultiple(toRemove);
            await _uow.SaveChangesAsync(cancellationToken);

            await _jobScheduling.ScheduleSingleAsync<PeriodicCleanupJob>(priority: int.MaxValue,
                                                                         postponeTo: DateTime.UtcNow.AddMinutes(60),
                                                                         runAfter: jobId,
                                                                         cancellationToken: cancellationToken);
        }
    }
}
