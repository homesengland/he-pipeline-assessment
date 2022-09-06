using Elsa.CustomModels;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public interface IPipelineAssessmentRepository
    {
        Task<AssessmentQuestion?> GetMultipleChoiceQuestions(string id,
            CancellationToken cancellationToken = default);

        Task<AssessmentQuestion?> GetMultipleChoiceQuestions(string activityId,
            string workflowInstanceId,
            CancellationToken cancellationToken = default);

        ValueTask<string?> CreateMultipleChoiceQuestionAsync(AssessmentQuestion model,
            CancellationToken cancellationToken = default);

        Task<AssessmentQuestion?> UpdateMultipleChoiceQuestion(AssessmentQuestion model, CancellationToken cancellationToken = default);
    }
}
