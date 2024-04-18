using He.PipelineAssessment.Data.VoaLandValues.Models.Office;
using System.Text.Json;

namespace He.PipelineAssessment.Data.VoaLandValues.Office
{
    public interface IOfficeLandValuesDataJsonHelper
    {
        OfficeLandValues? JsonToOfficeLandValuesData(string data);
        List<OfficeLandValues>? JsonToOfficeLandValuesDataList(string data);
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

        public List<OfficeLandValues>? JsonToOfficeLandValuesDataList(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<OfficeLandValuesResponse>(data);
                if (result != null && result.features.Any())
                {
                    List<OfficeLandValues> dataResult = result.features.Select(x => x.attributes).ToList();
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