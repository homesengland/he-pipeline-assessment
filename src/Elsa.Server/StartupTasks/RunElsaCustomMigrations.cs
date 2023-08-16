using Elsa.Services;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Server.StartupTasks
{
    public class RunElsaCustomMigrations : IStartupTask
    {
        private readonly DbContext _dbContext;

        public RunElsaCustomMigrations(DbContext dbContext)
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
