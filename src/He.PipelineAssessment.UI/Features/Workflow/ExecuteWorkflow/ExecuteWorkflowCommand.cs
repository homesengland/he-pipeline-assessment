using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.ExecuteWorkflow
{
    public class ExecuteWorkflowCommand : IRequest<LoadQuestionScreenRequest>
    {
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;
    }
}
