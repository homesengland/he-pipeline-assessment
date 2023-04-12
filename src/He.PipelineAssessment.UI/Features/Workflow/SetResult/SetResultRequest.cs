using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.SetResult
{
    public class SetResultRequest : IRequest<QuestionScreenSaveAndContinueCommandResponse>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
    }
}
