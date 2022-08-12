using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Elsa.Server.Data
{
    public class SqlitePipelineAssessmentContextFactory : IDesignTimeDbContextFactory<PipelineAssessmentContext>
    {
        public PipelineAssessmentContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PipelineAssessmentContext>();
            var connectionString = args.Any() ? args[0] : "Data Source=pipelineAssessment.db;Cache=Shared";

            builder.UseSqlite(connectionString, db => db
                .MigrationsAssembly(typeof(SqlitePipelineAssessmentContextFactory).Assembly.GetName().Name));

            return new PipelineAssessmentContext(builder.Options);
        }
    }
}
