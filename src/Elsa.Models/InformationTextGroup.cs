namespace Elsa.CustomModels
{
    public class InformationTextGroup
    {
        public string? Title { get; set; }
        public bool Collapsed { get; set; } = false;
        public bool IsBullets { get; set; } = false;
        public bool IsGuidance { get; set; } = false;
        public List<InformationText> Text { get; set; } = new List<InformationText>();
    }
}
