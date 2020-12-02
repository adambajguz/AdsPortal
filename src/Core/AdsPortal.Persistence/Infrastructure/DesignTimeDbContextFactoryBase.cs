namespace AdsPortal.Persistence.Infrastructure
{
    using System;
    using System.IO;
    using AdsPortal.Common;
    using AdsPortal.Persistence.DbContext.Settings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public TContext CreateDbContext(string[] args)
        {
            string basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}../Presentation/AdsPortal", Path.DirectorySeparatorChar);

            return Create(basePath, Environment.GetEnvironmentVariable(AspNetCoreEnvironment));
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        private TContext Create(string basePath, string? environmentName)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(basePath);
            configurationBuilder.AddJsonFile(GlobalAppConfig.AppSettingsFileName);

            IConfigurationRoot configurationRoot = configurationBuilder.AddJsonFile($"appsettings.Local.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            //TODO refactor
            string connectionString = configurationRoot.GetConnectionString(ConnectionStringsNames.SQLDatabase);

            return Create(connectionString);
        }

        private TContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"Connection string '{ConnectionStringsNames.SQLDatabase}' is null or empty.", nameof(connectionString));

            Console.WriteLine($"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'.");

            DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return CreateNewInstance(optionsBuilder.Options);
        }
    }
}
