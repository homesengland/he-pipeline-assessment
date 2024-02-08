using Newtonsoft.Json;

namespace Elsa.CustomModels
{
    public class DataDictionary : AuditableEntity
    {
        public int Id { get; set; }
        public int DataDictionaryGroupId { get; set; }
        [JsonIgnore]
        public DataDictionaryGroup Group { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? LegacyName { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }

        public bool IsArchived { get; set; } = false;
    }
}
