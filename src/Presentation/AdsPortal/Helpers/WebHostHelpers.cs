namespace AdsPortal.Helpers
{
    using AdsPortal;
    using AdsPortal.Common;
    using AdsPortal.Infrastructure.Logging.Configuration;
    using FluentValidation;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;

    public static class WebHostHelpers
    {
        public static IWebHost BuildWebHost()
        {
            SerilogConfiguration.ConfigureSerilog();

            Log.ForContext(typeof(WebHostHelpers)).Information(GlobalAppConfig.AppInfo.AppNameWithVersionCopyright);
            Log.ForContext(typeof(WebHostHelpers)).Information("Loading web host...");

            //Custom PropertyNameResolver to remove neasted Property in Classes e.g. Data.Id in UpdateUserCommandValidator.Model
            //ValidatorOptions.Global.PropertyNameResolver = (type, member, expression) => member?.Name;
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            //ValidatorOptions.LanguageManager.Culture = new CultureInfo("en");

            Log.ForContext(typeof(WebHostHelpers)).Information("FluentValidation's support for localization disabled. Default English messages are forced to be used, regardless of the thread's CurrentUICulture.");

            return CreateWebHostBuilder().Build();
        }

        private static IWebHostBuilder CreateWebHostBuilder()
        {
            Log.ForContext(typeof(WebHostHelpers)).Information("Starting web host...");

            return WebHost.CreateDefaultBuilder()
                          .UseKestrel()
                          .UseStaticWebAssets()
                          .UseStartup<Startup>()
                          .UseSerilog()
                          .UseUrls("http://*:2137", "http://*:2138");
        }
    }
}
