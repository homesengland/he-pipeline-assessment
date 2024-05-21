using He.PipelineAssessment.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(x => x.IsTemporal());
            builder.HasKey(x => x.CategoryId);
            builder
          .HasIndex(x => x.CategoryName)
          .IsUnique();
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
        }
    }
}
