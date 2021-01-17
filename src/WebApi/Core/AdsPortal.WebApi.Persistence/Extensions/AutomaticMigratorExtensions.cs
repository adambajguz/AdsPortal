namespace AdsPortal.WebApi.Persistence.Extensions
{
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Persistence.Migrator;
    using Microsoft.Extensions.DependencyInjection;

    public static class AutomaticMigratorExtensions
    {
        public static IServiceCollection AddAutomaticMigrator(this IServiceCollection services)
        {
            services.AddTransient<AutomaticMigrator>();
            services.AddMultipleInstanceHostedService<AutomaticMigratorHostedService>();

            return services;
        }
    }
}
