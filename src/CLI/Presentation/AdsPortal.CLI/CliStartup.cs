namespace AdsPortal.CLI
{
    using AdsPortal.CLI.Application;
    using AdsPortal.CLI.Exceptions;
    using Microsoft.Extensions.DependencyInjection;
    using Typin;
    using Typin.Exceptions;
    using Typin.Modes;

    public class CliStartup : ICliStartup
    {
        public void Configure(CliApplicationBuilder builder)
        {
            builder.ConfigureApplicationLayer();

            builder.AddCommandsFromThisAssembly();

            builder.UseDirectMode(true)
                   .UseInteractiveMode();

            builder.UseExceptionHandler<HttpExceptionHandler>();
            builder.UseExceptionHandler<DefaultExceptionHandler>();

            builder.UseTitle("AdsPortal CLI");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer();
        }
    }
}
