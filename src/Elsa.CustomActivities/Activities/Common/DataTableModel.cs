using Elsa.CustomWorkflow.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Activities.Common
{
    public class DataTable
    {
        public ICollection<TableInput> Inputs { get; set; } = new List<TableInput>();
        public string TypeOfInput { get; set; } = DataTableInputTypeConstants.CurrencyDataTableInput;
        public string? DisplayGroupId { get; set; }
    }

    public record TableInput(string? Identifier,string Title, string? Input = null);
}
