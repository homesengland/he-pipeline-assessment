using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersScreenRequest : IRequest<SaveAndContinueCommand>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
    }
}
