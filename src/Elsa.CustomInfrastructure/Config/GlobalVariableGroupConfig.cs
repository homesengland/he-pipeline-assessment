using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Config
{
    public class GlobalVariableGroupConfig : IEntityTypeConfiguration<GlobalVariableGroup>
    {
        public void Configure(EntityTypeBuilder<GlobalVariableGroup> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0).UseIdentityColumn();
            builder.Property(p => p.Name).HasMaxLength(EntityConfigurationConstants.MaxLength).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(p => p.Type).HasMaxLength(EntityConfigurationConstants.DataDictionaryTypeMaxLength).IsRequired();
            builder.HasMany(x => x.GlobalVariableList).WithOne(x => x.Group).HasForeignKey(x => x.GlobalVariableGroupId);
        }
    }
}