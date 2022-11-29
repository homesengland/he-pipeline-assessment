using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen
{
    public class LoadQuestionScreenRequest : IRequest<QuestionScreenSaveAndContinueCommand>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
    }
}
