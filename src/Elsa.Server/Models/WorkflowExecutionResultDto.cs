namespace Elsa.Server.Models
{
    public class WorkflowExecutionResultDto
    {
        public ActivityData? ActivityData { get; set; }
        public string WorkflowInstanceId { get; set; }
        public string ActivityId { get; set; }
    }

    public class ActivityData
    {
        public string Title { get; set; }
        public Choice[] Choices { get; set; }
        public string Question { get; set; }
        public object Output { get; set; }
    }

    public class Choice
    {
        public string Answer { get; set; }
        public bool IsSingle { get; set; }
        public bool IsSelected { get; set; }
    }
}
