using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace He.PipelineAssessment.Infrastructure.Config
{
    internal class AssessmentInterventionConfiguration : IEntityTypeConfiguration<AssessmentIntervention>
    {
        public void Configure(EntityTypeBuilder<AssessmentIntervention> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.RequestedBy)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);            
            builder.Property(x => x.RequestedByEmail)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);
            builder.Property(x => x.Administrator)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);
            builder.Property(x => x.AdministratorEmail)
                .HasMaxLength(EntityConfigurationConstants.StandardLength);
            builder.Property(x => x.DecisionType)
                .HasMaxLength(EntityConfigurationConstants.ShortLength);
            builder.Property(x => x.Status)
                .HasMaxLength(EntityConfigurationConstants.ShortLength);

            builder.HasOne(x => x.AssessmentToolWorkflowInstance)
                .WithMany(x => x.AssessmentInterventions)
                .HasForeignKey(x => x.AssessmentToolWorkflowInstanceId);

            builder.HasOne(x => x.TargetAssessmentToolWorkflow)
                .WithMany(x => x.AssessmentInterventions)
                .HasForeignKey(x => x.TargetAssessmentToolWorkflowId);
        }
    }
}