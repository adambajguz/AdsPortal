namespace MagicOperations
{
    using System;
    using System.Net.Http.Headers;
    using MagicOperations.Configurations;
    using MagicOperations.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMagicOperations(this IServiceCollection services, Action<MagicOperationsBuilder> builder)
        {
            MagicOperationsBuilder builderObj = new();
            builder?.Invoke(builderObj);
            MagicOperationsConfiguration configuration = builderObj.Build();

            services.AddSingleton<MagicOperationsConfiguration>(configuration);
            services.AddScoped<AuthTokenHolder>();
            services.AddScoped<ApiService>();

            services.AddHttpClient("MagicOperationsAPI", (services, cfg) =>
            {
                AuthTokenHolder tokenHolder = services.GetRequiredService<AuthTokenHolder>();
                cfg.BaseAddress = new Uri(configuration.BaseApiPath);

                if (tokenHolder.HasToken)
                    cfg.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenHolder.Token);
            });

            return services;
        }
    }
}
