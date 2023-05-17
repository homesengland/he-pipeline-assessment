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
        public Type TypeOfInput { get; set; } = typeof(string);
    }

    public record TableInput(string Title, string? Input = null);
}
