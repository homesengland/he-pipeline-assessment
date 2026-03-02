using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Config
{
    public class GlobalVariableConfig : IEntityTypeConfiguration<GlobalVariable>
    {
        public void Configure(EntityTypeBuilder<GlobalVariable> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0).UseIdentityColumn();
            builder.Property(p => p.Name).HasMaxLength(EntityConfigurationConstants.MaxLength).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(p => p.Value).HasMaxLength(EntityConfigurationConstants.DataDictionaryTypeMaxLength).IsRequired();
            builder.HasOne(x => x.Group).WithMany(x => x.GlobalVariableList).HasForeignKey(x => x.GlobalVariableGroupId);
        }
    }
}