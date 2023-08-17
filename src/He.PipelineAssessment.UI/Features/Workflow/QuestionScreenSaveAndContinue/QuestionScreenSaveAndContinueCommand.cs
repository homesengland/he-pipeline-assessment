using He.PipelineAssessment.UI.Features.Workflow.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommand : PageHeaderInformation, IRequest<QuestionScreenSaveAndContinueCommandResponse>
    {
        public int AssessmentId { get; set; }
        public string CorrelationId { get; set; } = null!;
        public bool IsAuthorised { get; set; }
        public bool IsReadOnly { get; set; }
        public string WorkflowDefinitionId { get; set; } = null!;

    }

}
