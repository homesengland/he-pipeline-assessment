using He.PipelineAssessment.Data.SinglePipeline;
using System.Text.Json;

namespace Elsa.Server.Features.Datasources.GetSinglePipelineData
{
    public interface IEsriSinglePipelineDataJsonHelper
    {
        SinglePipelineData? JsonToSinglePipelineData(string data);
    }

    public class EsriSinglePipelineDataJsonHelper : IEsriSinglePipelineDataJsonHelper
    {
        public SinglePipelineData? JsonToSinglePipelineData(string data)
        {
            var result = JsonSerializer.Deserialize<EsriSinglePipelineResponse>(data);
            if (result != null && result.features.FirstOrDefault() != null)
            {
                var dataResult = result.features.FirstOrDefault()!.attributes;
                return dataResult;
            }

            return null;
        }
    }
}
