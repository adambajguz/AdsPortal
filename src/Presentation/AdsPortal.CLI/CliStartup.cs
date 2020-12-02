namespace AdsPortal.CLI
{
    using AdsPortal.Common;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Modes;

    public class CliStartup : ICliStartup
    {
        public void Configure(CliApplicationBuilder builder)
        {
            builder.AddCommandsFromThisAssembly();

            builder.UseDirectMode(true, configuration: (cfg) =>
                   {

                   })
                   .UseInteractiveMode(configuration: (cfg) =>
                   {

                   });

            builder.UseTitle(GlobalAppConfig.AppInfo.InteractiveCLI)
                   .UseVersionText(GlobalAppConfig.AppInfo.AppVersionText)
                   .UseDescription(GlobalAppConfig.AppInfo.AppShortDescription);
        }

        public void ConfigureServices(IServiceCollection services)
        {

        }
    }
}
