using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequest : IRequest<SaveAndContinueCommand>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

        public string ActivityType { get; set; } = null!;
    }
}
