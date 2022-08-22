namespace Elsa.CustomWorkflow.Sdk.Models
{
    public class SaveAndContinueCommandDto
    {
        public string Id { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;

        public string Answer { get; set; } = null!;

        public string PreviousActivityId { get; set; } = null!;
    }
}