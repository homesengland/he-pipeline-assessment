using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Config
{

    public class QuestionScreenChoiceConfig : IEntityTypeConfiguration<QuestionChoice>
    {
        public void Configure(EntityTypeBuilder<QuestionChoice> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0);
            builder.Property(x => x.QuestionId).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.HasOne(x => x.Question).WithMany(x => x.Choices).HasForeignKey(x => x.QuestionId);
        }
    }
}
