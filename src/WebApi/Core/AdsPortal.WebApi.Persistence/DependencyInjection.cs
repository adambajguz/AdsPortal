namespace AdsPortal.WebApi.Persistence
{
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.Persistence;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.FileStorage;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.Repository;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AdsPortal.WebApi.Persistence.AOP;
    using AdsPortal.WebApi.Persistence.Configurations;
    using AdsPortal.WebApi.Persistence.DbContext;
    using AdsPortal.WebApi.Persistence.Extensions;
    using AdsPortal.WebApi.Persistence.FileStorage;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext.Generic;
    using AdsPortal.WebApi.Persistence.Repository;
    using AdsPortal.WebApi.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Persistence.UoW;
    using Castle.DynamicProxy;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        /// <summary>
        /// Persistence layer must be always registered first because hosted services (db migration) will be executed at startup in the same order they are added to the DI container.
        /// </summary>
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new Castle.DynamicProxy.ProxyGenerator());
            services.AddScoped<IInterceptor, LoggingInterceptor>();

            services.AddConfiguration<FileDiskStorageConfiguration>(configuration);
            services.AddSingleton<IFileStorageService, FileDiskStorageService>();
            services.AddSingleton<IIdentifiableFileStorageService, IdentifiableFileStorageService>();

            services.AddConfiguration(configuration, out RelationalDbConfiguration relationalDbConfiguration);

            services.AddDbContext<RelationalDbContext>(options => options.UseSqlServer(relationalDbConfiguration.ConnectionString))
                    .AddScoped<IRelationalDbContext>(c => c.GetRequiredService<RelationalDbContext>())
                    .AddScoped<IGenericRelationalDbContext>(c => c.GetRequiredService<RelationalDbContext>());

            services.AddTransient<IRepositoryFactory, RepositoryFactory>();

            services.AddProxiedScoped<IAppRelationalUnitOfWork, RelationalUnitOfWork>();

            services.AddRepository<Advertisement, IAdvertisementsRepository, AdvertisementsRepository>();
            services.AddRepository<Category, ICategoriesRepository, CategoriesRepository>();
            services.AddRepository<EntityAuditLog, IEntityAuditLogsRepository, EntityAuditLogsRepository>();
            services.AddRepository<Job, IJobsRepository, JobsRepository>();
            services.AddRepository<MediaItem, IMediaItemsRepository, MediaItemsRepository>();
            services.AddRepository<User, IUsersRepository, UsersRepository>();

            services.AddAutomaticMigrator();

            return services;
        }

        public static IHealthChecksBuilder AddPersistenceHealthChecks(this IHealthChecksBuilder healthChecksBuilder)
        {
            healthChecksBuilder.AddDbContextCheck<RelationalDbContext>();

            return healthChecksBuilder;
        }
    }
}
