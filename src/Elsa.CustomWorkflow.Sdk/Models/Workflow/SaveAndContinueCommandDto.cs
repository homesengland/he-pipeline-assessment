namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class SaveAndContinueCommandDto
    {
        public string Id { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;

        public List<Answer>? Answers { get; set; }
    }

    public record Answer(string Id, string? AnswerText, string? Comments);
}