namespace MagicModels
{
    using System;
    using MagicModels.Builder;
    using MagicModels.Interfaces;
    using MagicModels.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMagicModels(this IServiceCollection services, Action<MagicModelsBuilder> builder)
        {
            MagicModelsBuilder builderObj = new();
            builder?.Invoke(builderObj);
            MagicModelsConfiguration configuration = builderObj.Build();

            services.AddSingleton<MagicModelsConfiguration>(configuration);
            services.AddScoped<IModelRenderService, ModelRenderService>();

            return services;
        }
    }
}
