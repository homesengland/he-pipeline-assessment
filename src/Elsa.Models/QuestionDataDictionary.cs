using Newtonsoft.Json;

namespace Elsa.CustomModels
{
    public class QuestionDataDictionary : AuditableEntity
    {
        public int Id { get; set; }
        public int QuestionDataDictionaryGroupId { get; set; }
        [JsonIgnore]
        public QuestionDataDictionaryGroup Group { get; set; } = null!;
        public string Name { get; set; }
        public string LegacyName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
