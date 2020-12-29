namespace AdsPortal.Helpers
{
    using System;
    using AdsPortal;
    using AdsPortal.Common;
    using AdsPortal.Infrastructure.Logging.Helpers;
    using FluentValidation;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;

    public static class WebHostHelpers
    {
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public static IWebHost BuildWebHost()
        {
            //Custom PropertyNameResolver to remove neasted Property in Classes e.g. Data.Id in UpdateUserCommandValidator.Model
            //ValidatorOptions.Global.PropertyNameResolver = (type, member, expression) => member?.Name;
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            //ValidatorOptions.LanguageManager.Culture = new CultureInfo("en");

            Log.ForContext(typeof(WebHostHelpers)).Information("FluentValidation's support for localization disabled. Default English messages are forced to be used, regardless of the thread's CurrentUICulture.");

            return CreateWebHostBuilder().Build();
        }

        private static IWebHostBuilder CreateWebHostBuilder()
        {
            string environmentName = Environment.GetEnvironmentVariable(AspNetCoreEnvironment) ?? "Production";

            return WebHost.CreateDefaultBuilder()
                          .UseKestrel((hostingContext, serverOptions) =>
                          {

                          })
                          .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              config.AddConfigs(hostingContext, environmentName);
                          })
                          .ConfigureLogging((hostingContext, config) =>
                          {
                              SerilogConfigurationHelper.ConfigureSerilog(hostingContext.Configuration, hostingContext.HostingEnvironment, environmentName);

                              Log.ForContext(typeof(WebHostHelpers)).Information(GlobalAppConfig.AppInfo.AppNameWithVersionCopyright);
                              Log.ForContext(typeof(WebHostHelpers)).Information("Loading web host...");
                          })
                          .UseStaticWebAssets()
                          .UseStartup<Startup>()
                          .UseSerilog();
        }
    }
}
