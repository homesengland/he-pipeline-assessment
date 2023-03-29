namespace Elsa.CustomModels
{
    public class Answer : AuditableEntity
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public int? QuestionChoiceId { get; set; }
        public QuestionChoice? Choice { get; set; } = null!;

        public string AnswerText { get; set; } = null!;

    }
}
