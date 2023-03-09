namespace Elsa.CustomActivities.Activities.Common
{
    public class TextModel
    {
        public ICollection<TextRecord> TextRecords { get; set; } = new List<TextRecord>();

        public record TextRecord(string Text, bool IsParagraph, bool IsGuidance, bool IsHyperlink, string? Url);
    }
}
