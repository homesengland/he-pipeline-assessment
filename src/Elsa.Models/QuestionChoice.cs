namespace Elsa.CustomModels
{
    public class QuestionChoice : AuditableEntity
    {

        public int Id { get; set; }
        public int QuestionId { get; set; }
        public QuestionScreenQuestion Question { get; set; } = null!;

        public string Identifier { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public bool IsPrePopulated { get; set; }
        public string? PotScoreCategory { get; set; }
        public double? NumericScore { get; set; }


    }
}
