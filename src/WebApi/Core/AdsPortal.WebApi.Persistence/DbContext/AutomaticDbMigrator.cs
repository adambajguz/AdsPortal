namespace AdsPortal.WebApi.Persistence.DbContext
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public sealed class AutomaticDbMigrator : IDisposable
    {
        private readonly RelationalDbContext _relationalDbContext;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger _logger;

        public AutomaticDbMigrator(RelationalDbContext relationalDbContext, IHostApplicationLifetime lifetime, ILogger<AutomaticDbMigrator> logger)
        {
            _relationalDbContext = relationalDbContext;
            _lifetime = lifetime;
            _logger = logger;
        }

        public static async Task MigrateDatabase(IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var relationalDbContext = serviceScope.ServiceProvider.GetRequiredService<RelationalDbContext>();
                var lifetime = serviceScope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<AutomaticDbMigrator>>();

                using (AutomaticDbMigrator migrator = new(relationalDbContext, lifetime, logger))
                {
                    await migrator.MigrateAsync();
                }
            }
        }

        public async Task MigrateAsync()
        {
            _logger.LogInformation("Starting automatic database migrator.");

            if (!await _relationalDbContext.Database.CanConnectAsync())
            {
                _logger.LogWarning("Cannot connect to {ContextName} using {Provider}. Either connection string is invalid or database was not created. Trying to perform initial migration...", nameof(RelationalDbContext), _relationalDbContext.Database.ProviderName);

                IEnumerable<string> initialMigrations = _relationalDbContext.Database.GetMigrations();
                _logger.LogInformation("Database will be created and {Count} migration(s) will be applied: {PendingMigrations}", initialMigrations.Count(), initialMigrations);

                if (await TryMigrate())
                {
                    _logger.LogInformation("Database created and all migration were applied.");
                }
                else
                {
                    _logger.LogInformation("Database creation failed. Application will shutdown.");
                    _lifetime.StopApplication();
                }
            }
            else
            {
                IEnumerable<string> pendingMigrations = await _relationalDbContext.Database.GetPendingMigrationsAsync();

                int count = pendingMigrations.Count();
                _logger.LogInformation("{Count} pending migration(s) will be applied: {PendingMigrations}", count, pendingMigrations);

                if (count > 0)
                {
                    if (await TryMigrate())
                    {
                        _logger.LogInformation("All pending migrations were applied.");
                    }
                    else
                    {
                        _logger.LogInformation("Database migration failed. Application will shutdown.");
                        _lifetime.StopApplication();
                    }
                }
                else
                {
                    _logger.LogInformation("Database up-to-date.");
                }
            }
        }

        private async Task<bool> TryMigrate()
        {
            try
            {
                await _relationalDbContext.Database.MigrateAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to migrate database.");
                return false;
            }
        }

        public void Dispose()
        {
            _logger.LogInformation("Automatic database migrator finished its job and was disposed.");

            _relationalDbContext.Dispose();
        }
    }
}
