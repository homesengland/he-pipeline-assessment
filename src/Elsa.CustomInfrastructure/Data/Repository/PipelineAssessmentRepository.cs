using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public class PipelineAssessmentRepository : IPipelineAssessmentRepository
    {
        private readonly DbContext _dbContext;
        public PipelineAssessmentRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AssessmentQuestion?> GetMultipleChoiceQuestions(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<AssessmentQuestion>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<AssessmentQuestion?> GetMultipleChoiceQuestions(string activityId, string workflowInstanceId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<AssessmentQuestion>().FirstOrDefaultAsync(x => x.ActivityId == activityId && x.WorkflowInstanceId == workflowInstanceId, cancellationToken);
        }

        public async ValueTask<string?> CreateMultipleChoiceQuestionAsync(AssessmentQuestion model, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model.Id;
        }

        public async Task<AssessmentQuestion?> UpdateMultipleChoiceQuestion(AssessmentQuestion model, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }
    }
}
