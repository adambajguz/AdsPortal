namespace AdsPortal.Persistence.Configurations
{
    using AdsPortal.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PublicationAuthorConfiguration : IEntityTypeConfiguration<PublicationAuthor>
    {
        public void Configure(EntityTypeBuilder<PublicationAuthor> builder)
        {
            builder.HasOne(publicationAuthor => publicationAuthor.Publication)
                   .WithMany(publication => publication.PublicationAuthors)
                   .HasForeignKey(publicationAuthor => publicationAuthor.PublicationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(publicationAuthor => publicationAuthor.Author)
                   .WithMany(author => author.PublicationAuthors)
                   .HasForeignKey(publicationAuthor => publicationAuthor.AuthorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
