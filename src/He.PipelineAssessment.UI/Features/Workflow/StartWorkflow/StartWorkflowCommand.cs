using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommand : IRequest<LoadQuestionScreenRequest>
    {
        public string WorkflowDefinitionId { get; set; } = null!;
        public string CorrelationId { get; set; } = null!;
        public int AssessmentId { get; set; }
    }
}
