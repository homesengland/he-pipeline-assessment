using He.PipelineAssessment.Data.BIL;
using He.PipelineAssessment.Data.BIL.RegionalFigures;
using He.PipelineAssessment.Data.BIL.VOAAgriculturalLandValues;
using He.PipelineAssessment.Data.BIL.VOALandValues;
using He.PipelineAssessment.Data.BIL.VOAOfficeLandValues;
using System.Text.Json;

namespace He.PipelineAssessment.Data.PCSProfile
{
    public interface IEsriBILDataJsonHelper
    {
        BilRegionalFigures? JsonToRegionalFigures(string data);
        VoaLandValues? JsonToVoaLandValuesData(string data);
        VoaOfficeLandValues? JsonToVoaOfficeLandValuesData(string data);
        VoaAgricultureLandValues? JsonToVoaAgricultureLandValuesData(string data);
    }

    public class EsriBILDataJsonHelper : IEsriBILDataJsonHelper
    {
        public BilRegionalFigures? JsonToRegionalFigures(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<BilRegionalFiguresResponse>(data);
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

        public VoaAgricultureLandValues? JsonToVoaAgricultureLandValuesData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<VoaAgricultureLandValuesResponse>(data);
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

        public VoaLandValues? JsonToVoaLandValuesData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<VoaLandValuesResponse>(data);
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

        public VoaOfficeLandValues? JsonToVoaOfficeLandValuesData(string data)
        {
            try
            {
                var result = JsonSerializer.Deserialize<VoaOfficeLandValuesResponse>(data);
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