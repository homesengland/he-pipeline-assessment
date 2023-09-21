namespace Elsa.CustomModels
{
    public class InformationTextGroup
    {
        public string? Title { get; set; }
        public bool IsCollapsed { get; set; } = false;
        public bool IsBullets { get; set; } = false;
        public bool IsGuidance { get; set; } = false;
        public List<InformationText> InformationTextList { get; set; } = new List<InformationText>();
    }
}
