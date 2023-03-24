namespace Elsa.CustomModels
{
    public class QuestionScreenAnswer : AuditableEntity
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public QuestionScreenQuestion Question { get; set; } = null!;

        public int? QuestionScreenChoiceId { get; set; }
        public QuestionScreenChoice? Choice { get; set; } = null!;

        public string Answer { get; set; } = null!;

    }
}
