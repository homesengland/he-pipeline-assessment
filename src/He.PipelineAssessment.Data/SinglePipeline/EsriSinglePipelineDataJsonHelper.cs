using System.Text.Json;
using System.Text.Json.Serialization;

namespace He.PipelineAssessment.Data.SinglePipeline
{
    public interface IEsriSinglePipelineDataJsonHelper
    {
        SinglePipelineData? JsonToSinglePipelineData(string data);
        SinglePipelineDataList? JsonToSinglePipelineDataList(string data);
    }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public class EsriSinglePipelineDataJsonHelper : IEsriSinglePipelineDataJsonHelper
    {
        public SinglePipelineData? JsonToSinglePipelineData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriSinglePipelineResponse>(data);
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

        public SinglePipelineDataList? JsonToSinglePipelineDataList(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriSinglePipelineResponse>(data);
                if (result != null && result.features.Any())
                {
                    List<SinglePipelineData> dataResult = result.features.Select(x => x.attributes).ToList();
                    return new SinglePipelineDataList
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
