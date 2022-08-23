using Elsa.CustomModels;

namespace Elsa.CustomInfrastructure.Data.Repository
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

        Task<MultipleChoiceQuestionModel?> UpdateMultipleChoiceQuestion(MultipleChoiceQuestionModel model, CancellationToken cancellationToken = default);
    }
}
