using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public class ElsaCustomRepository : IElsaCustomRepository
    {
        private readonly DbContext _dbContext;
        public ElsaCustomRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AssessmentQuestion?> GetAssessmentQuestion(string activityId, string workflowInstanceId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<AssessmentQuestion>().FirstOrDefaultAsync(x => x.ActivityId == activityId && x.WorkflowInstanceId == workflowInstanceId, cancellationToken);
        }

        public async ValueTask<int?> CreateAssessmentQuestionAsync(AssessmentQuestion model, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model.Id;
        }

        public async Task<AssessmentQuestion?> UpdateAssessmentQuestion(AssessmentQuestion model, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task<IEnumerable<AssessmentQuestion>?> GetAssessmentQuestions(string workflowDefinitionId,
            string correlationId)
        {
            var latestInstance = await _dbContext.Set<AssessmentQuestion>().Where(x =>
                    x.CorrelationId == correlationId && x.WorkflowDefinitionId == workflowDefinitionId)
                .OrderByDescending(x => x.CreatedDateTime).Select(x => x.WorkflowInstanceId).FirstOrDefaultAsync();

            var assessmentQuestions =
                _dbContext.Set<AssessmentQuestion>().Where(x => x.WorkflowInstanceId == latestInstance);
            return assessmentQuestions;
        }

        public async Task<AssessmentQuestion?> GetAssessmentQuestion(string workflowDefinitionId, string correlationId, string activityName, CancellationToken cancellationToken = default)
        {
            var latestInstance = await _dbContext.Set<AssessmentQuestion>().Where(x =>
                    x.CorrelationId == correlationId && x.WorkflowDefinitionId == workflowDefinitionId)
                .OrderByDescending(x => x.CreatedDateTime).Select(x => x.WorkflowInstanceId).FirstOrDefaultAsync();

            var assessmentQuestion =
                await _dbContext.Set<AssessmentQuestion>().Where(x => x.WorkflowInstanceId == latestInstance && x.ActivityName == activityName).FirstOrDefaultAsync();
            return assessmentQuestion;
        }
    }
}
