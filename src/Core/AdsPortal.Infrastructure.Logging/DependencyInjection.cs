namespace AdsPortal.Infrastructure.Logging
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLogging(this IServiceCollection services)
        {
            services.AddLogging(cfg =>
            {
                cfg.ClearProviders();
                cfg.AddSerilog(dispose: true);
            });

            return services;
        }
    }
}
