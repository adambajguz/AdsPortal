namespace MagicOperations
{
    using System;
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
            services.AddSingleton<IMagicRouteResolver, MagicRouteResolver>();
            services.AddScoped<AuthTokenHolder>();
            services.AddScoped<MagicApiService>();

            services.AddHttpClient("MagicOperationsAPI", (services, cfg) =>
            {
                cfg.BaseAddress = new Uri(configuration.BaseApiPath);

                //Why does it throw: System.InvalidOperationException: Cannot resolve scoped service 'MagicOperations.Services.AuthTokenHolder' from root provider.
                //AuthTokenHolder tokenHolder = services.GetRequiredService<AuthTokenHolder>();

                //if (tokenHolder.HasToken)
                //    cfg.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenHolder.Token);
            });

            return services;
        }
    }
}
