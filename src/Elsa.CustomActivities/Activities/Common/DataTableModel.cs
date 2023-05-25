namespace Elsa.CustomActivities.Activities.Common
{
    public class DataTable
    {
        public List<TableInput> Inputs { get; set; } = new List<TableInput>();
        public string InputType { get; set; } = null!;
        public string? DisplayGroupId { get; set; }
    }

    public record TableInput(string? Identifier,string? Title, bool IsReadOnly, bool IsSummaryTotal, string? Input = null);
}
