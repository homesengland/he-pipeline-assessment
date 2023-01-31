using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentToolWorkflow
{
    public record DeleteAssessmentToolWorkflowCommand(int Id) : IRequest<int>;
}
