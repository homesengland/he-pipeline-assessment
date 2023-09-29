using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.Data.RegionalIPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.RegionalFigs
{
    public interface IEsriRegionalFigsJsonHelper
    {
        RegionalFigsData? JsonToRegionalFigsData(string data);
    }

    public class EsriRegionalFigsJsonHelper : IEsriRegionalFigsJsonHelper
    {
        public RegionalFigsData? JsonToRegionalFigsData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriRegionalFigsResponse>(data);
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
