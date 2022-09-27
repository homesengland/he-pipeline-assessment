using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Elsa.CustomInfrastructure.Data
{
    public class ElsaCustomContext : DbContext
    {
        public ElsaCustomContext()
        {
        }

        public ElsaCustomContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; } = default!;
    }
}
