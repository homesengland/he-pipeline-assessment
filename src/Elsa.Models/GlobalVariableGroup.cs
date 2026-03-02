namespace Elsa.CustomModels
{
    public class GlobalVariableGroup : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public required string Type { get; set; }
        public bool IsArchived { get; set; } = false;
        public virtual List<GlobalVariable> GlobalVariableList { get; set; } = new List<GlobalVariable>();
    }
}
