using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public interface IDistributedLockRepository
    {
        Task<bool> TryAcquireLockAsync(string lockId);

        Task ReleaseLockAsync(string lockId);
    }
    public class DistributedLockRepository : IDistributedLockRepository
    {
        private readonly PipelineAssessmentContext context;

        public DistributedLockRepository(PipelineAssessmentContext context)
        {
            this.context = context;
        }

        public async Task ReleaseLockAsync(string lockId)
        {
            var _lock = new DistributedLock { LockId = lockId };
            context.DistributedLock.Attach(_lock) ;
            context.DistributedLock.Remove(_lock) ;
            await context.SaveChangesAsync();
        }

        public async Task<bool> TryAcquireLockAsync(string lockId)
        {
            try
            {
                context.DistributedLock.Add(new Models.DistributedLock { LockId = lockId });
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
