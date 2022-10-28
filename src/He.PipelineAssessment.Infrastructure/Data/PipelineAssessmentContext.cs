using He.PipelineAssessment.Infrastructure.Models;
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
        public DbSet<AssessmentStageVersion> AssessmentStageVersions { get; set; } = default!;

    }
}