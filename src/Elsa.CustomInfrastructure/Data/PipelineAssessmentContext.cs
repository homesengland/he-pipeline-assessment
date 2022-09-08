using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Elsa.CustomInfrastructure.Data
{
    public class PipelineAssessmentContext : DbContext
    {
        public PipelineAssessmentContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; } = default!;
    }
}
