namespace AdsPortal.WebApi.Persistence.EntityConfigurations
{
    using AdsPortal.WebApi.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
    {
        public void Configure(EntityTypeBuilder<Advertisement> builder)
        {
            builder.HasOne(ad => ad.CoverImage)
                   .WithMany()
                   .HasForeignKey(author => author.CoverImageId)
                   .HasPrincipalKey(category => category.Id)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ad => ad.Category)
                   .WithMany(category => category.Advertisements)
                   .HasForeignKey(author => author.CategoryId)
                   .HasPrincipalKey(category => category.Id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(author => author.Author)
                   .WithMany(category => category.Advertisements)
                   .HasForeignKey(author => author.AuthorId)
                   .HasPrincipalKey(category => category.Id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
