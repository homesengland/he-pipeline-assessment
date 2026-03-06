using Newtonsoft.Json;

namespace Elsa.CustomModels
{
    public class Variable : AuditableEntity
    {
        public int Id { get; set; }
        public int VariableGroupId { get; set; }
        [JsonIgnore]
        public VariableGroup Group { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Value { get; set; }
        public bool IsArchived { get; set; } = false;
        public virtual List<VariableInstance> VariableInstances { get; set; } = new List<VariableInstance>();
    }
}
