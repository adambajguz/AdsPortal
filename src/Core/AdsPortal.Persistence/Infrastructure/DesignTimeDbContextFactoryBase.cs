namespace AdsPortal.Persistence.Infrastructure
{
    using System;
    using System.IO;
    using AdsPortal.Common;
    using AdsPortal.Common.Extensions;
    using AdsPortal.Persistence.Configurations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public TContext CreateDbContext(string[] args)
        {
            string basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}../Presentation/AdsPortal", Path.DirectorySeparatorChar);

            return Create(basePath, Environment.GetEnvironmentVariable(AspNetCoreEnvironment) ?? "Production");
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        private TContext Create(string basePath, string? environmentName)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(basePath);
            configurationBuilder.AddJsonFile(GlobalAppConfig.AppSettingsFileName);

            IConfigurationRoot configurationRoot = configurationBuilder
                .AddJsonFile($"appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            RelationalDbConfiguration relationalDbConfiguration = configurationRoot.GetValue<RelationalDbConfiguration>();

            return Create(relationalDbConfiguration.ConnectionString);
        }

        private TContext Create(string? connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"Connection string is null or empty.", nameof(connectionString));

            DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return CreateNewInstance(optionsBuilder.Options);
        }
    }
}
