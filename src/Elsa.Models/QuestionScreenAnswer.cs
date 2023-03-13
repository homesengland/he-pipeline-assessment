namespace Elsa.CustomModels
{
    public class QuestionScreenAnswer : AuditableEntity
    {
        public int Id { get; set; }
        public string ActivityId { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;
        public string CorrelationId { get; set; } = null!;
        public string? Question { get; set; }
        public string? QuestionId { get; set; }
        public string? QuestionType { get; set; }
        public string? Answer { get; set; }
        public string? Comments { get; set; }
        public List<Choice>? Choices { get; set; }

        public void SetAnswer(string? answer, DateTime lastModifiedDateTime)
        {
            this.Answer = answer;
            this.LastModifiedDateTime = lastModifiedDateTime;
        }
        public class Choice
        {
            public Choice()
            {

            }
            public string Identifier { get; set; } = null!;
            public string Answer { get; set; } = null!;
            public bool IsSingle { get; set; }
        }
    }
}
