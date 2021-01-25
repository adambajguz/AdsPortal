namespace AdsPortal.WebApi.Infrastructure.JobScheduler
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        public static IConfigurationBuilder AddJobSchedulerConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext, string diffDirectory)
        {
            string environmentName = hostingContext.HostingEnvironment.EnvironmentName;

            builder.AddJsonFile($"{diffDirectory}appsettings.JobScheduler.json", optional: false, reloadOnChange: false)
                   .AddJsonFile($"{diffDirectory}appsettings.JobScheduler.{environmentName}.json", optional: true, reloadOnChange: false);

            return builder;
        }
    }
}
