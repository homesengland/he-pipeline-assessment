using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.ReturnToActivity
{
    public class ReturnToActivityCommand : IRequest<ReturnToActivityCommandResponse>
    {
            public string WorkflowInstanceId { get; set; }
            public string ActivityId { get; set; }

    }
}
