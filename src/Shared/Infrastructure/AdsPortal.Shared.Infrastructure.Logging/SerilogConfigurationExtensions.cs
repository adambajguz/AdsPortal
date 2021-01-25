namespace AdsPortal.Shared.Infrastructure.Logging
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Exceptions;

    public static class SerilogConfigurationExtensions
    {
        public static ILoggingBuilder AddSerilog(this ILoggingBuilder builder)
        {
            builder.AddSerilog(dispose: true);

            return builder;
        }

        public static void ConfigureSerilog(this WebHostBuilderContext hostingContext, IConfiguration? configuration = null)
        {
            configuration ??= hostingContext.Configuration;

            string appName = configuration.GetValue<string>("Application:Name");
            string environmentName = hostingContext.HostingEnvironment.EnvironmentName;
            string projectName = hostingContext.HostingEnvironment.ApplicationName;

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("app", appName)
                .Enrich.WithProperty("project", projectName)
                .Enrich.WithProperty("env", environmentName)
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .CreateLogger();

            Log.Logger = logger;

            logger.Information("Starting application {project} {env} ...");
        }

        public static void ConfigureSerilog(this WebHostBuilderContext hostingContext, IConfigurationBuilder configurationBuilder)
        {
            IConfiguration configuration = configurationBuilder.Build();

            hostingContext.ConfigureSerilog(configuration);
        }
    }
}
