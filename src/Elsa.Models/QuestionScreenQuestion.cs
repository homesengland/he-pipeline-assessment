namespace Elsa.CustomModels
{
    public class QuestionScreenQuestion : AuditableEntity
    {
        public int Id { get; set; }
        public string ActivityId { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;
        public string CorrelationId { get; set; } = null!;
        public string? Question { get; set; }
        public string? QuestionId { get; set; }
        public string? QuestionType { get; set; }
        public double? Weighting { get; set; }
        public string? Comments { get; set; }

        public virtual List<QuestionChoice>? Choices { get; set; }
        public virtual List<QuestionAnswer>? Answers { get; set; }
    }
}
