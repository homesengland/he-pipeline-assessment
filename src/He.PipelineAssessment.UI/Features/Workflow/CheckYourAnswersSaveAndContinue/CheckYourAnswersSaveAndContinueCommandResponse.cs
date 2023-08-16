using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandResponse : IRequest<CheckYourAnswersSaveAndContinueCommand>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;

        public bool IsAuthorised { get; set; }
    }

}
