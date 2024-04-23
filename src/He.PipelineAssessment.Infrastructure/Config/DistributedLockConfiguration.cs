using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace He.PipelineAssessment.Infrastructure.Config
{
    public class DistributedLockConfiguration : IEntityTypeConfiguration<DistributedLock>
    {
        public void Configure(EntityTypeBuilder<DistributedLock> builder)
        {
            builder.ToTable(x => x.IsTemporal());

            builder.HasKey(x => x.LockId);
        }
    }
}
