using He.PipelineAssessment.UI.Features.Workflow.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class GetAssessmentDTOCommand : IRequest<GetAssessmentDTOCommandResponse>
    {
        public int ProjectId { get; set; }
        public int AssessmentToolWorkflowInstanceId { get; set; }
        public string WorkflowInstanceId { get; set; } = string.Empty;
    }
}
