namespace AdsPortal.Persistence.Configurations
{
    using AdsPortal.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PublicationConfiguration : IEntityTypeConfiguration<Publication>
    {
        public void Configure(EntityTypeBuilder<Publication> builder)
        {
            builder.HasOne(publication => publication.Journal)
                   .WithMany(journal => journal.Publications)
                   .HasForeignKey(publication => publication.JournalId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
