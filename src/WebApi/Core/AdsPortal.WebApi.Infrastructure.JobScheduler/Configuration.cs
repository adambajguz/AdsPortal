namespace AdsPortal.WebApi.Infrastructure.JobScheduler
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        public static IConfigurationBuilder AddJobSchedulerConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext, string environmentName)
        {
            builder.AddJsonFile($"appsettings.JobScheduler.json", optional: false)
                   .AddJsonFile($"appsettings.JobScheduler.{environmentName}.json", optional: true);

            return builder;
        }
    }
}
