namespace AdsPortal.WebApi.Client
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAdsPortalWebApiClient(this IServiceCollection services, string baseUrl)
        {
            services.TryAddTransient<WebApiClientAggregator>();

            services.TryAddTransient<HttpClient>();

            services.AddTransient<IAdvertisementClient>((provider) => new AdvertisementClient(baseUrl, provider.GetRequiredService<HttpClient>()));
            services.AddTransient<ICategoryClient>((provider) => new CategoryClient(baseUrl, provider.GetRequiredService<HttpClient>()));
            services.AddTransient<IEntityAuditLogClient>((provider) => new EntityAuditLogClient(baseUrl, provider.GetRequiredService<HttpClient>()));
            services.AddTransient<IMediaItemClient>((provider) => new MediaItemClient(baseUrl, provider.GetRequiredService<HttpClient>()));
            services.AddTransient<IUserClient>((provider) => new UserClient(baseUrl, provider.GetRequiredService<HttpClient>()));

            return services;
        }

        public static IServiceCollection AddAdsPortalWebApiClient(this IServiceCollection services, string baseUrl, HttpClient httpClient)
        {
            services.TryAddTransient<WebApiClientAggregator>();

            services.AddTransient<IAdvertisementClient>((provider) => new AdvertisementClient(baseUrl, httpClient));
            services.AddTransient<ICategoryClient>((provider) => new CategoryClient(baseUrl, httpClient));
            services.AddTransient<IEntityAuditLogClient>((provider) => new EntityAuditLogClient(baseUrl, httpClient));
            services.AddTransient<IMediaItemClient>((provider) => new MediaItemClient(baseUrl, httpClient));
            services.AddTransient<IUserClient>((provider) => new UserClient(baseUrl, httpClient));

            return services;
        }

        public static IServiceCollection AddAdsPortalWebApiClient(this IServiceCollection services, string baseUrl, Func<IServiceProvider, HttpClient> action)
        {
            services.TryAddTransient<WebApiClientAggregator>();

            services.AddTransient<IAdvertisementClient>((provider) =>
            {
                HttpClient httpClient = action(provider);
                return new AdvertisementClient(baseUrl, httpClient);
            });

            services.AddTransient<ICategoryClient>((provider) =>
            {
                HttpClient httpClient = action(provider);
                return new CategoryClient(baseUrl, httpClient);
            });

            services.AddTransient<IEntityAuditLogClient>((provider) =>
            {
                HttpClient httpClient = action(provider);
                return new EntityAuditLogClient(baseUrl, httpClient);
            });

            services.AddTransient<IMediaItemClient>((provider) =>
            {
                HttpClient httpClient = action(provider);
                return new MediaItemClient(baseUrl, httpClient);
            });

            services.AddTransient<IUserClient>((provider) =>
            {
                HttpClient httpClient = action(provider);
                return new UserClient(baseUrl, httpClient);
            });

            return services;
        }
    }
}
