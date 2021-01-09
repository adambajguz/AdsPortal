namespace AdsPortal.Infrastructure
{
    using AdsPortal.Infrastructure.Media.QRCode;
    using AdsPortal.WebApi.Application.Interfaces.Media;
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
