namespace Elsa.CustomActivities.Activities.Common
{
    public class CheckboxModel
    {
        public ICollection<CheckboxRecord> Choices { get; set; } = new List<CheckboxRecord>();
    }

    public record CheckboxRecord(string Identifier, string Answer, bool IsSingle, bool IsPrePopulated);
}
