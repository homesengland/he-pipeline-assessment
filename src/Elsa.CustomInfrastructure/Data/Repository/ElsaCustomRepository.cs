using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public class ElsaCustomRepository : IElsaCustomRepository
    {
        private readonly DbContext _dbContext;
        public ElsaCustomRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomActivityNavigation?> GetCustomActivityNavigation(string activityId, string workflowInstanceId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<CustomActivityNavigation>().FirstOrDefaultAsync(x => x.ActivityId == activityId && x.WorkflowInstanceId == workflowInstanceId, cancellationToken);
        }

        public async ValueTask<int?> CreateCustomActivityNavigationAsync(CustomActivityNavigation model, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model.Id;
        }

        public async Task<Question?> UpdateQuestion(Question model, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task<CustomActivityNavigation?> UpdateCustomActivityNavigation(CustomActivityNavigation model, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task CreateQuestionsAsync(List<Question> assessments, CancellationToken cancellationToken)
        {
            await _dbContext.AddRangeAsync(assessments, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Question>> GetActivityQuestions(string activityId, string workflowInstanceId,
            CancellationToken cancellationToken)
        {
            var list = await _dbContext.Set<Question>()
                .Where(x => x.ActivityId == activityId && x.WorkflowInstanceId == workflowInstanceId && x.QuestionId != null)
                .Include(x => x.Choices)!.ThenInclude(y => y.QuestionChoiceGroup)
                .Include(x => x.Answers)
                .ToListAsync(cancellationToken);
            return list;
        }

        public async Task<Question?> GetQuestionByCorrelationId(string activityId, string correlationId, string questionID,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.Set<Question>()
                .Where(x => x.ActivityId == activityId && x.CorrelationId == correlationId && x.QuestionId == questionID)
                .Include(x => x.Choices)
                .Include(x => x.Answers)
                .OrderByDescending(x => x.CreatedDateTime)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
           
            return result;
        }

        public async Task<Question?> GetQuestionById(int id)
        {
            var result = await _dbContext.Set<Question>().FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Question>> GetWorkflowInstanceQuestions(string workflowInstanceId, CancellationToken cancellationToken)
        {
            var list = await _dbContext.Set<Question>()
                .Where(x => x.WorkflowInstanceId == workflowInstanceId)
                .Include(x => x.Choices)
                .Include(x => x.Answers)
                .ToListAsync(cancellationToken);
            return list;
        }

        public async Task<CustomActivityNavigation?> GetChangedPathNavigation(string workflowInstanceId,
            string currentActivityId, string nextActivityId, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<CustomActivityNavigation>()
                .Where(x => x.WorkflowInstanceId == workflowInstanceId && x.PreviousActivityId == currentActivityId && x.ActivityId != nextActivityId &&
                            x.ActivityId != currentActivityId).OrderByDescending(x => x.LastModifiedDateTime)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task DeleteCustomNavigations(List<string> previousPathActivities, string workflowInstanceId, CancellationToken cancellationToken)
        {
            var list = _dbContext.Set<CustomActivityNavigation>().Where(x => x.WorkflowInstanceId == workflowInstanceId && previousPathActivities.Contains(x.ActivityId));
            _dbContext.RemoveRange(list);
            await SaveChanges(cancellationToken);
        }

        public async Task DeleteQuestions(string workflowInstanceId, List<string> previousPathActivities, CancellationToken cancellationToken)
        {
            var list = _dbContext.Set<Question>().Where(x => x.WorkflowInstanceId == workflowInstanceId && previousPathActivities.Contains(x.ActivityId));
            _dbContext.RemoveRange(list);
            await SaveChanges(cancellationToken);
        }

        public async Task<List<PotScoreOption>> GetPotScoreOptionsAsync(CancellationToken cancellationToken) =>
            await _dbContext.Set<PotScoreOption>().Where(x => x.IsActive).ToListAsync();

        public async Task CreateQuestionWorkflowInstance(QuestionWorkflowInstance questionWorkflowInstance, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(questionWorkflowInstance, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<QuestionWorkflowInstance?> GetQuestionWorkflowInstance(string workflowInstanceId, CancellationToken cancellationToken = default)
        {
            QuestionWorkflowInstance? workflow = await _dbContext.Set<QuestionWorkflowInstance>().FirstOrDefaultAsync(x => x.WorkflowInstanceId == workflowInstanceId);
            return workflow;
        }

        public async Task<List<QuestionWorkflowInstance>> GetQuestionWorkflowInstancesByName(string correlationId, string name,
            CancellationToken cancellationToken = default)
        {
            var workflowInstances = await _dbContext.Set<QuestionWorkflowInstance>()
                .Where(x => x.CorrelationId == correlationId && x.WorkflowName == name).ToListAsync(cancellationToken);
            return workflowInstances;
        }

        public async Task<QuestionWorkflowInstance?> GetQuestionWorkflowInstanceByDefinitionId(string workflowDefinitionId, string correlationId,
            CancellationToken cancellationToken = default)
        {
            var workflowInstance = await _dbContext.Set<QuestionWorkflowInstance>()
                .FirstOrDefaultAsync(
                    x => x.WorkflowDefinitionId == workflowDefinitionId && x.CorrelationId == correlationId,
                    cancellationToken: cancellationToken);

            return workflowInstance;
        }

        public async Task SetWorkflowInstanceResult(string workflowInstanceId, string result, CancellationToken cancellationToken = default)
        {
            var workflowInstance = _dbContext.Set<QuestionWorkflowInstance>().Where(x => x.WorkflowInstanceId == workflowInstanceId).ToList();
            foreach(var workflow in workflowInstance)
            {
                workflow.Result = result;
            }
            _dbContext.UpdateRange(workflowInstance);
            await SaveChanges(cancellationToken);
        }

        public async Task SetWorkflowInstanceScore(string workflowInstanceId, string score, CancellationToken cancellationToken = default)
        {
            var workflowInstance = _dbContext.Set<QuestionWorkflowInstance>().Where(x => x.WorkflowInstanceId == workflowInstanceId).ToList();
            foreach (var workflow in workflowInstance)
            {
                workflow.Score = score;
            }
            _dbContext.UpdateRange(workflowInstance);
            await SaveChanges(cancellationToken);
        }
    }
}
