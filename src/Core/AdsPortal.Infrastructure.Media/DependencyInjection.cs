namespace AdsPortal.Infrastructure
{
    using AdsPortal.Application.Interfaces.Media;
    using AdsPortal.Infrastructure.Media.CsvBuilder;
    using AdsPortal.Infrastructure.Media.QRCode;
    using Microsoft.Extensions.DependencyInjection;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureMediaLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICsvBuilderService, CsvBuilderService>();
            services.AddSingleton<IQRCodeService, QRCodeService>();

            return services;
        }
    }
}
