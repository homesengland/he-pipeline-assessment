using Elsa.CustomModels;
using static Elsa.CustomActivities.Activities.ConfirmationScreen.ConfirmationScreen;

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
        public List<string> Text { get; set; } = new List<string>();
        public string? NextWorkflowDefinitionIds { get; set; } = null!;
        public List<QuestionScreenAnswer>? CheckQuestionScreenAnswers { get; set; }
    }
}
