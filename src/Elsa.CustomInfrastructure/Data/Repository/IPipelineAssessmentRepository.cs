using Elsa.CustomModels;

namespace Elsa.CustomInfrastructure.Data.Repository
{
    public interface IPipelineAssessmentRepository
    {
        Task<AssessmentQuestion?> GetAssessmentQuestion(string activityId,
            string workflowInstanceId,
            CancellationToken cancellationToken = default);

        ValueTask<int?> CreateAssessmentQuestionAsync(AssessmentQuestion model,
            CancellationToken cancellationToken = default);

        Task<AssessmentQuestion?> UpdateAssessmentQuestion(AssessmentQuestion model, CancellationToken cancellationToken = default);
    }
}
