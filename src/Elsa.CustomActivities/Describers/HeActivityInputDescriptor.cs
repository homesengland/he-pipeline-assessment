using Elsa.Metadata;

namespace Elsa.CustomActivities.Describers
{
    public class HeActivityInputDescriptor : ActivityInputDescriptor
    {
        public bool DisplayInDesigner { get; set; } = true;

        //Used to match to a given Nested Activity Type (i.e. Question Type) in Elsa and only to be displayed if null, or a match.
        public IEnumerable<string>? ConditionalActivityTypes { get; set; }
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
            string? defaultSyntax = null,
            IEnumerable<string>? supportedSyntaxes = null,
            bool isReadOnly = false,
            bool isBrowsable = true,
            bool isDesignerCritical = false,
            string? defaultWorkflowStorageProvider = null,
            bool disableWorkflowProviderSelection = false,
            bool considerValuesAsOutcomes = false,
            bool displayInDesigner = true,
            IEnumerable<string>? conditionalActivityTypes = null
        )
            : base(name, type, uiHint, label, hint, options, category, order, defaultValue, defaultSyntax, supportedSyntaxes, isReadOnly, isBrowsable, isDesignerCritical, defaultWorkflowStorageProvider, disableWorkflowProviderSelection, considerValuesAsOutcomes)
        {
            DisplayInDesigner = displayInDesigner;
            ConditionalActivityTypes = conditionalActivityTypes ?? new List<string>();
            ExpectedOutputType = expectedOutputType;
        }
    }


}
