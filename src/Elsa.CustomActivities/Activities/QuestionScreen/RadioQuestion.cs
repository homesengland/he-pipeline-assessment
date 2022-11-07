namespace Elsa.CustomActivities.Activities.QuestionScreen
{
    public class RadioQuestion : Question
    {
        public RadioModel Radio { get; set; } = new RadioModel();

        public override string QuestionType => Constants.RadioQuestion;
    }

    public class RadioModel
    {
        public ICollection<RadioRecord> Choices { get; set; } = new List<RadioRecord>();
    }

    public record RadioRecord(string Answer);
}