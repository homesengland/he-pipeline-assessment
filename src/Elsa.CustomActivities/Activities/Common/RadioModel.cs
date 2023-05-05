using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Expressions;

namespace Elsa.CustomActivities.Activities.Common
{
    public class RadioModel
    {
        [HeActivityInput(HasCollectedProperties = true,
            CollectedPropertyType = typeof(RadioProperty))]
        public ICollection<RadioRecord> Choices { get; set; } = new List<RadioRecord>();
    }

    public record RadioRecord(string Identifier, string Answer, bool IsPrePopulated);

    public class RadioProperty
    {
        [HeActivityInput(Hint = "Identifier", UIHint = HePropertyUIHints.SingleLine)]
        public string Identifier { get; set; } = null!;

        [HeActivityInput(Hint = "Answer", UIHint = HePropertyUIHints.SingleLine,
           SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript })]
        public string? Answer { get; set; }

        [HeActivityInput(Hint = "Pre Populated", UIHint = HePropertyUIHints.Checkbox)]
        public bool IsPrePopulated { get; set; } = false;

    }
}
