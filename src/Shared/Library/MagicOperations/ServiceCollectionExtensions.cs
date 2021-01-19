namespace MagicOperations
{
    using System;
    using MagicModels;
    using MagicModels.Interfaces;
    using MagicModels.Services;
    using MagicOperations.Builder;
    using MagicOperations.Interfaces;
    using MagicOperations.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMagicOperations(this IServiceCollection services, Action<MagicOperationsBuilder> builder)
        {
            MagicOperationsBuilder builderObj = new();
            builder?.Invoke(builderObj);
            var cfg = builderObj.Build();

            services.AddSingleton<MagicModelsConfiguration>(cfg.Item1);
            services.AddScoped<IModelRenderService, ModelRenderService>();

            services.TryAddSingleton<ISerializer, DefaultSerializer>();
            services.AddSingleton<MagicOperationsConfiguration>(cfg.Item2);
            services.AddSingleton<IOperationModelFactory, OperationModelFactory>();
            services.AddScoped<IMagicRenderService, MagicRenderService>();
            services.AddScoped<IMagicApiService, MagicApiService>();
            services.AddScoped<AuthTokenHolder>();

            services.AddHttpClient("MagicOperationsAPI", (services, c) =>
            {
                c.BaseAddress = new Uri(cfg.Item2.BaseApiPath);
            });

            return services;
        }
    }
}
