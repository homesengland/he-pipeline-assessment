namespace Elsa.CustomActivities.Activities.Common
{
    public class WeightedCheckboxModel
    {
        public IDictionary<string, WeightedCheckboxGroup> Groups { get; set; } = new Dictionary<string, WeightedCheckboxGroup>();

    }
    public class WeightedCheckboxGroup
    {
        public ICollection<WeightedCheckboxRecord> Choices { get; set; } = new List<WeightedCheckboxRecord>();
        public int MaxGroupScore { get; set; }

    }
    public record WeightedCheckboxRecord(string Identifier, string Answer, bool IsSingle, string Score, bool IsPrePopulated);
}
