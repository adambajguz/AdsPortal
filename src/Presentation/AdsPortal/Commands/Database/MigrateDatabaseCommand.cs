namespace AdsPortal.Commands.Database
{
    using System;
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
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

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

                    Log.ForContext(typeof(RunWebHostCommand)).Warning("Server START: {Mode} mode enabled.", mode);
                }

#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    await MigrateDatabase<IRelationalDbContext>(console, webHost);
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

        private async Task MigrateDatabase<TDbContext>(IConsole console, IWebHost webHost) where TDbContext : IGenericRelationalDbContext
        {
            console.Output.WriteLine($"Applying Entity Framework migrations for {typeof(TDbContext).Name}");

            IServiceScopeFactory serviceScopeFactory = webHost.Services.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

                await dbContext.Provider.Database.MigrateAsync();
                console.Output.WriteLine("All done, closing app");
            }
        }
    }
}
