namespace AdsPortal.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Commands.Database;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Common;
    using AdsPortal.Helpers;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using Typin.Console;

    public class DbMigrationsService : IDbMigrationsService
    {
        public async ValueTask Migrate(IConsole console)
        {
            using (IWebHost webHost = WebHostHelpers.BuildWebHost())
            {
                string mode = GlobalAppConfig.IsDevMode ? "Development" : "Production";

                Log.ForContext(typeof(MigrateDatabaseCommand)).Warning("Server START: {Mode} mode enabled.", mode);

                try
                {
                    await MigrateDatabase<IRelationalDbContext>(console, webHost);
                }
                catch (Exception ex)
                {
                    Log.ForContext(typeof(MigrateDatabaseCommand)).Fatal(ex, "Host terminated unexpectedly!");
                }
                finally
                {
                    Log.ForContext(typeof(MigrateDatabaseCommand)).Information("Closing web host...");

                    Log.CloseAndFlush();
                }
            }
        }

        private async Task MigrateDatabase<TDbContext>(IConsole console, IWebHost webHost) where TDbContext : IGenericRelationalDbContext
        {
            console.Output.WriteLine($"Applying Entity Framework migrations for {typeof(TDbContext).Name}");

            IServiceScopeFactory serviceScopeFactory = webHost.Services.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

                await dbContext.Provider.Database.MigrateAsync();
                console.Output.WriteLine("All done, closing app");
            }
        }

        public async ValueTask Verify(IConsole console)
        {
            using (IWebHost webHost = WebHostHelpers.BuildWebHost())
            {
                string mode = GlobalAppConfig.IsDevMode ? "Development" : "Production";

                Log.ForContext(typeof(DatabaseVerifyCommand)).Warning("Server START: {Mode} mode enabled.", mode);

                try
                {
                    bool result = await VerifyMigrations<IRelationalDbContext>(console, webHost);
                }
                catch (Exception ex)
                {
                    Log.ForContext(typeof(DatabaseVerifyCommand)).Fatal(ex, "Host terminated unexpectedly!");
                }
                finally
                {
                    Log.ForContext(typeof(DatabaseVerifyCommand)).Information("Closing web host...");

                    Log.CloseAndFlush();
                }
            }
        }

        public async Task<bool> VerifyMigrations<TDbContext>(IConsole console, IWebHost webHost) where TDbContext : IGenericRelationalDbContext
        {
            console.Output.WriteLine($"Validating status of Entity Framework migrations for {typeof(TDbContext).Name}");

            IServiceScopeFactory serviceScopeFactory = webHost.Services.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
                DatabaseFacade database = dbContext.Provider.Database;

                IEnumerable<string> pendingMigrations = await database.GetPendingMigrationsAsync(console.GetCancellationToken());
                IList<string> migrations = pendingMigrations as IList<string> ?? pendingMigrations.ToList();
                if (!migrations.Any())
                {
                    console.Output.WriteLine("No pending migratons");
                    return true;
                }

                console.Output.WriteLine("Pending migratons {0}", migrations.Count);
                foreach (string migration in migrations)
                    console.Output.WriteLine($"\t{migration}");

                return false;
            }
        }
    }
}
