using System.Text.Json;

namespace He.PipelineAssessment.UI.Helper;

public static class DataTableHelper
{
    public static DataTableModel ToTableModel(this string jsonModel)
    {
        var dataTableModel = new DataTableModel();
        var dataTableRows = JsonSerializer.Deserialize<List<DataTableRow>>(jsonModel);
        if(dataTableRows != null)
        {
            dataTableModel.DataTableRows.AddRange(dataTableRows);
        }
        return dataTableModel;
    }

    public class DataTableModel
    {
        public List<DataTableRow> DataTableRows { get; set; } = new List<DataTableRow>();
    }
    public class DataTableRow
    {
        public string? Title { get; set; }
        public string? Input { get; set; }
    }
}