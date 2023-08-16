using Elsa.CustomModels;

namespace Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersScreenResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
        public string PreviousActivityType { get; set; } = null!;

        public string? PageTitle { get; set; } = null!;

        public string? FooterTitle { get; set; } = null!;
        public string? FooterText { get; set; } = null!;
        public List<Question>? CheckQuestions { get; set; }
    }
}
