using Elsa.Attributes;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.Models;

namespace Elsa.CustomActivities.Activities.SingleChoice
{
    [Trigger(
        Category = "Homes England Activities",
        Description = "Assessment screen single choice question",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class SingleChoiceQuestion : CustomQuestion
    {
        [ActivityInput(Label = "Single-choice questions", Hint = "Possible assessment screen answers.",
            UIHint = "singleChoice-record-builder", DefaultSyntax = "Json", IsDesignerCritical = true)]
        public SingleChoiceModel SingleChoice { get; set; } = new SingleChoiceModel();
        //public ICollection<MultiChoiceRecord> Choices { get; set; } = new List<MultiChoiceRecord>();
        //public bool IsMultiSelect { get; set; } = true;
    }

    public class SingleChoiceModel
    {
        public ICollection<SingleChoiceRecord> Choices { get; set; } = new List<SingleChoiceRecord>();
    }

    public record SingleChoiceRecord(string Answer);
}