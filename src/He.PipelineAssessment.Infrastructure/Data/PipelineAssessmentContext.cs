using He.PipelineAssessment.Infrastructure.Config;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Data
{
    public class PipelineAssessmentContext : DbContext, IDataProtectionKeyContext
    {
        public PipelineAssessmentContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MockAssessmentConfiguration).Assembly);
        }
    }
}