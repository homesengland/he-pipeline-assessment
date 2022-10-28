using He.PipelineAssessment.Infrastructure.Models;

namespace He.PipelineAssessment.Infrastructure.Repository
{
    public interface IPipelineAssessmentRepository
    {
        Task<AssessmentStageVersion?> GetLatestAssessmentStageVersion(string workflowDefinitionId,
            CancellationToken cancellationToken = default);

        Task<AssessmentStageVersion?> GetSpecificAssessmentStageVersion(string workflowDefinitionId, int version,
            CancellationToken cancellationToken = default);

    }
}
