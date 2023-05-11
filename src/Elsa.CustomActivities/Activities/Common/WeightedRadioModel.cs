namespace Elsa.CustomActivities.Activities.Common
{
    public class WeightedRadioModel
    {
        public ICollection<WeightedRadioRecord> Choices { get; set; } = new List<WeightedRadioRecord>();
    }

    public record WeightedRadioRecord(string Identifier, string Answer, decimal Score, bool IsPrePopulated);
}
