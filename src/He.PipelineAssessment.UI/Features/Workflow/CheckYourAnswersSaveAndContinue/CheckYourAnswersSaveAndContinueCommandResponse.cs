using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandResponse : IRequest<CheckYourAnswersSaveAndContinueCommand>
    {
        public int AssessmentId { get; set; }
        public string CorrelationId { get; set; } = null!;
    }
}
