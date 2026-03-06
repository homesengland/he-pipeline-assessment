using Newtonsoft.Json;

namespace Elsa.CustomModels
{
    public class VariableInstance : AuditableEntity
    {
        public int Id { get; set; }
        public int SpId { get; set; }
        public int VariableId { get; set; }
        public string? Value { get; set; }
        public string? LastUpdatedBy { get; set; }
        public virtual Variable Variable { get; set; } = null!;
    }
}
