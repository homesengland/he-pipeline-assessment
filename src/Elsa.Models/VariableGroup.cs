namespace Elsa.CustomModels
{
    public class VariableGroup : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public required string Type { get; set; }
        public bool IsArchived { get; set; } = false;
        public virtual List<Variable> Variables { get; set; } = new List<Variable>();
    }

    public class VariableGroupTypes
    {
        public const string Dynamic = "dynamic";
        public const string Static = "static";
    }
}
