namespace AdsPortal.WebApi.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.FileStorage;
    using AdsPortal.WebApi.Application.Jobs;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class DeveloperSandbox : IHostedService
    {
        private const bool IsFileStorageListingTestEnabled = false;
        private const bool IsJobSchedulerTestEnabled = false;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public DeveloperSandbox(IServiceScopeFactory serviceScopeFactory, ILogger<DeveloperSandbox> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
#pragma warning disable CS0162 // Unreachable code detected
            _logger.LogInformation("Starting {Name} hosted service", nameof(DeveloperSandbox));

            if (IsFileStorageListingTestEnabled)
            {
                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    _logger.LogInformation("Starting {Name}", nameof(FileStorageListingTest));
                    FileStorageListingTest(scope, cancellationToken);
                    _logger.LogInformation("Finished {Name}", nameof(FileStorageListingTest));
                }
            }

            if (IsJobSchedulerTestEnabled)
            {
                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    _logger.LogInformation("Starting {Name}", nameof(JobSchedulerTest));
                    await JobSchedulerTest(scope, cancellationToken);
                    _logger.LogInformation("Finished {Name}", nameof(JobSchedulerTest));
                }
            }

            _logger.LogInformation("Finished {Name} hosted service", nameof(DeveloperSandbox));
#pragma warning restore CS0162 // Unreachable code detected
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private void FileStorageListingTest(IServiceScope scope, CancellationToken cancellationToken)
        {
            IFileStorageService x = scope.ServiceProvider.GetRequiredService<IFileStorageService>();

            IEnumerable<string> listing = x.GetDirectoryListing(recursive: true, cancellationToken: cancellationToken);
            _logger.LogInformation("File storage listing: {Files}", listing);
        }

        private async Task JobSchedulerTest(IServiceScope scope, CancellationToken cancellationToken)
        {
            IJobSchedulingService jobScheduling = scope.ServiceProvider.GetRequiredService<IJobSchedulingService>();

            for (int priority = 1; priority <= 2; ++priority)
            {
                for (int i = 0; i < 300; ++i)
                {
                    if ((i + 1) % 100 == 0)
                    {
                        _logger.LogInformation("Added {Count} jobs with priority {Priority}", i + 1, priority);
                    }

                    await jobScheduling.ScheduleAsync<TestJob>(priority: priority, cancellationToken: cancellationToken);
                }
            }
        }
    }
}
