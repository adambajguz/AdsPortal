﻿namespace AdsPortal.Persistence
{
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Persistence.Configurations;
    using AdsPortal.Persistence.DbContext;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.UoW;
    using AdsPortal.Shared.Extensions.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<RelationalDbConfiguration>(configuration, out RelationalDbConfiguration relationalDbConfiguration);

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