namespace AdsPortal.CLI
{
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddFoundationCLIServices(this IServiceCollection services)
        {
            //Register services
            services.AddSingleton<IWebHostRunnerService, WebHostRunnerService>();
            services.AddScoped<IBackgroundWebHostProviderService, BackgroundWebHostProviderService>();

            return services;
        }
    }
}
