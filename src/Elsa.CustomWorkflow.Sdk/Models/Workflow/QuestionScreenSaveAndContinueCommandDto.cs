namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class QuestionScreenSaveAndContinueCommandDto
    {
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;

        public List<Answer>? Answers { get; set; }
    }

    public record Answer(string WorkflowQuestionId, string? AnswerText, string? Comments, string? DocumentEvidenceLink, int? ChoiceId = null);
}