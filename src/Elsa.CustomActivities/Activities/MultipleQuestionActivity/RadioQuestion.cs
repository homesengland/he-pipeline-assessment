using Elsa.Attributes;

namespace Elsa.CustomActivities.Activities.MultipleQuestionActivity
{
    public class RadioQuestion : Question
    {
        [ActivityInput(Label = "Radio-button questions", Hint = "Possible assessment screen answers.",
            UIHint = "singleChoice-record-builder", DefaultSyntax = "Json", IsDesignerCritical = true)]
        public RadioModel Radio { get; set; } = new RadioModel();
    }

    public class RadioModel
    {
        public ICollection<RadioRecord> Choices { get; set; } = new List<RadioRecord>();
    }

    public record RadioRecord(string Answer);
}