using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Config
{
    public class CustomActivityNavigationConfig : IEntityTypeConfiguration<CustomActivityNavigation>
    {


        public void Configure(EntityTypeBuilder<CustomActivityNavigation> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnOrder(0);
            builder.Property(x => x.ActivityType).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.ActivityId).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.WorkflowInstanceId).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.PreviousActivityId).HasMaxLength(EntityConfigurationConstants.MaxLength);
            builder.Property(x => x.PreviousActivityType).HasMaxLength(EntityConfigurationConstants.MaxLength);

        }
    }
}
