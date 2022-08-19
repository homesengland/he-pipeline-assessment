namespace Elsa.CustomWorkflow.Sdk.Models
{
    public class WorkflowNavigationDto
    {
        public ActivityData ActivityData { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
    }

    public class ActivityData
    {
        public string Title { get; set; } = null!;
        public Choice[] Choices { get; set; } = null!;
        public string Question { get; set; } = null!;
        public object Output { get; set; } = null!;
    }

    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public bool IsSelected { get; set; }
    }
}
