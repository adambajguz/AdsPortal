namespace AdsPortal.Persistence
{
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Common.Extensions;
    using AdsPortal.Persistence.Configurations;
    using AdsPortal.Persistence.DbContext;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.UoW;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<RelationalDbConfiguration>(configuration, out RelationalDbConfiguration relationalDbConfiguration);
            services.AddConfiguration<MongoDbConfiguration>(configuration);

            services.AddDbContext<RelationalDbContext>(options => options.UseSqlServer(relationalDbConfiguration.ConnectionString))
                    .AddScoped<IRelationalDbContext>(c => c.GetRequiredService<RelationalDbContext>());

            services.AddSingleton<IMongoDbContext, MongoDbContext>();

            services.AddScoped<IAppRelationalUnitOfWork, RelationalUnitOfWork>();
            services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();

            return services;
        }

        public static IHealthChecksBuilder AddPersistenceHealthChecks(this IHealthChecksBuilder healthChecksBuilder)
        {
            healthChecksBuilder.AddDbContextCheck<RelationalDbContext>();
            //healthChecksBuilder.AddMongoDb();

            return healthChecksBuilder;
        }
    }
}
