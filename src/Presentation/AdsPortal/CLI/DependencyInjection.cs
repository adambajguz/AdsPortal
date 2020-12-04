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
            services.AddSingleton<IDbMigrationsService, DbMigrationsService>();
            services.AddSingleton<IWebHostRunnerService, WebHostRunnerService>();
            services.AddSingleton<IBackgroundWebHostProviderService, BackgroundWebHostProviderService>();

            return services;
        }
    }
}
