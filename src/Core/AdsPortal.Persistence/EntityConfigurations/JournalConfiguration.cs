namespace AdsPortal.Persistence.Configurations
{
    using AdsPortal.Domain.Entities;
    using AdsPortal.Persistence.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class JournalConfiguration : IEntityTypeConfiguration<Journal>
    {
        public void Configure(EntityTypeBuilder<Journal> builder)
        {
            builder.Property(x => x.Disciplines)
                   .HasJsonConversion();
        }
    }
}
