namespace AdsPortal.Persistence.Configurations
{
    using AdsPortal.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EntityAuditLogConfiguration : IEntityTypeConfiguration<EntityAuditLog>
    {
        public void Configure(EntityTypeBuilder<EntityAuditLog> builder)
        {
            builder.Property(x => x.Action)
                   .HasConversion<int>();

            builder.HasIndex(x => x.Key);
        }
    }
}
