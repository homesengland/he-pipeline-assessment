using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Config
{
    internal class QuestionChoiceGroupConfig : IEntityTypeConfiguration<QuestionChoiceGroup>
    {
        public void Configure(EntityTypeBuilder<QuestionChoiceGroup> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0);
            builder.Property(x => x.GroupIdentifier).HasMaxLength(EntityConfigurationConstants.QuestionTypeMaxLength);
            builder.HasMany(x => x.QuestionGroupChoices).WithOne(x => x.QuestionChoiceGroup)
                .HasForeignKey(x => x.QuestionChoiceGroupId);
        }
    }
}
