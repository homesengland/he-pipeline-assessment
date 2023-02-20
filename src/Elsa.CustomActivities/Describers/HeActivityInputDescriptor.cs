using Elsa.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Describers
{
    public class HeActivityInputDescriptor : ActivityInputDescriptor
    {
        public bool DisplayInDesigner { get; set; } = true;

        //Used to match to a given Nested Activity Type (i.e. Question Type) in Elsa and only to be displayed if null, or a match.
        public string? ConditionalActivityType { get; set; }
        public string? ExpectedOutputType { get; set; }

        public HeActivityInputDescriptor() : base() { }

        public HeActivityInputDescriptor(string name,
            Type type,
            string uiHint,
            string label,
            string expectedOutputType,
            string? hint = null,
            object? options = null,
            string? category = null,
            float order = 0f,
            object? defaultValue = null,
            string? defaultSyntax = "Literal",
            IEnumerable<string>? supportedSyntaxes = null,
            bool isReadOnly = false,
            bool isBrowsable = true,
            bool isDesignerCritical = false,
            string? defaultWorkflowStorageProvider = null,
            bool disableWorkflowProviderSelection = false,
            bool considerValuesAsOutcomes = false,
            bool displayInDesigner = true,
            string? conditionalActivityType = null

            ) 
            : base(name, type, uiHint, label, hint, options, category, order, defaultValue, defaultSyntax, supportedSyntaxes, isReadOnly, isBrowsable, isDesignerCritical, defaultWorkflowStorageProvider, disableWorkflowProviderSelection, considerValuesAsOutcomes) 
        {
            DisplayInDesigner = displayInDesigner;
            ConditionalActivityType = conditionalActivityType;
            ExpectedOutputType = expectedOutputType;
        }
    }

    
}
