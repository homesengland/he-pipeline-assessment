using He.PipelineAssessment.Data.SinglePipeline;
using System.Text.Json;

namespace He.PipelineAssessment.Data.LaHouseNeed
{
    public interface IEsriLaHouseNeedDataJsonHelper
    {
        List<LaHouseNeedData>? JsonToLAHouseNeedData(string data);
    }

    public class EsriLAHouseNeedDataJsonHelper : IEsriLaHouseNeedDataJsonHelper
    {
        public List<LaHouseNeedData>? JsonToLAHouseNeedData(string data)
        {
            try
            {
                var laHouseNeedDataList = new List<LaHouseNeedData>();
                var result = JsonSerializer.Deserialize<EsriLAHouseNeedResponse>(data);
                if (result != null)
                {

                    foreach (var feature in result.features)
                    {
                        laHouseNeedDataList.Add(feature.attributes);
                    }
                }
                
                return laHouseNeedDataList;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
