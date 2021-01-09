namespace AdsPortal.Infrastructure.JobScheduler
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        public static IConfigurationBuilder AddMailingConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext, string environmentName)
        {
            builder.AddJsonFile($"appsettings.Mailing.json", optional: false)
                   .AddJsonFile($"appsettings.Mailing.{environmentName}.json", optional: true);

            return builder;
        }
    }
}
