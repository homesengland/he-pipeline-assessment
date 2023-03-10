﻿namespace Elsa.CustomModels
{
    public class InformationText
    {
        public string Text { get; set; } = null!;
        public bool IsParagraph { get; set; } = true;
        public bool IsGuidance { get; set; } = false;
        public bool IsHyperlink { get; set; } = false;
        public string? Url { get; set; }
    }
}