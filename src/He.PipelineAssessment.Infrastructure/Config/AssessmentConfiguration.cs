using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace He.PipelineAssessment.Infrastructure.Config
{
    public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
    {
        public void Configure(EntityTypeBuilder<Assessment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Counterparty)
                .HasMaxLength(500);

            builder.Property(x => x.Reference)
                .HasMaxLength(100);

            builder.Property(x => x.SiteName)
                .HasMaxLength(500);

            builder.Property(x => x.Status)
                .HasMaxLength(50);

        }
    }
}
