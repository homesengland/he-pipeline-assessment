using He.PipelineAssessment.UI.Features.Workflow.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommand : PageHeaderInformation, IRequest<QuestionScreenSaveAndContinueCommandResponse>
    {

        public bool IsAuthorised { get; set; }
        public bool IsReadOnly { get; set; }
        public string WorkflowDefinitionId { get; set; } = null!;
    }
}
