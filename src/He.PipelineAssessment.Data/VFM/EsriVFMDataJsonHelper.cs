using System.Text.Json;

namespace He.PipelineAssessment.Data.VFM
{
    public interface IEsriVFMDataJsonHelper
    {
        VFMCalculationData? JsonToVFMCalculationData(string data);

    }

    public class EsriVFMDataJsonHelper : IEsriVFMDataJsonHelper
    {
        public VFMCalculationData? JsonToVFMCalculationData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriVFMResponse>(data);
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
