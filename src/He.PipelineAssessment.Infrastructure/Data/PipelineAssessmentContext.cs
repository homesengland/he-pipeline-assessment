using He.PipelineAssessment.Infrastructure.Config;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Models.ViewModels;
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
        public DbSet<Assessment> Assessment { get; set; } = default!;
        public DbSet<AssessmentTool> AssessmentTool { get; set; } = default!;
        public DbSet<AssessmentToolInstanceNextWorkflow> AssessmentToolInstanceNextWorkflow { get; set; } = default!;
        public DbSet<AssessmentToolWorkflow> AssessmentToolWorkflow { get; set; } = default!;
        public DbSet<AssessmentStageViewModel> AssessmentStageViewModel { get; set; } = default!;
        public DbSet<StartableToolViewModel> StartableToolViewModel { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AssessmentStageViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<StartableToolViewModel>().HasNoKey().ToView(null);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssessmentConfiguration).Assembly);
        }
    }
}