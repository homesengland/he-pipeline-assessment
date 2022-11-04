using System.Text.Json;

namespace He.PipelineAssessment.Data.SinglePipeline
{
    public interface IEsriSinglePipelineDataJsonHelper
    {
        SinglePipelineData? JsonToSinglePipelineData(string data);
        List<SinglePipelineData>? JsonToSinglePipelineDataList(string data);
    }

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

        public List<SinglePipelineData>? JsonToSinglePipelineDataList(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriSinglePipelineResponse>(data);
                if (result != null && result.features.Any())
                {
                    List<SinglePipelineData> dataResult = result.features.Select(x => x.attributes).ToList();
                    return dataResult;
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
