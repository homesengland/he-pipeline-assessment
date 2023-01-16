﻿using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace He.PipelineAssessment.Infrastructure.Config
{
    public class AssessmentToolInstanceNextWorkflowConfiguration : IEntityTypeConfiguration<AssessmentToolInstanceNextWorkflow>
    {
        public void Configure(EntityTypeBuilder<AssessmentToolInstanceNextWorkflow> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.NextWorkflowDefinitionId)
                .HasMaxLength(450);

            builder.HasOne(x => x.AssessmentToolWorkflowInstance)
                .WithMany(x => x.AssessmentToolInstanceNextWorkflows)
                .HasForeignKey(x => x.AssessmentToolWorkflowInstanceId);
        }
    }
}
