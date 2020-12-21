namespace MagicCRUD
{
    using System;
    using System.Net.Http.Headers;
    using MagicCRUD.Builder;
    using MagicCRUD.Configurations;
    using MagicCRUD.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMagicCRUD(this IServiceCollection services, Action<MagicCRUDBuilder> configuration)
        {
            MagicCRUDBuilder builder = new();
            configuration?.Invoke(builder);
            MagicCRUDConfiguration built = builder.Build();

            services.AddSingleton<MagicCRUDConfiguration>(built);

            services.AddScoped<AuthTokenHolder>();
            services.AddScoped<ApiService>();

            services.AddHttpClient("MagicCRUDAPI", (services, cfg) =>
            {
                AuthTokenHolder tokenHolder = services.GetRequiredService<AuthTokenHolder>();
                MagicCRUDConfiguration configuration = services.GetRequiredService<MagicCRUDConfiguration>();

                cfg.BaseAddress = new Uri(configuration.BaseApiPath);

                if (tokenHolder.HasToken)
                    cfg.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenHolder.Token);
            });

            return services;
        }
    }
}
