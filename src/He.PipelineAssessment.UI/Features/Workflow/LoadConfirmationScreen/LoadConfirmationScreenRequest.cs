using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequest : IRequest<LoadConfirmationScreenResponse>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

    }
}
