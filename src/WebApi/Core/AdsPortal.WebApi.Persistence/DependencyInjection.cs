namespace AdsPortal.WebApi.Persistence
{
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Persistence.Configurations;
    using AdsPortal.WebApi.Persistence.DbContext;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext;
    using AdsPortal.WebApi.Persistence.UoW;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration(configuration, out RelationalDbConfiguration relationalDbConfiguration);

            services.AddDbContext<RelationalDbContext>(options => options.UseSqlServer(relationalDbConfiguration.ConnectionString))
                    .AddScoped<IRelationalDbContext>(c => c.GetRequiredService<RelationalDbContext>());

            services.AddScoped<IAppRelationalUnitOfWork, RelationalUnitOfWork>();

            return services;
        }

        public static IHealthChecksBuilder AddPersistenceHealthChecks(this IHealthChecksBuilder healthChecksBuilder)
        {
            healthChecksBuilder.AddDbContextCheck<RelationalDbContext>();

            return healthChecksBuilder;
        }

    }
}
