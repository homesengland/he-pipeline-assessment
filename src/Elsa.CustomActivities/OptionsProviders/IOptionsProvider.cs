
using Newtonsoft.Json;

namespace Elsa.CustomActivities.OptionsProviders
{
    public interface IOptionsProvider
    {
        public string GetOptions();
    }

    public class JsonOptionsProvider : IOptionsProvider
    {
        public string _jsonMetadata = "";
        public string _jsonData = "";

        public JsonOptionsProvider(object? metaData, object? data)
        {
            _jsonMetadata = metaData != null ? JsonConvert.SerializeObject(metaData) : "";
            _jsonData = data != null ? JsonConvert.SerializeObject(data) : "";
        }

        //public IOptions GetOptions()
        //{
        //    return new JsonOptions
        //    {
        //        Data = _jsonData,
        //        Metadata = _jsonMetadata,
        //    };
        //}
        public string GetOptions()
        {
            throw new NotImplementedException();
        }
    }

    public class PotScoreOptionsProvider : IOptionsProvider
    {
        public string GetOptions()
        {
            return "High, Medium, Low, Very Low";
        }
    }

}
