using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.SubmitAssessmentStage
{
    public class SubmitAssessmentStageCommand : IRequest<LoadWorkflowActivityRequest>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
    }
}
