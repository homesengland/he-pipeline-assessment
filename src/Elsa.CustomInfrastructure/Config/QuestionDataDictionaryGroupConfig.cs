using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomInfrastructure.Config
{
    public class QuestionDataDictionaryGroupConfig : IEntityTypeConfiguration<QuestionDataDictionaryGroup>
    {
        public void Configure(EntityTypeBuilder<QuestionDataDictionaryGroup> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Name).HasMaxLength(EntityConfigurationConstants.MaxLength);
        }
    }
}
