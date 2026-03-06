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

            // Configure composite primary key
            builder.HasKey(x => new { x.SpId, x.VariableId });

            builder.Property(p => p.SpId).HasColumnOrder(0);
            builder.Property(p => p.VariableId).HasColumnOrder(1);
            builder.Property(p => p.Value).IsRequired();
            builder.Property(p => p.LastUpdatedBy);

            builder.HasOne(x => x.Variable)
                .WithMany(x => x.VariableInstances)
                .HasForeignKey(x => x.VariableId);
        }
    }
}