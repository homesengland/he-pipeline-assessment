using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Config
{
    public class QuestionWorkflowInstanceConfig : IEntityTypeConfiguration<QuestionWorkflowInstance>
    {
        public void Configure(EntityTypeBuilder<QuestionWorkflowInstance> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0);
            builder.Property(x => x.WorkflowInstanceId).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.Result).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.Result).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.WorkflowName).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.CorrelationId).HasMaxLength(EntityConfigurationConstants.QuestionTypeMaxLength);
        }
    }
}
