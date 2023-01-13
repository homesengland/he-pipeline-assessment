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
        public string? ConfirmationText { get; set; } = null!;
        public string? FooterTitle { get; set; } = null!;
        public string? FooterText { get; set; } = null!;
        public string? AdditionalTextLine1 { get; set; } = null!;
        public string? AdditionalTextLine2 { get; set; } = null!;
        public string? AdditionalTextLine3 { get; set; } = null!;
        public string? AdditionalTextLine4 { get; set; } = null!;
        public string? AdditionalTextLine5 { get; set; } = null!;
        public string? NextWorkflowDefinitionId { get; set; } = null!;
        public List<QuestionScreenAnswer>? CheckQuestionScreenAnswers { get; set; }
    }
}
