namespace AdsPortal.Shared.Extensions.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;

    public static class ServiceCollectionHostedServicesExtensions
    {
        public static IServiceCollection AddMultipleInstanceHostedService<TImplementation>(this IServiceCollection services)
            where TImplementation : class, IHostedService
        {
            services.AddSingleton<IHostedService, TImplementation>();

            return services;
        }

        public static IServiceCollection AddMultipleInstanceHostedService<TService>(this IServiceCollection services, Func<IServiceProvider, IHostedService> implementationFactory)
        {
            services.AddSingleton<IHostedService>(implementationFactory);

            return services;
        }

        public static IServiceCollection TryAddHostedService<TImplementation>(this IServiceCollection services)
            where TImplementation : class, IHostedService
        {
            services.TryAddSingleton<IHostedService, TImplementation>();

            return services;
        }

        public static IServiceCollection TryAddHostedService<TService>(this IServiceCollection services, Func<IServiceProvider, IHostedService> implementationFactory)
        {
            services.TryAddSingleton<IHostedService>(implementationFactory);

            return services;
        }
    }
}
