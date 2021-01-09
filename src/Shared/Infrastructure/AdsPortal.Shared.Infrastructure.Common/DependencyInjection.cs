namespace AdsPortal.Shared.Infrastructure.Common
{
    using AdsPortal.Shared.Common.Interfaces;
    using AdsPortal.Shared.Common.Interfaces.StringSimilarity;
    using AdsPortal.Shared.Infrastructure.Common.Caching;
    using AdsPortal.Shared.Infrastructure.Common.MachineInfo;
    using AdsPortal.Shared.Infrastructure.Common.StringSimilarityComparer;
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
