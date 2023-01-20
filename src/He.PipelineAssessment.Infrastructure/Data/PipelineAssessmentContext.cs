using He.PipelineAssessment.Infrastructure.Config;
using He.PipelineAssessment.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Data
{
    public class PipelineAssessmentContext : DbContext, IDataProtectionKeyContext
    {
        public PipelineAssessmentContext(DbContextOptions<PipelineAssessmentContext> options) : base(options)
        {

        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;
        public DbSet<Assessment> Assessment { get; set; } = default!;
        public DbSet<AssessmentTool> AssessmentTool { get; set; } = default!;
        public DbSet<AssessmentToolInstanceNextWorkflow> AssessmentToolInstanceNextWorkflow { get; set; } = default!;
        public DbSet<AssessmentToolWorkflow> AssessmentToolWorkflow { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssessmentConfiguration).Assembly);
        }
    }
}