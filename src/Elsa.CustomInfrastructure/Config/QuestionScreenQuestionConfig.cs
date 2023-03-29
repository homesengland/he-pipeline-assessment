using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Config
{
    public class QuestionScreenQuestionConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0);
            builder.Property(x => x.ActivityId).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.WorkflowInstanceId).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.QuestionId).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.QuestionType).HasMaxLength(EntityConfigurationConstants.QuestionTypeMaxLength);

        }
    }
}
