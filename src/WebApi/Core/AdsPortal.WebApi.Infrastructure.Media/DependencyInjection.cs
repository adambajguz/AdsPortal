namespace AdsPortal.WebApi.Infrastructure.Media
{
    using AdsPortal.WebApi.Application.Interfaces.Media;
    using AdsPortal.WebApi.Infrastructure.Media.QRCode;
    using Microsoft.Extensions.DependencyInjection;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureMediaLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IQRCodeService, QRCodeService>();

            return services;
        }
    }
}
