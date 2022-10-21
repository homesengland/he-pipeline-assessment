using Elsa.CustomModels;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public interface IElsaCustomRepository
    {
        Task<AssessmentQuestion?> GetAssessmentQuestion(string activityId,
            string workflowInstanceId,
            CancellationToken cancellationToken = default);

        ValueTask<int?> CreateAssessmentQuestionAsync(AssessmentQuestion model,
            CancellationToken cancellationToken = default);

        Task<AssessmentQuestion?> UpdateAssessmentQuestion(AssessmentQuestion model, CancellationToken cancellationToken = default);
        Task<IEnumerable<AssessmentQuestion>?> GetAssessmentQuestions(string workflowDefinitionId,
            string correlationId);

        Task<AssessmentQuestion?> GetAssessmentQuestion(string workflowDefinitionId,
            string correlationId, string activityName, CancellationToken cancellationToken = default);
    }
}
