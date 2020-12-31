namespace AdsPortal.WebApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    public class DbMigrationsService
    {
        public async ValueTask Migrate(IWebHost webHost)
        {
            await MigrateDatabase<IRelationalDbContext>(webHost);
        }

        private async Task MigrateDatabase<TDbContext>(IWebHost webHost) where TDbContext : IGenericRelationalDbContext
        {
            Console.WriteLine($"Applying Entity Framework migrations for {typeof(TDbContext).Name}");

            IServiceScopeFactory serviceScopeFactory = webHost.Services.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

                await dbContext.Provider.Database.MigrateAsync();
                Console.WriteLine("All done, closing app");
            }
        }

        public async ValueTask<bool> Verify(IWebHost webHost)
        {
            return await VerifyMigrations<IRelationalDbContext>(webHost);
        }

        public async Task<bool> VerifyMigrations<TDbContext>(IWebHost webHost) where TDbContext : IGenericRelationalDbContext
        {
            Console.WriteLine($"Validating status of Entity Framework migrations for {typeof(TDbContext).Name}");

            IServiceScopeFactory serviceScopeFactory = webHost.Services.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
                DatabaseFacade database = dbContext.Provider.Database;

                IEnumerable<string> pendingMigrations = await database.GetPendingMigrationsAsync();
                IList<string> migrations = pendingMigrations as IList<string> ?? pendingMigrations.ToList();
                if (!migrations.Any())
                {
                    Console.WriteLine("No pending migratons");
                    return true;
                }

                Console.WriteLine("Pending migratons {0}", migrations.Count);
                foreach (string migration in migrations)
                    Console.WriteLine($"\t{migration}");

                return false;
            }
        }
    }
}
