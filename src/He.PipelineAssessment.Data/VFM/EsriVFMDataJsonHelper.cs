using He.PipelineAssessment.Data.SinglePipeline;
using System.Text.Json;

namespace He.PipelineAssessment.Data.VFM
{
    public interface IEsriVFMDataJsonHelper
    {
        VFMCalculationData? JsonToVFMCalculationData(string data);
        List<VFMCalculationData>? JsonToVFMCalculationDataList(string data);
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

        public List<VFMCalculationData>? JsonToVFMCalculationDataList(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriVFMResponse>(data);
                if (result != null && result.features.Any())
                {
                    List<VFMCalculationData> dataResult = result.features.Select(x => x.attributes).ToList();
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
