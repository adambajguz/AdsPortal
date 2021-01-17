namespace AdsPortal.WebApi.Application.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Application.Jobs;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class PeriodicJobStarter : IHostedService
    {
        private const bool IsFileStorageListingTestEnabled = false;
        private const bool IsJobSchedulerTestEnabled = false;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public PeriodicJobStarter(IServiceScopeFactory serviceScopeFactory, ILogger<PeriodicJobStarter> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting {Name} hosted service", nameof(PeriodicJobStarter));

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                IJobSchedulingService jobScheduling = scope.ServiceProvider.GetRequiredService<IJobSchedulingService>();
                await jobScheduling.ScheduleAsync<AdvertisementExpirationNotificationSenderJob>(priority: 10, cancellationToken: cancellationToken);
            }

            _logger.LogInformation("Finished {Name} hosted service", nameof(PeriodicJobStarter));
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
