using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand
{
    public class UpdateAssessmentToolWorkflowCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string WorkflowDefinitionId { get; set; } = string.Empty;
        public bool IsFirstWorkflow { get; set; }
        public int AssessmentToolId { get; set; }
        public int Version { get; set; } = 1;
        public bool IsLatest { get; set; } = true;
    }
}
