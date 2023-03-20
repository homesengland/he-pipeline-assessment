namespace Elsa.CustomActivities.Activities.Common;
public class PotScoreRadioModel
{
    public ICollection<PotScoreRadioRecord> Choices { get; set; } = new List<PotScoreRadioRecord>();
}
public record PotScoreRadioRecord(string Identifier, string Answer, string PotScore, bool IsPrePopulated);
