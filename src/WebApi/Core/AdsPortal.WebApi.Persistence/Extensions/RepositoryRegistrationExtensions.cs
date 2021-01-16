namespace AdsPortal.WebApi.Persistence.Extensions
{
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public static class RepositoryRegistrationExtensions
    {
        public static IServiceCollection AddRepository<TEntity, TService, TImplementation>(this IServiceCollection services)
            where TEntity : class, IBaseRelationalEntity
            where TService : class
            where TImplementation : class, TService, IGenericRelationalRepository<TEntity>
        {
            services.AddTransient<TService, TImplementation>();

            services.AddTransient<IGenericRelationalReadOnlyRepository, TImplementation>();
            services.AddTransient<IGenericRelationalReadOnlyRepository<TEntity>, TImplementation>();

            services.AddTransient<IGenericRelationalRepository, TImplementation>();
            services.AddTransient<IGenericRelationalRepository<TEntity>, TImplementation>();

            return services;
        }
    }
}
