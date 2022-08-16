using Elsa.Services;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Server.Data.StartupTasks
{
    public class RunPipelineAssessmentMigrations : IStartupTask
    {
        private readonly IDbContextFactory<PipelineAssessmentContext> _dbContextFactory;

        public RunPipelineAssessmentMigrations(IDbContextFactory<PipelineAssessmentContext> dbContextFactoryFactory)
        {
            _dbContextFactory = dbContextFactoryFactory;
        }

        public int Order => 0;

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();
            await dbContext.Database.MigrateAsync(cancellationToken);
            await dbContext.DisposeAsync();
        }
    }
}
