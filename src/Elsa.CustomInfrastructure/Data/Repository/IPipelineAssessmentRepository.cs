using Elsa.CustomModels;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public interface IPipelineAssessmentRepository
    {
        Task<AssessmentQuestion?> GetAssessmentQuestion(string id,
            CancellationToken cancellationToken = default);

        Task<AssessmentQuestion?> GetAssessmentQuestion(string activityId,
            string workflowInstanceId,
            CancellationToken cancellationToken = default);

        ValueTask<string?> CreateAssessmentQuestionAsync(AssessmentQuestion model,
            CancellationToken cancellationToken = default);

        Task<AssessmentQuestion?> UpdateAssessmentQuestion(AssessmentQuestion model, CancellationToken cancellationToken = default);
    }
}
