namespace AdsPortal.WebApi
{
    using System;
    using AdsPortal.Shared.Infrastructure.Logging;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
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
                              config.AddConfigs(hostingContext);
                          })
                          .ConfigureLogging((hostingContext, config) =>
                          {
                              string appName = hostingContext.Configuration.GetValue<string>("Application:Name");
                              string projectName = typeof(Program).Assembly.GetName().Name ?? string.Empty;

                              hostingContext.Configuration.ConfigureSerilog(appName, projectName);
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
