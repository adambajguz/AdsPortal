namespace AdsPortal.WebApi.Persistence
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        public static IConfigurationBuilder AddPersistenceConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext, string environmentName)
        {
            builder.AddJsonFile($"appsettings.Persistence.json", optional: false)
                   .AddJsonFile($"appsettings.Persistence.{environmentName}.json", optional: true);

            return builder;
        }
    }
}
