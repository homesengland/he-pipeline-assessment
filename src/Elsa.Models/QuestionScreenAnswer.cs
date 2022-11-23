namespace Elsa.CustomModels
{
    public class QuestionScreenAnswer
    {
        public int Id { get; set; }
        public string ActivityId { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;
        public string? QuestionId { get; set; }
        public string? QuestionType { get; set; }
        public string? Answer { get; set; }
        public string? Comments { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public void SetAnswer(string? answer, DateTime lastModifiedDateTime)
        {
            this.Answer = answer;
            this.LastModifiedDateTime = lastModifiedDateTime;
        }
    }
}
