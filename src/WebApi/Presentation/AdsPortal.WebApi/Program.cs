namespace AdsPortal.WebApi
{
    using System;
    using System.Diagnostics;
    using AdsPortal.Shared.Infrastructure.Logging;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilderWithConfiguration(args)
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHostBuilder CreateWebHostBuilderWithConfiguration(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseKestrel((hostingContext, serverOptions) =>
                          {

                          })
                          .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              //Add core config to build logger
                              config.AddCoreConfigs(hostingContext);

                              //Initialize serilog so that it can log config.AddConfigs(hostingContext) and other initialization errors
                              hostingContext.ConfigureSerilog(config);

                              config.AddConfigs(hostingContext);
                          })
                          .ConfigureLogging((hostingContext, config) =>
                          {
                              config.ClearProviders();
                              config.AddSerilog();
                          })
                          .UseSerilog()
                          //.ConfigureKestrel(x =>
                          //{
                          //    //Overrides the default settings binding the development server to localhost:5001
                          //    //Make sure to also update Project Options > Run > Configurations > Default > ASP.NET Core > App URL. You'll need to close / re-open the solution for the settings to take effect.
                          //    //x.Listen(IPAddress.Any, 5001);

                          //    //x.AddServerHeader = false;
                          //})
                          .UseStartup<Startup>();
        }
    }
}
