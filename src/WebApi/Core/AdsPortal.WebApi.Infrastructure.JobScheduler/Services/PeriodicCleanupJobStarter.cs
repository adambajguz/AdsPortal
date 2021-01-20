namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Jobs;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class PeriodicCleanupJobStarter : IHostedService
    {
        private const bool IsFileStorageListingTestEnabled = false;
        private const bool IsJobSchedulerTestEnabled = false;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public PeriodicCleanupJobStarter(IServiceScopeFactory serviceScopeFactory, ILogger<PeriodicCleanupJobStarter> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting {Name} hosted service", nameof(PeriodicCleanupJobStarter));

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                IJobSchedulingService jobScheduling = scope.ServiceProvider.GetRequiredService<IJobSchedulingService>();
                await jobScheduling.ScheduleSingleAsync<PeriodicCleanupJob>(priority: int.MaxValue, cancellationToken: cancellationToken);
            }

            _logger.LogInformation("Finished {Name} hosted service", nameof(PeriodicCleanupJobStarter));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
