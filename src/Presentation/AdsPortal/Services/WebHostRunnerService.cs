namespace AdsPortal.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Common;
    using AdsPortal.Helpers;
    using AdsPortal.Infrastructure.Logging.Helpers;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;
    using Serilog.Events;

    public class WebHostRunnerService : IWebHostRunnerService
    {
        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            using (IWebHost webHost = GetWebHost())
            {
                try
                {
                    await webHost.RunAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    Log.ForContext<WebHostRunnerService>().Fatal(ex, "Host terminated unexpectedly!");
                }
                finally
                {
                    Log.ForContext<WebHostRunnerService>().Information("Closing web host...");
                    Log.CloseAndFlush();
                }
            }
        }

        public async Task<IWebHost> StartAsync(CancellationToken cancellationToken = default)
        {
            SerilogConfigurationHelper.ConsoleLoggingLevelSwitch.MinimumLevel = LogEventLevel.Debug;// + 1;

            IWebHost webHost = GetWebHost();

            try
            {
                await webHost.StartAsync(cancellationToken);

                Log.ForContext<WebHostRunnerService>().Information("Background mode initialized!");
                SerilogConfigurationHelper.ConsoleLoggingLevelSwitch.MinimumLevel = LogEventLevel.Fatal;// + 1;
            }
            catch (Exception ex)
            {
                Log.ForContext<WebHostRunnerService>().Fatal(ex, "Host terminated unexpectedly!");
                Log.CloseAndFlush();
            }

            return webHost;
        }

        private IWebHost GetWebHost()
        {
            IWebHost webHost = WebHostHelpers.BuildWebHost();

            string mode = GlobalAppConfig.IsDevMode ? "Development" : "Production";
            Log.ForContext<WebHostRunnerService>().Warning("Server START: {Mode} mode enabled.", mode);

            return webHost;
        }

    }
}
