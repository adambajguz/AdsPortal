namespace AdsPortal.Commands.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AdsPortal.Commands;
    using AdsPortal.Common;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using AdsPortal.RuntimeArguments;
    using CliFx;
    using CliFx.Attributes;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

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

                    Log.ForContext(typeof(RunWebHostCommand)).Warning("Server START: {Mode} mode enabled.", mode);
                }

#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    bool result = await VerifyMigrations<IRelationalDbContext>(console, webHost);
                }
                catch (Exception ex)
                {
                    Log.ForContext(typeof(RunWebHostCommand)).Fatal(ex, "Host terminated unexpectedly!");
                }
                finally
                {
                    Log.ForContext(typeof(RunWebHostCommand)).Information("Closing web host...");

                    Log.CloseAndFlush();
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }
        }

        public async Task<bool> VerifyMigrations<TDbContext>(IConsole console, IWebHost webHost) where TDbContext : IGenericRelationalDbContext
        {
            console.Output.WriteLine($"Validating status of Entity Framework migrations for {typeof(TDbContext).Name}");

            IServiceScopeFactory serviceScopeFactory = webHost.Services.GetService<IServiceScopeFactory>();

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
