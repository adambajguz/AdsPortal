namespace AdsPortal.Analytics
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class FeatureConfiguration
    {
        public static IConfigurationBuilder AddAnalyticsConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext, string environmentName)
        {
            builder.AddJsonFile($"appsettings.Analytics.json", optional: false)
                   .AddJsonFile($"appsettings.Analytics.{environmentName}.json", optional: true);

            return builder;
        }
    }
}
