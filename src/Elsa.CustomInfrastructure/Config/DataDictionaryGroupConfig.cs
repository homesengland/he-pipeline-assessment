using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Elsa.CustomInfrastructure.Config
{
    public class DataDictionaryGroupConfig : IEntityTypeConfiguration<DataDictionaryGroup>
    {
        public void Configure(EntityTypeBuilder<DataDictionaryGroup> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0).UseIdentityColumn();
            builder.Property(p => p.Name).HasMaxLength(EntityConfigurationConstants.MaxLength);
        }
    }
}
