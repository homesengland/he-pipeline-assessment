namespace Elsa.CustomModels
{
    public class MultipleChoiceQuestionModel
    {
        public string Id { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;

        public string? Answer { get; set; }

        public bool? FinishWorkflow { get; set; }
        public bool? NavigateBack { get; set; }
        public string PreviousActivityId { get; set; } = null!;
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public void SetAnswer(List<string> answersList, DateTime lastModifiedDateTime)
        {
            this.Answer = string.Join(Constants.StringSeparator, answersList);
            this.LastModifiedDateTime = lastModifiedDateTime;
        }
    }
}
