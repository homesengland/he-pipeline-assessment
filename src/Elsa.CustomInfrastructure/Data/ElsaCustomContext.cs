namespace Elsa.CustomInfrastructure.Data
{
    public class ElsaCustomContext : DbContext
    {
        public ElsaCustomContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; } = default!;
    }
}
