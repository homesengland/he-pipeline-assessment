using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace He.PipelineAssessment.Infrastructure.Config
{
    internal class AssessmentToolWorkflowConfiguration : IEntityTypeConfiguration<AssessmentToolWorkflow>
    {
        public void Configure(EntityTypeBuilder<AssessmentToolWorkflow> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.WorkflowDefinitionId)
                .HasMaxLength(450);
            builder.Property(x => x.Name)
                .HasMaxLength(100);

            builder.HasOne(x => x.AssessmentTool)
                .WithMany(x => x.AssessmentToolWorkflows)
                .HasForeignKey(x => x.AssessmentToolId);

            builder.Property(x => x.CreatedBy)
               .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);
        }
    }
}