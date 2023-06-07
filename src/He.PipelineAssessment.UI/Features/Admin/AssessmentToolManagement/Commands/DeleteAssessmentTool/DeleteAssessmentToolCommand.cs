using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentTool
{
    public record DeleteAssessmentToolCommand(int Id) : IRequest<int>;
}
