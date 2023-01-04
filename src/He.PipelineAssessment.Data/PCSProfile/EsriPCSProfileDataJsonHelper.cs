using System.Text.Json;

namespace He.PipelineAssessment.Data.PCSProfile
{
    public interface IEsriPCSProfileDataJsonHelper
    {
        PCSProfileData? JsonToPCSProfileData(string data);
    }

    public class EsriPCSProfileDataJsonHelper : IEsriPCSProfileDataJsonHelper
    {
        public PCSProfileData? JsonToPCSProfileData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriPCSProfileResponse>(data);
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
