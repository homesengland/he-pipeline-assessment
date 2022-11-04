﻿using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Config
{
    public class AssessmentQuestionConfig : IEntityTypeConfiguration<AssessmentQuestion>
    {
        public const int MaxLength = 450;

        public void Configure(EntityTypeBuilder<AssessmentQuestion> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0);
            builder.Property(x => x.ActivityType).HasMaxLength(MaxLength);
            builder.Property(x => x.ActivityId).HasMaxLength(MaxLength);
            builder.Property(x => x.WorkflowInstanceId).HasMaxLength(MaxLength);
            builder.Property(x => x.PreviousActivityId).HasMaxLength(MaxLength);
        }
    }
}
