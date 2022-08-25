namespace Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue
{
    public class SaveAndContinueCommandDto
    {
        public string Id { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;

        public List<string> Answers { get; set; } = null!;
    }

}