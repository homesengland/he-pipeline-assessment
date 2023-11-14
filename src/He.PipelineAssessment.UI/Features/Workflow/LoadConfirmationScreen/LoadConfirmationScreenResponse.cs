using He.PipelineAssessment.UI.Features.Workflow.ViewModels;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenResponse : PageHeaderInformation
    {
        public bool IsLatestSubmittedWorkflow { get; set; }
        public bool IsAmendableWorkflow { get; set; }
        public bool IsVariationAllowed { get; set; }
        public bool IsAuthorised { get; set; }
    }
}
