namespace AdsPortal.Persistence.Configurations
{
    using AdsPortal.WebApi.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EntityAuditLogConfiguration : IEntityTypeConfiguration<EntityAuditLog>
    {
        public void Configure(EntityTypeBuilder<EntityAuditLog> builder)
        {
            builder.HasIndex(x => x.Key);
        }
    }
}
