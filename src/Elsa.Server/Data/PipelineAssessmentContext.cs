using Elsa.Models;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Server.Data
{
    public class PipelineAssessmentContext : DbContext
    {
        public PipelineAssessmentContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MultipleChoiceQuestionModel?> MultipleChoiceQuestions { get; set; } = default!;
    }
}
