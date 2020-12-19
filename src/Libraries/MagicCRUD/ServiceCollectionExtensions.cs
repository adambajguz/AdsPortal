namespace MagicCRUD
{
    using System;
    using MagicCRUD.Builder;
    using MagicCRUD.Configurations;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMagicCRUD(this IServiceCollection services, Action<MagicCRUDBuilder> configuration)
        {
            MagicCRUDBuilder builder = new();
            configuration?.Invoke(builder);
            MagicCRUDConfiguration built = builder.Build();

            services.AddSingleton<MagicCRUDConfiguration>(built);

            return services;
        }
    }
}
