namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{ public class LoadWorkflowActivityResponse
    {
        public ActivityData? ActivityData { get; set; }
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
    }
    public class ActivityData
    {
        public string Title { get; set; } = null!;
        public Choice[] Choices { get; set; } = null!;
        public string Question { get; set; } = null!;
        public object? Output { get; set; }
    }

    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public bool IsSelected { get; set; }
    }
}
