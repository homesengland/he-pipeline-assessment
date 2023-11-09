
using He.PipelineAssessment.Data.VoaLandValues.Models.Office;
using System.Text.Json;

namespace He.PipelineAssessment.Data.VoaLandValues.Office
{
    public interface IOfficeLandValuesDataJsonHelper
    {
        OfficeLandValues? JsonToOfficeLandValuesData(string data);
    }

    public class OfficeLandValuesDataJsonHelper : IOfficeLandValuesDataJsonHelper
    {

        public OfficeLandValues? JsonToOfficeLandValuesData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<OfficeLandValuesResponse>(data);
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