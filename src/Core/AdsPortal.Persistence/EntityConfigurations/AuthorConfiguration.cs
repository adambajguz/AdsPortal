namespace AdsPortal.Persistence.Configurations
{
    using AdsPortal.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasOne(author => author.Department)
                   .WithMany(department => department.Authors)
                   .HasForeignKey(author => author.DepartmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(author => author.Degree)
                   .WithMany(degree => degree.Authors)
                   .HasForeignKey(author => author.DegreeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
