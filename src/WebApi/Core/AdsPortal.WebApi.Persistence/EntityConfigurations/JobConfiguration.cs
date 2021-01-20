namespace AdsPortal.WebApi.Persistence.EntityConfigurations
{
    using AdsPortal.WebApi.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public const string JobNoSequenceName = "Job_JobNo_Sequence";

        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasIndex(x => x.JobNo).IsUnique();

            builder.Property(e => e.JobNo)
                   .HasDefaultValueSql($"NEXT VALUE FOR {JobNoSequenceName}");

            builder.HasIndex(x => x.Priority);

            builder.HasIndex(x => x.Status);

            builder.Ignore(x => x.HasFinished);

            builder.HasOne(x => x.RunAfter)
                   .WithOne()
                   .HasForeignKey<Job>(x => x.RunAfterId)
                   .HasPrincipalKey<Job>(x => x.Id)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
