namespace AdsPortal.WebApi.Grpc
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddGrpcApi(this IServiceCollection services)
        {
            return services;
        }

        public static IApplicationBuilder ConfigureGrpcApi(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            return app;
        }
    }
}
