using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Elsa.CustomInfrastructure.Config
{
    public class QuestionDataDictionaryGroupConfig : IEntityTypeConfiguration<QuestionDataDictionaryGroup>
    {
        public void Configure(EntityTypeBuilder<QuestionDataDictionaryGroup> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0);
            builder.Property(p => p.Name).HasMaxLength(EntityConfigurationConstants.MaxLength);
        }
    }
}
