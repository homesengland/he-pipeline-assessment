using Newtonsoft.Json;

namespace Elsa.CustomModels
{
    public class GlobalVariable : AuditableEntity
    {
        public int Id { get; set; }
        public int GlobalVariableGroupId { get; set; }
        [JsonIgnore]
        public GlobalVariableGroup Group { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Value { get; set; }
        public bool IsArchived { get; set; } = false;
    }
}
