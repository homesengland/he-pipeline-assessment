using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommand : IRequest<LoadQuestionScreenRequest>
    {
        public string WorkflowDefinitionId { get; set; } = null!;
    }
}
