namespace AdsPortal.Persistence.Configurations
{
    using AdsPortal.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MediaItemConfiguration : IEntityTypeConfiguration<MediaItem>
    {
        public void Configure(EntityTypeBuilder<MediaItem> builder)
        {
            builder.Property(x => x.Role)
                   .HasConversion<uint>();

            builder.HasIndex(x => x.PathHashCode);
        }
    }
}
