namespace AdsPortal
{
    using AdsPortal.Infrastructure.JobScheduler;
    using AdsPortal.ManagementUI;
    using AdsPortal.Persistence;
    using AdsPortal.WebAPI;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        public static IConfigurationBuilder AddConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext, string environmentName)
        {
            builder.AddJsonFile($"appsettings.json", optional: false)
                   .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                   .AddPersistenceConfigs(hostingContext, environmentName)
                   .AddJobSchedulerConfigs(hostingContext, environmentName)
                   .AddWebAPIConfigs(hostingContext, environmentName)
                   .AddManagementUIConfigs(hostingContext, environmentName)
                   .AddEnvironmentVariables();

            return builder;
        }
    }
}
