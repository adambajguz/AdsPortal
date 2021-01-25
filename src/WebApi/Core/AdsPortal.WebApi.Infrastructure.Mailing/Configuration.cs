namespace AdsPortal.WebApi.Infrastructure.Mailing
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        public static IConfigurationBuilder AddMailingConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext, string diffDirectory)
        {
            string environmentName = hostingContext.HostingEnvironment.EnvironmentName;

            builder.AddJsonFile($"{diffDirectory}appsettings.Mailing.json", optional: false, reloadOnChange: false)
                   .AddJsonFile($"{diffDirectory}appsettings.Mailing.{environmentName}.json", optional: true, reloadOnChange: false);

            return builder;
        }
    }
}
