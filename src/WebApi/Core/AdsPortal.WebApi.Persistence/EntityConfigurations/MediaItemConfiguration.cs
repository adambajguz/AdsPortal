namespace AdsPortal.Persistence.Configurations
{
    using AdsPortal.WebApi.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MediaItemConfiguration : IEntityTypeConfiguration<MediaItem>
    {
        public void Configure(EntityTypeBuilder<MediaItem> builder)
        {
            builder.HasIndex(x => x.PathHashCode);
        }
    }
}
