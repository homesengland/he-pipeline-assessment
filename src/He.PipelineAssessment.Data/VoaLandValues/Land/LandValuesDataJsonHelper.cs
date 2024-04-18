using He.PipelineAssessment.Data.VoaLandValues.Models.Land;
using System.Text.Json;

namespace He.PipelineAssessment.Data.VoaLandValues.Land
{
    public interface ILandValuesDataJsonHelper
    {
        LandValues? JsonToLandValuesData(string data);
    }
    public class LandValuesDataJsonHelper : ILandValuesDataJsonHelper
    {
        public LandValues? JsonToLandValuesData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<LandValuesResponse>(data);
                if (result != null)
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