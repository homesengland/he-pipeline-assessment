using Elsa.Attributes;

namespace Elsa.CustomActivities.Activities.MultipleQuestionActivity
{
    public class CheckboxQuestion : Question
    {
        [ActivityInput(Label = "Checkbox questions", Hint = "Possible assessment screen answers.",
            UIHint = "multiChoice-record-builder", DefaultSyntax = "Json", IsDesignerCritical = true)]
        public CheckboxModel Checkbox { get; set; } = new CheckboxModel();
    }

    public class CheckboxModel
    {
        public ICollection<CheckboxRecord> Choices { get; set; } = new List<CheckboxRecord>();
    }

    public record CheckboxRecord(string Answer, bool IsSingle);
}