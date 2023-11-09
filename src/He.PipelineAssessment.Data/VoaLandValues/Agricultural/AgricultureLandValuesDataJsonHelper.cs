using He.PipelineAssessment.Data.VoaLandValues.Models.Agriculture;
using System.Text.Json;

namespace He.PipelineAssessment.Data.VoaLandValues.Agricultural
{
    public interface IAgricultureLandValuesDataJsonHelper
    {
        AgricultureLandValues? JsonToVoaAgricultureLandValuesData(string data);
    }

    public class AgricultureLandValuesDataJsonHelper : IAgricultureLandValuesDataJsonHelper
    {

        public AgricultureLandValues? JsonToVoaAgricultureLandValuesData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<AgricultureLandValuesResponse>(data);
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