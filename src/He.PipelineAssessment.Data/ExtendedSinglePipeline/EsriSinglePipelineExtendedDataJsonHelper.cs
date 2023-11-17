using System.Text.Json;
using System.Text.Json.Serialization;

namespace He.PipelineAssessment.Data.ExtendedSinglePipeline
{
    public interface IEsriSinglePipelineExtendedDataJsonHelper
    {
        SinglePipelineExtendedData? JsonToSinglePipelineData(string data);
        SinglePipelineExtendedDataList? JsonToSinglePipelineDataList(string data);
    }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public class EsriSinglePipelineExtendedDataJsonHelper : IEsriSinglePipelineExtendedDataJsonHelper
    {
        public SinglePipelineExtendedData? JsonToSinglePipelineData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriSinglePipelineExtendedResponse>(data);
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

        public SinglePipelineExtendedDataList? JsonToSinglePipelineDataList(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriSinglePipelineExtendedResponse>(data);
                if (result != null && result.features.Any())
                {
                    List<SinglePipelineExtendedData> dataResult = result.features.Select(x => x.attributes).ToList();
                    return new SinglePipelineExtendedDataList
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
