using System.Text.Json;

namespace He.PipelineAssessment.UI.Helper;

public static class DataTableHelper
{
    public static DataTableModel ToTableModel(this string jsonModel)
    {
        var dataTableModel = JsonSerializer.Deserialize<DataTableModel>(jsonModel);
        return dataTableModel ?? new DataTableModel();
    }

    public class DataTableModel
    {
        public string InputType { get; set; } = null!;
        public List<DataTableRow> Inputs { get; set; } = new List<DataTableRow>();
    }
    public class DataTableRow
    {
        public string? Title { get; set; }
        public string? Input { get; set; }
    }
}