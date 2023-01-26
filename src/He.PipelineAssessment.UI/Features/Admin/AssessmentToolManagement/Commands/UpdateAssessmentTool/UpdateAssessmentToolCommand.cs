using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool
{
    public class UpdateAssessmentToolCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
    }
}
