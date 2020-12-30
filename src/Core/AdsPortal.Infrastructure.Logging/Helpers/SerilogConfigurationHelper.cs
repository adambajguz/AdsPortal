namespace AdsPortal.Infrastructure.Logging.Helpers
{
    using System;
    using AdsPortal.Common;
    using AdsPortal.Common.Extensions;
    using AdsPortal.Infrastructure.Logging.Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Sentry;
    using Serilog;
    using Serilog.Configuration;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Serilog.Sinks.SystemConsole.Themes;

    public static class SerilogConfigurationHelper
    {
        public static LoggingLevelSwitch ConsoleLoggingLevelSwitch { get; set; } = new LoggingLevelSwitch();

        public static ILogger ConfigureSerilog(IConfiguration configuration, IWebHostEnvironment environment, string environmentName)
        {
            LoggingConfiguration loggerSettigns = configuration.GetValue<LoggingConfiguration>();

            //TODO: use ILogger or ILogger<T> in the app
            //TODO: consider adding seq
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                                         .Enrich.FromLogContext()
                                         .Enrich.WithExceptionDetails()
                                         .Enrich.WithProcessId()
                                         .Enrich.WithThreadId()
                                         .SetLoggingLevels(loggerSettigns, environment)
                                         .ConfigureSentry(loggerSettigns, environmentName)
                                         .SetOutput(loggerSettigns);

            Logger logger = loggerConfiguration.CreateLogger();
            Log.Logger = logger;

            logger.ForContext(typeof(SerilogConfigurationHelper)).Information("Environment Name file: {EnvironmentName}", environmentName);
            logger.ForContext(typeof(SerilogConfigurationHelper)).Information("Logs are stored under: {Path}", loggerSettigns.FullPath);

            return logger;
        }

        #region Private LoggerConfiguration Extensions
        private static LoggerConfiguration SetOutput(this LoggerConfiguration loggerConfiguration, LoggingConfiguration loggerSettigns)
        {
            if (loggerSettigns.IsConsoleOutputEnabled)
            {
                try
                {
                    loggerConfiguration.WriteTo.Async(WriteToConsole(loggerSettigns));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(new string('=', 25));
                    Console.WriteLine($"Cannot enable console logging. {ex}");
                    Console.WriteLine(new string('=', 25));
                }
            }

            if (loggerSettigns.IsFileOutputEnabled)
            {
                try
                {
                    loggerConfiguration.WriteTo.Async(WriteToFile(loggerSettigns));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(new string('=', 25));
                    Console.WriteLine($"Cannot enable file logging. {ex}");
                    Console.WriteLine(new string('=', 25));
                }
            }

            return loggerConfiguration;
        }

        private static LoggerConfiguration SetLoggingLevels(this LoggerConfiguration loggerConfiguration, LoggingConfiguration loggerSettigns, IWebHostEnvironment environment)
        {
            //TODO: replace custom config with Serilog.Settings.Configuration
            if (environment.IsDevelopment())
            {
                if (loggerSettigns.LogEverythingInDev)
                {
                    loggerConfiguration.MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Verbose)
                                       .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose);

                    loggerConfiguration.MinimumLevel.Verbose();

                    ConsoleLoggingLevelSwitch.MinimumLevel = LogEventLevel.Verbose;
                }
                else
                {
                    loggerConfiguration.MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                                       .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

                    loggerConfiguration.MinimumLevel.Debug();
                    ConsoleLoggingLevelSwitch.MinimumLevel = LogEventLevel.Debug;
                }
            }
            else
            {
                loggerConfiguration.MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                                   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);

                loggerConfiguration.MinimumLevel.Information();
                ConsoleLoggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
            }

            return loggerConfiguration;
        }

        private static LoggerConfiguration ConfigureSentry(this LoggerConfiguration loggerConfiguration, LoggingConfiguration loggerSettigns, string environmentName)
        {
            if (!loggerSettigns.SentryEnabled)
                return loggerConfiguration;

            loggerConfiguration.WriteTo.Sentry(o =>
            {
                // Debug and higher are stored as breadcrumbs (default is Information)
                o.MinimumBreadcrumbLevel = LogEventLevel.Debug;
                // Warning and higher is sent as event (default is Error)
                o.MinimumEventLevel = LogEventLevel.Warning;
                o.Dsn = new Dsn(loggerSettigns.SentryDSN);
                o.AttachStacktrace = true;
                o.SendDefaultPii = true;
                o.Release = AppInfo.SentryReleaseVersion;
                o.ReportAssemblies = true;
                o.Environment = environmentName;
            });

            return loggerConfiguration;
        }

        private static Action<LoggerSinkConfiguration> WriteToConsole(LoggingConfiguration loggerSettigns)
        {
            return x => x.Console(outputTemplate: loggerSettigns.ConsoleOutputTemplate,
                                  theme: AnsiConsoleTheme.Code, levelSwitch: ConsoleLoggingLevelSwitch);
        }

        private static Action<LoggerSinkConfiguration> WriteToFile(LoggingConfiguration loggerSettigns)
        {
            return x => x.File(loggerSettigns.FullPath,
                               outputTemplate: loggerSettigns.FileOutputTemplate,
                               fileSizeLimitBytes: loggerSettigns.FileSizeLimitInBytes,
                               rollingInterval: RollingInterval.Day,
                               rollOnFileSizeLimit: true,
                               flushToDiskInterval: TimeSpan.FromSeconds(loggerSettigns.FlushIntervalInSeconds),
                               retainedFileCountLimit: loggerSettigns.RetainedFileCountLimit,
                               shared: true);
        }
        #endregion
    }
}
