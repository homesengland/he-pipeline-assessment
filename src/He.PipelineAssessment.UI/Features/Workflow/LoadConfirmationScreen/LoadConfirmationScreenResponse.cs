using He.PipelineAssessment.UI.Features.Workflow.ViewModels;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenResponse : PageHeaderInformation
    {
        public int AssessmentId { get; set; }
        public string CorrelationId { get; set; } = null!;
        public bool IsLatestSubmittedWorkflow { get; set; }
    }
}
