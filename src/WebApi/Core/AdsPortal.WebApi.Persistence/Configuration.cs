namespace AdsPortal.WebApi.Persistence
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        public static IConfigurationBuilder AddPersistenceConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext)
        {
            string environmentName = hostingContext.HostingEnvironment.EnvironmentName;

            builder.AddJsonFile($"appsettings.Persistence.json", optional: false, reloadOnChange: false)
                   .AddJsonFile($"appsettings.Persistence.{environmentName}.json", optional: true, reloadOnChange: false);

            return builder;
        }
    }
}
