using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace He.PipelineAssessment.Infrastructure.Config
{
    internal class AssessmentToolWorkflowInstanceConfiguration : IEntityTypeConfiguration<AssessmentToolWorkflowInstance>
    {
        public void Configure(EntityTypeBuilder<AssessmentToolWorkflowInstance> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.WorkflowDefinitionId)
                .HasMaxLength(50);

            builder.Property(x => x.WorkflowInstanceId)
                .HasMaxLength(50);

            builder.Property(x => x.WorkflowName)
                .HasMaxLength(100);

            builder.Property(x => x.CurrentActivityId)
                .HasMaxLength(50);

            builder.Property(x => x.CurrentActivityType)
                .HasMaxLength(50);

            builder.Property(x => x.Status)
                .HasMaxLength(50);

            builder.HasOne(x => x.Assessment)
                .WithMany(x => x.AssessmentToolWorkflowInstances)
                .HasForeignKey(x => x.AssessmentId);
        }
    }
}
