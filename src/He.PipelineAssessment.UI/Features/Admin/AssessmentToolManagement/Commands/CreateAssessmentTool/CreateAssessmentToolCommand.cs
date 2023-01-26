using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool
{
    public class CreateAssessmentToolCommand : IRequest<CreateAssessmentToolData>
    {
        public CreateAssessmentToolDto AssessmentToolDto { get; set; } = null!;
    }
}
