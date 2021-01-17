namespace AdsPortal.WebApi.Persistence.Migrator
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class AutomaticMigratorHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public AutomaticMigratorHostedService(IServiceScopeFactory serviceScopeFactory, ILogger<AutomaticMigratorHostedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting automatic database migrator hosted service.");

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                AutomaticMigrator migrator = scope.ServiceProvider.GetRequiredService<AutomaticMigrator>();

                await migrator.MigrateAsync();
            }

            _logger.LogInformation("Finished automatic database migrator hosted service.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
