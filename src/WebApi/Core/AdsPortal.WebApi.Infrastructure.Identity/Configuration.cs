namespace AdsPortal.WebApi.Infrastructure.Identity
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        public static IConfigurationBuilder AddIdentityConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext)
        {
            string environmentName = hostingContext.HostingEnvironment.EnvironmentName;

            builder.AddJsonFile($"appsettings.Identity.json", optional: false, reloadOnChange: false)
                   .AddJsonFile($"appsettings.Identity.{environmentName}.json", optional: true, reloadOnChange: false);

            return builder;
        }
    }
}
