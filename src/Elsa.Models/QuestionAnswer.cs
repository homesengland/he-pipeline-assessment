namespace Elsa.CustomModels
{
    public class QuestionAnswer : AuditableEntity
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public QuestionScreenQuestion Question { get; set; } = null!;

        public int? QuestionChoiceId { get; set; }
        public QuestionChoice? Choice { get; set; } = null!;

        public string Answer { get; set; } = null!;

    }
}
