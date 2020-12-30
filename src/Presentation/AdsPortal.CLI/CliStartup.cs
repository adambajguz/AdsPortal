namespace AdsPortal.CLI
{
    using System;
    using System.Net.Http.Headers;
    using AdsPortal.CLI.Services;
    using AdsPortal.Common;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Modes;

    public class CliStartup : ICliStartup
    {
        public void Configure(CliApplicationBuilder builder)
        {
            builder.AddCommandsFromThisAssembly();

            builder.UseDirectMode(true)
                   .UseInteractiveMode();

            builder.UseTitle(AppInfo.InteractiveCLI)
                   .UseVersionText(AppInfo.AppVersionText)
                   .UseDescription(AppInfo.AppShortDescription);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AuthTokenHolder>();

            services.AddHttpClient("api", (services, cfg) =>
            {
                AuthTokenHolder tokenHolder = services.GetRequiredService<AuthTokenHolder>();

                cfg.BaseAddress = new Uri("https://localhost:5001/api/");

                if (tokenHolder.HasToken)
                    cfg.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenHolder.Token);
            });
        }
    }
}
