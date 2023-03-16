using Newtonsoft.Json;

namespace Elsa.CustomActivities.OptionsProviders
{
    public interface IOptionsProvider
    {
        public string GetOptions();
    }

    public class PotScoreOptionsProvider : IOptionsProvider
    {
        public string GetOptions()
        {
            var potScoreOptions = new List<string> { "High", "Medium", "Low", "Very Low" };

            return JsonConvert.SerializeObject(potScoreOptions);
        }
    }

}
