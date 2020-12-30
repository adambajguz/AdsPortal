namespace AdsPortal.CLI.Application
{
    using System;
    using System.Net.Http.Headers;
    using AdsPortal.CLI.Application.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;

    public static class DependencyInjection
    {
        public static void ConfigureApplicationLayer(this CliApplicationBuilder builder)
        {
            builder.AddCommandsFromThisAssembly();
        }

        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddSingleton<AuthTokenHolder>();

            services.AddHttpClient("api", (services, cfg) =>
            {
                AuthTokenHolder tokenHolder = services.GetRequiredService<AuthTokenHolder>();

                cfg.BaseAddress = new Uri("https://localhost:5002/api/");

                if (tokenHolder.HasToken)
                    cfg.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenHolder.Token);
            });

            return services;
        }
    }
}
