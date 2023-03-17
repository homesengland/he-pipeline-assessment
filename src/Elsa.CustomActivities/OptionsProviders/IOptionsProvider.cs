using Newtonsoft.Json;

namespace Elsa.CustomActivities.OptionsProviders
{
    public interface IOptionsProvider
    {
        public string GetOptions();
    }

    public class PotScoreOptionsProvider : IOptionsProvider
    {
        public string GetOptions() => 
            JsonConvert.SerializeObject(new List<string> { "High", "Medium", "Low", "Very Low" });
    }
}
