namespace AdsPortal.WebApi.Rest
{
    using AdsPortal.WebApi.Rest.Configurations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddRestApi(this IServiceCollection services)
        {
            services.AddSwagger();
            services.AddDistributedMemoryCache();

            return services;
        }

        public static IApplicationBuilder ConfigureRestApi(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureSwagger();

            return app;
        }
    }
}
