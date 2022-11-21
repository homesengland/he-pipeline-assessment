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

        public async Task<QuestionScreenAnswer?> UpdateQuestionScreenAnswer(QuestionScreenAnswer model, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task CreateQuestionScreenAnswersAsync(List<QuestionScreenAnswer> assessments, CancellationToken cancellationToken)
        {
            await _dbContext.AddRangeAsync(assessments, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<QuestionScreenAnswer>> GetQuestionScreenAnswers(string activityId, string workflowInstanceId,
            CancellationToken cancellationToken)
        {
            var list = await _dbContext.Set<QuestionScreenAnswer>().Where(x => x.ActivityId == activityId && x.WorkflowInstanceId == workflowInstanceId && x.QuestionId != null).ToListAsync();
            return list;
        }

        public Task<QuestionScreenAnswer> GetQuestionScreenAnswer(string activityId, string correlationId, string questionID,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }


    }
}
