namespace Elsa.CustomActivities.Activities.Common;
public class PotScoreRadioModel
{
    public ICollection<PotScoreRadioRecord> Choices { get; set; } = new List<PotScoreRadioRecord>();
}
public record PotScoreRadioRecord(string Identifier, string Answer, PotScore PotScore);
public enum PotScore
{
    High,
    Medium,
    Low,
    VeryLow
}

