using Elsa.Attributes;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.Models;

namespace Elsa.CustomActivities.Activities.MultipleChoice
{
    [Trigger(
        Category = "Homes England Activities",
        Description = "Assessment screen multiple choice question",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class MultipleChoiceQuestion : CustomQuestion
    {
        [ActivityInput(Label = "Multi-choice questions", Hint = "Possible assessment screen answers.",
            UIHint = "multiChoice-record-builder", DefaultSyntax = "Json", IsDesignerCritical = true)]
        public MultipleChoiceModel MultipleChoice { get; set; } = new MultipleChoiceModel();
        //public ICollection<MultiChoiceRecord> Choices { get; set; } = new List<MultiChoiceRecord>();
        //public bool IsMultiSelect { get; set; } = true;
    }

    public class MultipleChoiceModel
    {
        public ICollection<MultiChoiceRecord> Choices { get; set; } = new List<MultiChoiceRecord>();
    }

    public record MultiChoiceRecord(string Answer, bool IsSingle);
}