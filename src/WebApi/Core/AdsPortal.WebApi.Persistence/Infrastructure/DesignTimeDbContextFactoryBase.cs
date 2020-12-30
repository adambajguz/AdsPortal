namespace AdsPortal.Persistence.Infrastructure
{
    using System;
    using System.IO;
    using AdsPortal.Persistence.Configurations;
    using AdsPortal.Shared.Extensions.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public TContext CreateDbContext(string[] args)
        {
            string basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}../Presentation/AdsPortal", Path.DirectorySeparatorChar);
            string currentPath = Directory.GetCurrentDirectory();

            return Create(basePath, currentPath, Environment.GetEnvironmentVariable(AspNetCoreEnvironment) ?? "Production");
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        private TContext Create(string basePath, string currentPath, string environmentName)
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().SetBasePath(basePath)
                .AddJsonFile($"appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile($"{currentPath}{Path.DirectorySeparatorChar}appsettings.Persistence.json", optional: false)
                .AddJsonFile($"{currentPath}{Path.DirectorySeparatorChar}appsettings.Persistence.{environmentName}.json", optional: true)
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
