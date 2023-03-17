namespace Elsa.CustomActivities.Activities.Common
{
    public class RadioModel
    {
        public ICollection<RadioRecord> Choices { get; set; } = new List<RadioRecord>();
    }

    public record RadioRecord(string Identifier, string Answer, bool IsPrePopulated);
}
