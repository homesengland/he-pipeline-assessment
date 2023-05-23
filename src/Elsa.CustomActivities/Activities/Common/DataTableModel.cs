namespace Elsa.CustomActivities.Activities.Common
{
    public class DataTable
    {
        public ICollection<TableInput> Inputs { get; set; } = new List<TableInput>();
        public string InputType { get; set; } = null!;
        public string? DisplayGroupId { get; set; }
    }

    public record TableInput(string Title, string? Input = null);
}
