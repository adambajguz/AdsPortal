namespace AdsPortal.Persistence.Interfaces.DbContext
{
    using AdsPortal.Persistence.Interfaces.DbContext.Generic;
    using AdsPortal.WebApi.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IRelationalDbContext : IGenericRelationalDbContext
    {
        //Data
        DbSet<Advertisement> Advertisements { get; }
        DbSet<Category> Categories { get; }

        //Media
        DbSet<MediaItem> MediaItems { get; set; }

        //Identity
        DbSet<User> Users { get; }

        //Jobs
        DbSet<Job> Jobs { get; }

        //Audit
        DbSet<EntityAuditLog> EntityAuditLogs { get; }
    }
}
