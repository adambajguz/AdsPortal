namespace AdsPortal.Infrastructure.Common
{
    using AdsPortal.Common.Interfaces;
    using AdsPortal.Common.Interfaces.StringSimilarity;
    using AdsPortal.Infrastructure.Cache;
    using AdsPortal.Infrastructure.Common.MachineInfo;
    using AdsPortal.Infrastructure.Common.StringSimilarityComparer;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureCrossCuttingLayer(this IServiceCollection services)
        {
            services.AddSingleton<IMachineInfoService, MachineInfoService>();
            services.AddSingleton<ICachingService, CachingService>();
            services.AddSingleton<IStringSimilarityComparerService, StringSimilarityComparerService>();

            return services;
        }
    }
}
