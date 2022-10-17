using He.PipelineAssessment.Data.SinglePipeline;
using System.Text.Json;

namespace Elsa.Server.Features.Datasources.GetSinglePipelineData
{
    public interface IGetSinglePipelineDataJsonHelper
    {
        SinglePipelineData JsonToSinglePipelineData(string data);
    }

    public class GetSinglePipelineDataJsonHelper : IGetSinglePipelineDataJsonHelper
    {
        public SinglePipelineData JsonToSinglePipelineData(string data)
        {
            var result = JsonSerializer.Deserialize<EsriSinglePipelineResponse>(data);
            var dataResult = result.features.FirstOrDefault().attributes;
            return dataResult;
        }
    }
}
