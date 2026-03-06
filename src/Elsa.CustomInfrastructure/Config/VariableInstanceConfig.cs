using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Config
{
    public class VariableInstanceConfig : IEntityTypeConfiguration<VariableInstance>
    {
        public void Configure(EntityTypeBuilder<VariableInstance> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0).UseIdentityColumn();
            builder.Property(p => p.Value).IsRequired();
            builder.Property(p => p.LastUpdatedBy);
            builder.HasOne(x => x.Variable).WithMany(x => x.VariableInstances).HasForeignKey(x => x.VariableId);
        }
    }
}