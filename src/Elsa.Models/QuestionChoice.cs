using Newtonsoft.Json;

namespace Elsa.CustomModels
{
    public class QuestionChoice : AuditableEntity
    {

        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int? QuestionChoiceGroupId { get; set; }
        [JsonIgnore]
        public Question Question { get; set; } = null!;
        public QuestionChoiceGroup? QuestionChoiceGroup { get; set; }

        public string Identifier { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public bool IsPrePopulated { get; set; }
        public string? PotScoreCategory { get; set; }
        public decimal? NumericScore { get; set; }
        public bool IsExclusiveToQuestion { get; set; }

    }
}
