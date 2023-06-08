using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace He.PipelineAssessment.Infrastructure.Config
{
    internal class AssessmentToolWorkflowInstanceConfiguration : IEntityTypeConfiguration<AssessmentToolWorkflowInstance>
    {
        public void Configure(EntityTypeBuilder<AssessmentToolWorkflowInstance> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);

            builder.Property(x => x.WorkflowDefinitionId)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.WorkflowInstanceId)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.WorkflowName)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);

            builder.Property(x => x.CurrentActivityId)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.CurrentActivityType)
                .HasMaxLength(EntityConfigurationConstants.ShortLength);

            builder.Property(x => x.Status)
                .HasMaxLength(EntityConfigurationConstants.ShortLength);

            builder.HasOne(x => x.Assessment)
                .WithMany(x => x.AssessmentToolWorkflowInstances)
                .HasForeignKey(x => x.AssessmentId);

            builder.Property(x => x.Result)
               .HasMaxLength(EntityConfigurationConstants.StandardLength);

            builder.Property(x => x.SubmittedBy)
               .HasMaxLength(EntityConfigurationConstants.StandardLength);

            builder.Property(x => x.CreatedBy)
               .HasMaxLength(EntityConfigurationConstants.MaxLength);

            builder.Property(x => x.LastModifiedBy)
                .HasMaxLength(EntityConfigurationConstants.MaxLength);
        }
    }
}
