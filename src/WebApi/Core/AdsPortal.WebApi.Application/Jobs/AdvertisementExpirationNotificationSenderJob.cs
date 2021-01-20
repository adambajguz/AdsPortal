namespace AdsPortal.WebApi.Application.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Domain.EmailTemplates;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using Microsoft.Extensions.Logging;

    public class AdvertisementExpirationNotificationSenderJob : IJob
    {
        private readonly IAppRelationalUnitOfWork _uow;
        private readonly IJobSchedulingService _jobScheduling;
        private readonly ILogger _logger;

        public AdvertisementExpirationNotificationSenderJob(IAppRelationalUnitOfWork uow, IJobSchedulingService jobScheduling, ILogger<AdvertisementExpirationNotificationSenderJob> logger)
        {
            _uow = uow;
            _jobScheduling = jobScheduling;
            _logger = logger;
        }

        public async ValueTask Handle(Guid jobId, object? args, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            DateTime maxVisibleTo = now.AddDays(1);

            List<Advertisement> adsAboutToExpire = await _uow.Advertisements.AllAsync(filter => filter.IsPublished &&
                                                                                                filter.VisibleTo <= maxVisibleTo && filter.VisibleTo > now &&
                                                                                                (filter.LastExpirationNotification == null || filter.LastExpirationNotification <= maxVisibleTo && filter.LastExpirationNotification > now),
                                                                                      cancellationToken: cancellationToken);
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogInformation("Sending notification for {Count} advertisement(s) that are about to expire (VisibleTo <= {VisibleTo}): {Ads}.", adsAboutToExpire.Count, maxVisibleTo, adsAboutToExpire);
            }
            else
            {
                _logger.LogInformation("Sending notification for {Count} advertisement(s) that are about to expire (VisibleTo <= {VisibleTo}).", adsAboutToExpire.Count, maxVisibleTo);
            }

            foreach (Advertisement ad in adsAboutToExpire)
            {
                User user = await _uow.Users.SingleByIdAsync(ad.AuthorId, noTracking: true, cancellationToken);

                SendEmailJobArguments jobArgs = new()
                {
                    Email = user.Email,
                    Template = new AdvertisementAboutToExpireEmail { UserName = user.Name, AdvertisementTitle = ad.Title, AdvertisementVisibleTo = ad.VisibleTo }
                };

                ad.LastExpirationNotification = now;

                await _jobScheduling.ScheduleAsync<SendEmailJob>(operationArguments: jobArgs, cancellationToken: cancellationToken);
            }

            await _uow.SaveChangesAsync(cancellationToken);

            await _jobScheduling.ScheduleSingleAsync<AdvertisementExpirationNotificationSenderJob>(priority: 10,
                                                                                                   postponeTo: DateTime.UtcNow.AddMinutes(5),
                                                                                                   runAfter: jobId,
                                                                                                   cancellationToken: cancellationToken);
        }
    }
}
