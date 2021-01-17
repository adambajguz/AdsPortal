namespace AdsPortal.WebApi.Persistence.Migrator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Persistence.DbContext;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public sealed class AutomaticMigrator : IDisposable
    {
        private readonly RelationalDbContext _relationalDbContext;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger _logger;

        public AutomaticMigrator(RelationalDbContext relationalDbContext, IHostApplicationLifetime lifetime, ILogger<AutomaticMigrator> logger)
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
                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<AutomaticMigrator>>();

                using (AutomaticMigrator migrator = new(relationalDbContext, lifetime, logger))
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

                try
                {
                    await _relationalDbContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Database creation failed. Application will shutdown.");
                    _lifetime.StopApplication();

                    throw;
                }
            }
            else
            {
                IEnumerable<string> pendingMigrations = await _relationalDbContext.Database.GetPendingMigrationsAsync();

                int count = pendingMigrations.Count();
                _logger.LogInformation("{Count} pending migration(s) will be applied: {PendingMigrations}", count, pendingMigrations);

                if (count > 0)
                {
                    try
                    {
                        await _relationalDbContext.Database.MigrateAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, "Database migration failed. Application will shutdown.");
                        _lifetime.StopApplication();

                        throw;
                    }
                }
                else
                {
                    _logger.LogInformation("Database up-to-date.");
                }
            }
        }

        public void Dispose()
        {
            _logger.LogInformation("Automatic database migrator finished its job and was disposed.");

            _relationalDbContext.Dispose();
        }
    }
}
