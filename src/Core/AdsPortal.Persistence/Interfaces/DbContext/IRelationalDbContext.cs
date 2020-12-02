namespace AdsPortal.Persistence.Interfaces.DbContext
{
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IRelationalDbContext : IGenericRelationalDbContext
    {
        DbSet<EntityAuditLog> EntityAuditLogs { get; }

        DbSet<Author> Authors { get; }
        DbSet<Degree> Degrees { get; }
        DbSet<Department> Departments { get; }
        DbSet<Journal> Journals { get; }
        DbSet<Publication> Publications { get; }
        DbSet<PublicationAuthor> PublicationAuthors { get; }

        DbSet<MediaItem> MediaItems { get; set; }

        DbSet<User> Users { get; }

        DbSet<Job> Jobs { get; }
    }
}
