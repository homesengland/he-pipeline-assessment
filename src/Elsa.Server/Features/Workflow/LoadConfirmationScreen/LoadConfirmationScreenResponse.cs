using Elsa.CustomModels;

namespace Elsa.Server.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
        public string PreviousActivityType { get; set; } = null!;

        public string? ConfirmationTitle { get; set; } = null!;
        public bool ShowToolName { get; set; } = true;
        public string? ConfirmationText { get; set; } = null!;
        public string? FooterTitle { get; set; } = null!;
        public string? FooterText { get; set; } = null!;
        public List<Information> Text { get; set; } = null!;
        public string? NextWorkflowDefinitionIds { get; set; } = null!;
        public List<Question>? CheckQuestions { get; set; }
    }

    public class Information
    {
        public string? Title { get; set; }
        public bool IsGuidance { get; set; } = false;
        public bool IsCollapsed { get; set; } = false;
        public bool IsBullets { get; set; } = false;
        public bool DisplayOnPage { get; set; } = true;
        public List<InformationText> InformationTextList { get; set; } = new List<InformationText>();
    }
}
