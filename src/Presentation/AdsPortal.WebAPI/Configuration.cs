namespace AdsPortal.WebAPI
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        public static IConfigurationBuilder AddWebAPIConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext, string environmentName)
        {
            builder.AddJsonFile($"appsettings.WebAPI.json", optional: false)
                   .AddJsonFile($"appsettings.WebAPI.{environmentName}.json", optional: true);

            return builder;
        }
    }
}
