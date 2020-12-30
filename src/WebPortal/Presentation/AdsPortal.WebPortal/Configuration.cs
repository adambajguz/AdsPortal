namespace AdsPortal
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public static IConfigurationBuilder AddConfigs(this IConfigurationBuilder builder, WebHostBuilderContext hostingContext)
        {
            string environmentName = Environment.GetEnvironmentVariable(AspNetCoreEnvironment) ?? "Production";

            builder.AddJsonFile($"appsettings.json", optional: false)
                   .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                   .AddEnvironmentVariables();

            return builder;
        }
    }
}
