namespace AdsPortal.CLI.Database
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Common;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using AdsPortal.RuntimeArguments;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("database migrate", Description = "Apply Entity Framework migrations.")]
    public class MigrateDatabaseCommand : ICommand
    {
        public async ValueTask ExecuteAsync(IConsole console)
        {
            using (IWebHost webHost = WebHostHelpers.BuildWebHost())
            {
#pragma warning disable CS0162 // Unreachable code detected
                {
                    string mode = GlobalAppConfig.IsDevMode ? "Development" : "Production";

                    Log.ForContext(typeof(MigrateDatabaseCommand)).Warning("Server START: {Mode} mode enabled.", mode);
                }

#pragma warning disable CA1031 // Do not catch general exception types
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
#pragma warning restore CA1031 // Do not catch general exception types
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
    }
}
