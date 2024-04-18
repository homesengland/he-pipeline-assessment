using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace He.PipelineAssessment.Infrastructure.Config
{
    public class TargetAssessmentToolWorkflowConfiguration : IEntityTypeConfiguration<TargetAssessmentToolWorkflow>
    {
        public void Configure(EntityTypeBuilder<TargetAssessmentToolWorkflow> builder)
        {
            builder.HasOne(x => x.AssessmentIntervention)
                .WithMany(x => x.TargetAssessmentToolWorkflows)
                .HasForeignKey(x => x.AssessmentInterventionId);

            builder.HasOne(x => x.AssessmentToolWorkflow)
                .WithMany(x => x.TargetAssessmentToolWorkflows)
                .HasForeignKey(x => x.AssessmentToolWorkflowId);
        }
    }
}
