using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows
{
    public record AssessmentToolWorkflowQuery(int AssessmentToolId) : IRequest<AssessmentToolWorkflowListDto>
    {
    }
}
