using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomWorkflow.Sdk.Models.Activities
{
    public class HeActivityInputDescriptorDTO
    {
        public string Name { get; set; } = null!;
        public Type Type { get; set; } = null!;
        public string? UIHint { get; set; } = null;
        public string? Label { get; set; } = null;
        public string? Hint { get; set; }
        public object? Options { get; set; }
        public string? Category { get; set; }
        public float Order { get; set; }
        public object? DefaultValue { get; set; }
        public string? DefaultSyntax { get; set; }
        public IList<string> SupportedSyntaxes { get; set; } = new List<string>();
        public bool? IsReadOnly { get; set; }
        public bool? IsBrowsable { get; set; }
        public bool IsDesignerCritical { get; set; }
        public string? DefaultWorkflowStorageProvider { get; set; }
        public bool DisableWorkflowProviderSelection { get; set; }
        public bool ConsiderValuesAsOutcomes { get; set; }
        public bool DisplayInDesigner { get; set; } = true;
        public string? ConditionalActivityType { get; set; }
        public string? ExpectedOutputType { get; set; }
    }
}
