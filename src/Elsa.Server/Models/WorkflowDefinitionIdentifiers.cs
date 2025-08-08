namespace Elsa.Server.Models
{
    public class WorkflowDefinitionIdentifiers
    {
        public string DefinitionId { get; set; } = null!;
        public string Id { get; set; } = null!;
        public bool IsLatest { get; set; }
        public bool IsPublished { get; set; }
        public int Version { get; set; }
    }
}
