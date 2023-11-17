using System.Text.Json;
using System.Text.Json.Serialization;

namespace He.PipelineAssessment.Data.ExtendedSinglePipeline
{
    public interface IEsriExtendedSinglePipelineDataJsonHelper
    {
        ExtendedSinglePipelineData? JsonToSinglePipelineData(string data);
        ExtendedSinglePipelineDataList? JsonToSinglePipelineDataList(string data);
    }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public class EsriExtendedSinglePipelineDataJsonHelper : IEsriExtendedSinglePipelineDataJsonHelper
    {
        public ExtendedSinglePipelineData? JsonToSinglePipelineData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriExtendedSinglePipelineResponse>(data);
                if (result != null && result.features.FirstOrDefault() != null)
                {
                    
                    var dataResult = result.features.FirstOrDefault()!.attributes;
                    return dataResult;
                }
            }
            catch (Exception)
            {

                return null;
            }

            return null;
        }

        public ExtendedSinglePipelineDataList? JsonToSinglePipelineDataList(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriExtendedSinglePipelineResponse>(data);
                if (result != null && result.features.Any())
                {
                    List<ExtendedSinglePipelineData> dataResult = result.features.Select(x => x.attributes).ToList();
                    return new ExtendedSinglePipelineDataList
                    {
                        SinglePipelineData = dataResult,
                        ExceededTransferLimit = result.exceededTransferLimit
                    };
                }
            }
            catch (Exception)
            {

                return null;
            }

            return null;
        }
    }
}
