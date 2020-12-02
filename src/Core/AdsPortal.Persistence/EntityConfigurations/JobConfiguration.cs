namespace AdsPortal.Persistence.Configurations
{
    using AdsPortal.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public const string JobNoSequenceName = "Job_JobNo_Sequence";

        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.Property(e => e.JobNo)
                   .HasDefaultValueSql($"NEXT VALUE FOR {JobNoSequenceName}");
        }
    }
}
