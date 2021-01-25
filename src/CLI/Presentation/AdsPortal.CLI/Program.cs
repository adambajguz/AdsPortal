namespace AdsPortal.CLI
{
    using System;
    using System.Threading.Tasks;
    using Serilog;
    using Serilog.Exceptions;
    using Typin;

    internal static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                return await new CliApplicationBuilder()
                    .UseStartup<CliStartup>()
                    .ConfigureLogging((builder) =>
                    {
                        Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .Enrich.WithMachineName()
                            .Enrich.WithEnvironmentUserName()
                            .Enrich.FromLogContext()
                            .Enrich.WithExceptionDetails()
                            .Enrich.WithProcessId()
                            .Enrich.WithThreadId()
                            .WriteTo.File(path: "./logs/CLI_.log",
                                          outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj} <s:{SourceContext} pid:{ProcessId} t:{ThreadId}>{NewLine}{Exception}",
                                          rollingInterval: RollingInterval.Day,
                                          retainedFileCountLimit: 64,
                                          fileSizeLimitBytes: 10485760,
                                          flushToDiskInterval: TimeSpan.FromSeconds(1),
                                          buffered: true)
                            .CreateLogger();

                        builder.AddSerilog(dispose: true);
                    })
                    .Build()
                    .RunAsync();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}