namespace AdsPortal.Shared.Infrastructure.Logging
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Exceptions;

    public static class SerilogConfigurationExtensions
    {
        public static ILoggingBuilder AddSerilog(this ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.AddSerilog(dispose: true);

            return builder;
        }

        public static IConfiguration ConfigureSerilog(this IConfiguration configuration, string appName, string projectName)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("app", appName)
                .Enrich.WithProperty("project", projectName)
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .CreateLogger();

            Log.Logger = logger;

            logger.Information("Starting application {project}...");

            return configuration;
        }
    }
}
