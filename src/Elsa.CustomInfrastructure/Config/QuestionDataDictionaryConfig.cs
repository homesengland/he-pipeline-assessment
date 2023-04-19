using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomInfrastructure.Config
{
    public class QuestionDataDictionaryConfig : IEntityTypeConfiguration<QuestionDataDictionary>
    {
        public void Configure(EntityTypeBuilder<QuestionDataDictionary> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0);
            builder.Property(p => p.Name).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(p => p.LegacyName).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(p => p.Description).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(p => p.Type).HasMaxLength(EntityConfigurationConstants.DataDictionaryTypeMaxLength);
            builder.HasOne(x => x.Group).WithMany().HasForeignKey(x => x.QuestionDataDictionaryGroupId);
        }
    }
}
