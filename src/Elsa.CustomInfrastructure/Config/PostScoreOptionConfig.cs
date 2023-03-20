using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Elsa.CustomInfrastructure.Config;
public class PostScoreOptionConfig : IEntityTypeConfiguration<PotScoreOption>
{
    public void Configure(EntityTypeBuilder<PotScoreOption> builder)
    {
        builder.ToTable(x => x.IsTemporal());
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id).HasColumnOrder(0);
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}
