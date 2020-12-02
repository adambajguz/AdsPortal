namespace AdsPortal
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using AdsPortal.CLI;
    using AdsPortal.CLI.Interactive;
    using AdsPortal.Commands;
    using AdsPortal.Common;
    using CliFx;
    using Microsoft.Extensions.DependencyInjection;

    internal static class Program
    {
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection? services = new ServiceCollection();

            services.AddSingleton<IConsole, SystemConsole>(); // or `VirtualConsole`, depending on what you need

            services.AddCLICommands()
                    .AddFoundationCLICommands();

            return services.BuildServiceProvider();
        }

        public static async Task<int> Main(string[] args)
        {
            //Console.WriteLine(string.Join(' ', args));

            CliApplicationBuilder cliBuilder = new CliApplicationBuilder();
            cliBuilder.AllowDebugMode(true)
                      .AllowPreviewMode(true);

            //Add DI
            IServiceProvider serviceProvider = GetServiceProvider();
            IConsole console = serviceProvider.GetService<IConsole>();

            cliBuilder.UseConsole(console) // make sure CliFx is using the same instance of IConsole
                      .UseTypeActivator(serviceProvider.GetService);

            // Set Assemblies
            cliBuilder.AddCommandsFrom(new Assembly[] {
                                           typeof(CLI.DependencyInjection).Assembly,
                                           typeof(Program).Assembly
                                       });

            // Set info
            cliBuilder.UseTitle($"Help for {GlobalAppConfig.AppInfo.InteractiveCLI}")
                      .UseVersionText(GlobalAppConfig.AppInfo.AppVersionText)
                      .UseDescription(GlobalAppConfig.AppInfo.AppShortDescription);

            return await cliBuilder.BuildInteractive(null, GlobalAppConfig.AppInfo.AppNameWithVersionCopyright, GlobalAppConfig.AppInfo.InteractiveCLIWithVersion + " Ready")
                                   .RunAsync();
        }
    }
}