using He.PipelineAssessment.Infrastructure.Config;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Assessments;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace He.PipelineAssessment.Infrastructure.Data
{
    public class PipelineAssessmentContext : DbContext, IDataProtectionKeyContext
    {
        public PipelineAssessmentContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;
        public DbSet<Assessment> Assessment { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssessmentConfiguration).Assembly);
        }
    }
}