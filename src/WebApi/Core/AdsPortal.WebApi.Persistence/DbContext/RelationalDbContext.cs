namespace AdsPortal.Persistence.DbContext
{
    using AdsPortal.Persistence.Configurations;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.WebApi.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class RelationalDbContext : DbContext, IRelationalDbContext
    {
        public DbContext Provider => this;

        public RelationalDbContext(DbContextOptions<RelationalDbContext> options) : base(options)
        {

        }

        //Data
        public virtual DbSet<Advertisement> Advertisements { get; set; } = default!;
        public virtual DbSet<Category> Categories { get; set; } = default!;

        //Media
        public virtual DbSet<MediaItem> MediaItems { get; set; } = default!;

        //Identity
        public virtual DbSet<User> Users { get; set; } = default!;

        //Jobs
        public virtual DbSet<Job> Jobs { get; set; } = default!;

        //Audit
        public virtual DbSet<EntityAuditLog> EntityAuditLogs { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<long>(JobConfiguration.JobNoSequenceName);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RelationalDbContext).Assembly);
        }
    }
}
