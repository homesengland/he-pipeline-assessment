using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Data.Configuration
{
    public class AssessmentQuestionConfig : IEntityTypeConfiguration<AssessmentQuestion>
    {
        public const int MaxLength = 450;

        public void Configure(EntityTypeBuilder<AssessmentQuestion> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CorrelationId).HasMaxLength(MaxLength);
            builder.Property(x => x.ActivityName).HasMaxLength(MaxLength);
            builder.Property(x => x.ActivityType).HasMaxLength(MaxLength);
            builder.Property(x => x.ActivityId).HasMaxLength(MaxLength);
            builder.Property(x => x.WorkflowName).HasMaxLength(MaxLength);
            builder.Property(x => x.WorkflowInstanceId).HasMaxLength(MaxLength);
            builder.Property(x => x.WorkflowDefinitionId).HasMaxLength(MaxLength);
            builder.Property(x => x.PreviousActivityId).HasMaxLength(MaxLength);
        }
    }
}
