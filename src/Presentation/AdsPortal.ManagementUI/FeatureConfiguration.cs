namespace AdsPortal.ManagementUI
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class FeatureConfiguration
    {
        public static IConfigurationBuilder AddManagementUIConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext, string environmentName)
        {
            builder.AddJsonFile($"appsettings.ManagementUI.json", optional: false)
                   .AddJsonFile($"appsettings.ManagementUI.{environmentName}.json", optional: true);

            return builder;
        }
    }
}
