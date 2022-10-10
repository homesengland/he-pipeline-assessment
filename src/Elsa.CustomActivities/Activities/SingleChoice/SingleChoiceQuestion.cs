using Elsa.Attributes;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.Models;

namespace Elsa.CustomActivities.Activities.SingleChoice
{
    [Trigger(
        Category = "Homes England Activities",
        Description = "Assessment screen radio button question",
        Outcomes = new[] { OutcomeNames.Done },
        DisplayName = "Radio Button Question"
    )]
    public class SingleChoiceQuestion : CustomQuestion
    {
        [ActivityInput(Label = "Radio-button questions", Hint = "Possible assessment screen answers.",
            UIHint = "singleChoice-record-builder", DefaultSyntax = "Json", IsDesignerCritical = true, Name = "Radio Button Question")]
        public SingleChoiceModel SingleChoice { get; set; } = new SingleChoiceModel();

    }

    public class SingleChoiceModel
    {
        public ICollection<SingleChoiceRecord> Choices { get; set; } = new List<SingleChoiceRecord>();
    }

    public record SingleChoiceRecord(string Answer);
}