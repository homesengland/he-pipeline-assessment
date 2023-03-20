namespace Elsa.CustomActivities.OptionsProviders
{
    public interface IOptions
    {
        public string Data { get; set; }
        public string Metadata { get; set; }
    }

    public class JsonOptions : IOptions
    {
        public string Data { get; set; } = null!;
        public string Metadata { get; set; } = null!;
    }

    public class PotScoreOptions : IOptions
    {
        public string Data { get; set; } = null!;
        public string? Metadata { get; set; }
    }
}
