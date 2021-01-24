namespace AdsPortal.WebApi.Persistence.Extensions
{
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;
    using AdsPortal.WebApi.Persistence.AOP;
    using Microsoft.Extensions.DependencyInjection;

    public static class RepositoryRegistrationExtensions
    {
        public static IServiceCollection AddRepository<TEntity, TService, TImplementation>(this IServiceCollection services)
            where TEntity : class, IBaseRelationalEntity
            where TService : class
            where TImplementation : class, TService, IGenericRelationalRepository<TEntity>
        {
            services.AddProxiedTransient<TService, TImplementation>();

            services.AddProxiedTransient<IGenericRelationalReadOnlyRepository, TImplementation>();
            services.AddProxiedTransient<IGenericRelationalReadOnlyRepository<TEntity>, TImplementation>();

            services.AddProxiedTransient<IGenericRelationalRepository, TImplementation>();
            services.AddProxiedTransient<IGenericRelationalRepository<TEntity>, TImplementation>();

            return services;
        }
    }
}
