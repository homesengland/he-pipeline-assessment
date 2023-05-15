using Newtonsoft.Json;

namespace Elsa.CustomModels
{
    public class QuestionChoiceGroup : AuditableEntity
    {
        public int Id { get; set; }
        public string GroupIdentifier { get; set; } = null!;
        [JsonIgnore]
        public virtual List<QuestionChoice> QuestionGroupChoices { get; set; } = new();
    }
}
