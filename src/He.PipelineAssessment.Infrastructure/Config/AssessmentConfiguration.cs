using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Assessments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace He.PipelineAssessment.Infrastructure.Config
{
    public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
    {
        public void Configure(EntityTypeBuilder<Assessment> builder)
        {
            builder.ToTable(x => x.IsTemporal());

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SpId);

            builder.Property(x => x.Counterparty)
                .HasMaxLength(500);

            builder.Property(x => x.Reference)
                .HasMaxLength(100);

            builder.Property(x => x.SiteName)
                .HasMaxLength(500);

            builder.Property(x => x.ProjectManager)
                .HasMaxLength(100);

            builder.Property(x => x.ProjectManagerEmail)
                .HasMaxLength(100);

            builder.Property(x => x.Status)
                .HasMaxLength(50);

            builder.Property(x => x.LocalAuthority)
                .HasMaxLength(500);
        }
    }
}
