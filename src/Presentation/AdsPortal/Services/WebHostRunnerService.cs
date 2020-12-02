namespace AdsPortal.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Common;
    using AdsPortal.Infrastructure.Logging.Configuration;
    using AdsPortal.RuntimeArguments;
    using CliFx;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;
    using Serilog.Events;

    public class WebHostRunnerService : IWebHostRunnerService
    {
        private readonly IConsole _console;

        public WebHostRunnerService(IConsole console)
        {
            _console = console;
        }

        public IWebHost GetWebHost()
        {
            IWebHost webHost = WebHostHelpers.BuildWebHost();

            string mode = GlobalAppConfig.IsDevMode ? "Development" : "Production";
            Log.ForContext<WebHostRunnerService>().Warning("Server START: {Mode} mode enabled.", mode);

            return webHost;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            using (IWebHost webHost = GetWebHost())
            {
#pragma warning disable CA1031 // Do not catch general exception types
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
#pragma warning restore CA1031 // Do not catch general exception types
            }
        }

        public async Task<IWebHost> StartAsync(CancellationToken cancellationToken = default)
        {
            SerilogConfiguration.ConsoleLoggingLevelSwitch.MinimumLevel = LogEventLevel.Debug;// + 1;

            IWebHost webHost = GetWebHost();

#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                await webHost.StartAsync(cancellationToken);

                Log.ForContext<WebHostRunnerService>().Information("Background mode initiated!");
                SerilogConfiguration.ConsoleLoggingLevelSwitch.MinimumLevel = LogEventLevel.Fatal;// + 1;
                //Log.ForContext<WebHostRunnerService>().Fatal("Background mode initiated!");
            }
            catch (Exception ex)
            {
                Log.ForContext<WebHostRunnerService>().Fatal(ex, "Host terminated unexpectedly!");
                Log.CloseAndFlush();
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return webHost;
        }
    }
}
