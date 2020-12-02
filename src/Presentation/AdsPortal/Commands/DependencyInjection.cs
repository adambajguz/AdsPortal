namespace AdsPortal.Commands
{
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Commands.Database;
    using AdsPortal.Commands.Logging;
    using AdsPortal.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddFoundationCLICommands(this IServiceCollection services)
        {
            //Register services
            services.AddSingleton<IWebHostRunnerService, WebHostRunnerService>();
            services.AddScoped<IBackgroundWebHostProviderService, BackgroundWebHostProviderService>();

            //Register commands
            services.AddTransient<DatabaseVerifyCommand>();
            services.AddTransient<MigrateDatabaseCommand>();
            services.AddTransient<RunWebHostCommand>();

            services.AddTransient<GetLogLevelCommand>();
            services.AddTransient<SetLogLevelCommand>();

            return services;
        }
    }
}
