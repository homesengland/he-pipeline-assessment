using Elsa.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Server.Data
{
    public interface IPipelineAssessmentRepository
    {
        Task<MultipleChoiceQuestionModel?> GetMultipleChoiceQuestions(string id,
            CancellationToken cancellationToken = default);

        Task<MultipleChoiceQuestionModel?> GetMultipleChoiceQuestions(string activityId,
            string workflowInstanceId,
            CancellationToken cancellationToken = default);

        ValueTask<string?> CreateMultipleChoiceQuestionAsync(MultipleChoiceQuestionModel model,
            CancellationToken cancellationToken = default);

        Task<MultipleChoiceQuestionModel?> UpdateMultipleChoiceQuestion(MultipleChoiceQuestionModel model, CancellationToken cancellationToken= default);
    }

    public class PipelineAssessmentRepository : IPipelineAssessmentRepository
    {
        private readonly DbContext _dbContext;
        public PipelineAssessmentRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MultipleChoiceQuestionModel?> GetMultipleChoiceQuestions(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<MultipleChoiceQuestionModel>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<MultipleChoiceQuestionModel?> GetMultipleChoiceQuestions(string activityId, string workflowInstanceId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<MultipleChoiceQuestionModel>().FirstOrDefaultAsync(x => x.ActivityId == activityId && x.WorkflowInstanceId == workflowInstanceId, cancellationToken);
        }

        public async ValueTask<string?> CreateMultipleChoiceQuestionAsync(MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model.Id;
        }

        public async Task<MultipleChoiceQuestionModel?> UpdateMultipleChoiceQuestion(MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return model;
        }
    }
}
