using He.PipelineAssessment.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public class PipelineAssessmentRepository : IPipelineAssessmentRepository
    {
        private readonly DbContext _dbContext;

        public PipelineAssessmentRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AssessmentStageVersion?> GetLatestAssessmentStageVersion(string workflowDefinitionId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<AssessmentStageVersion>().Where(x => x.WorkflowDefinitionId == workflowDefinitionId).OrderByDescending(x => x.Version)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<AssessmentStageVersion?> GetSpecificAssessmentStageVersion(string workflowDefinitionId, int version, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<AssessmentStageVersion>()
                .FirstOrDefaultAsync(x => x.WorkflowDefinitionId == workflowDefinitionId && x.Version == version, cancellationToken: cancellationToken);
        }
    }
}
