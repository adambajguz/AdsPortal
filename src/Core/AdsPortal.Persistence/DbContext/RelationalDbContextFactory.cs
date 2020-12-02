namespace AdsPortal.Persistence.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using Persistence.Infrastructure;

    public class RelationalDbContextFactory : DesignTimeDbContextFactoryBase<RelationalDbContext>
    {
        protected override RelationalDbContext CreateNewInstance(DbContextOptions<RelationalDbContext> options)
        {
            return new RelationalDbContext(options);
        }
    }
}
