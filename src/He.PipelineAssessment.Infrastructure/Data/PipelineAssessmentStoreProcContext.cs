using He.PipelineAssessment.Infrastructure.Config;
using He.PipelineAssessment.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Data
{
    public class PipelineAssessmentStoreProcContext : DbContext
    {
        public PipelineAssessmentStoreProcContext(DbContextOptions<PipelineAssessmentStoreProcContext> options) : base(options)
        {

        }
        public DbSet<AssessmentStageViewModel> AssessmentStageViewModel { get; set; } = default!;
        public DbSet<StartableToolViewModel> StartableToolViewModel { get; set; } = default!;
        public DbSet<AssessmentDataViewModel> AssessmentDataViewModel { get; set; } = default!;
        public DbSet<AssessmentInterventionViewModel> AssessmentInterventionViewModel { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AssessmentStageViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<StartableToolViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<AssessmentDataViewModel>().HasNoKey().ToView(null);
            modelBuilder.Entity<AssessmentInterventionViewModel>().HasNoKey().ToView(null);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssessmentConfiguration).Assembly);
        }
    }
}