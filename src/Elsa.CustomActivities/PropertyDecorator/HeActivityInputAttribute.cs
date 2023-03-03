using Elsa.Attributes;

namespace Elsa.CustomActivities.PropertyDecorator
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HeActivityInputAttribute : ActivityInputAttribute
    {
        public bool DisplayInDesigner { get; set; } = true;

        //Used to match to a given Nested Activity Type (i.e. Question Type) in Elsa and only to be displayed if null, or a match.
        public string[]? ConditionalActivityTypes { get; set; }
        public string? ExpectedOutputType { get; set; }
    }
}
