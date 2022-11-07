namespace Elsa.CustomActivities.Activities.QuestionScreen
{
    public class CheckboxQuestion : Question
    {
        public CheckboxModel Checkbox { get; set; } = new CheckboxModel();

        public override string QuestionType => Constants.CheckboxQuestion;
    }

    public class CheckboxModel
    {
        public ICollection<CheckboxRecord> Choices { get; set; } = new List<CheckboxRecord>();
    }

    public record CheckboxRecord(string Answer, bool IsSingle);
}