namespace AdsPortal.CLI
{
    using AdsPortal.CLI.Commands;
    using AdsPortal.CLI.Commands.WebHostCommands;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddCLICommands(this IServiceCollection services)
        {
            //Register services
            services.AddSingleton<ICliRuntimeService, CliRuntimeService>();

            //Register commands
            services.AddTransient<ExitCommand>();

            services.AddTransient<TestCommand>();
            services.AddTransient<Test2Command>();
            services.AddTransient<CreateSuperUserCommand>();

            services.AddTransient<WebHostCommand>();
            services.AddTransient<WebHostStartCommand>();
            services.AddTransient<WebHostRestartCommand>();
            services.AddTransient<WebHostStopCommand>();
            services.AddTransient<WebHostStatusCommand>();

            return services;
        }
    }
}
