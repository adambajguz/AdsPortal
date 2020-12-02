namespace AdsPortal.CLI.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AdsPortal.Common;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using AdsPortal.RuntimeArguments;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("database verify", Description = "Verify Entity Framework migrations.")]
    public class DatabaseVerifyCommand : ICommand
    {
        public async ValueTask ExecuteAsync(IConsole console)
        {
            using (IWebHost webHost = WebHostHelpers.BuildWebHost())
            {
#pragma warning disable CS0162 // Unreachable code detected
                {
                    string mode = GlobalAppConfig.IsDevMode ? "Development" : "Production";

                    Log.ForContext(typeof(DatabaseVerifyCommand)).Warning("Server START: {Mode} mode enabled.", mode);
                }

#pragma warning disable CA1031 // Do not catch general exception types
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
#pragma warning restore CA1031 // Do not catch general exception types
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

                console.Output.WriteLine("Pending migratons {0}", migrations.Count());
                foreach (string migration in migrations)
                    console.Output.WriteLine($"\t{migration}");

                return false;
            }
        }
    }
}
