namespace AdsPortal.Persistence.DbContext
{
    using AdsPortal.Domain.Entities;
    using AdsPortal.Persistence.Configurations;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using Microsoft.EntityFrameworkCore;

    public class RelationalDbContext : DbContext, IRelationalDbContext
    {
        public DbContext Provider => this;

        public RelationalDbContext(DbContextOptions<RelationalDbContext> options) : base(options)
        {

        }

        public virtual DbSet<EntityAuditLog> EntityAuditLogs { get; set; } = default!;

        public virtual DbSet<Author> Authors { get; set; } = default!;
        public virtual DbSet<Degree> Degrees { get; set; } = default!;
        public virtual DbSet<Department> Departments { get; set; } = default!;
        public virtual DbSet<Journal> Journals { get; set; } = default!;
        public virtual DbSet<Publication> Publications { get; set; } = default!;
        public virtual DbSet<PublicationAuthor> PublicationAuthors { get; set; } = default!;

        public virtual DbSet<MediaItem> MediaItems { get; set; } = default!;

        public virtual DbSet<User> Users { get; set; } = default!;

        public virtual DbSet<Job> Jobs { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<long>(JobConfiguration.JobNoSequenceName);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RelationalDbContext).Assembly);
        }
    }
}
