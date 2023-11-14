﻿namespace Elsa.CustomActivities.Activities.Common
{
    public class TextModel
    {
        public List<TextRecord> TextRecords { get; set; } = new List<TextRecord>();
    }

    public record TextRecord(string Text, bool IsParagraph, bool IsGuidance, bool IsHyperlink, string? Url, bool IsBold = false);

    public class TextGroup
    {
        public string? Title { get; set; }
        public bool Collapsed { get; set; } = false;
        public bool Bullets { get; set; } = false;
        public bool Guidance { get; set; } = false;
        public bool DisplayOnPage { get; set; } = true;
        public List<TextRecord> TextRecords { get; set; } = new List<TextRecord>();
    }

    public class GroupedTextModel
    {
        public List<TextGroup> TextGroups { get; set; } = new List<TextGroup>();
    }
}
