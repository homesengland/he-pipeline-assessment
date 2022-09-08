using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommand : IRequest<LoadWorkflowActivityRequest>
    {
        public string WorkflowDefinitionId { get; set; } = null!;
    }
}
