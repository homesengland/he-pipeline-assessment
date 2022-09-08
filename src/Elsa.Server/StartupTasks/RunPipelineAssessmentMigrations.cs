using Elsa.Services;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Server.StartupTasks
{
    public class RunPipelineAssessmentMigrations : IStartupTask
    {
        private readonly DbContext _dbContext;

        public RunPipelineAssessmentMigrations(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Order => 0;

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.Database.MigrateAsync(cancellationToken);
            await _dbContext.DisposeAsync();
        }
    }
}
