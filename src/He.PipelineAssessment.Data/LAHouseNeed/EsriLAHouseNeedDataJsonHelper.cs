using He.PipelineAssessment.Data.SinglePipeline;
using System.Text.Json;

namespace He.PipelineAssessment.Data.LaHouseNeed
{
    public interface IEsriLaHouseNeedDataJsonHelper
    {
        LaHouseNeedData? JsonToLAHouseNeedData(string data);
    }

    public class EsriLAHouseNeedDataJsonHelper : IEsriLaHouseNeedDataJsonHelper
    {
        public LaHouseNeedData? JsonToLAHouseNeedData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriLAHouseNeedResponse>(data);
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
    }
}
