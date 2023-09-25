using He.PipelineAssessment.Data.PCSProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.RegionalIPU
{
    public interface IEsriRegionalIPUJsonHelper
    {
        RegionalIPUData? JsonToRegionalIPUData(string data);
    }

    public class EsriRegionalIPUJsonHelper : IEsriRegionalIPUJsonHelper
    {
        public RegionalIPUData? JsonToRegionalIPUData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<EsriRegionalIPUResponse>(data);
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
