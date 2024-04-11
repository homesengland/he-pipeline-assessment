using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Services;
using He.PipelineAssessment.Data.Dataverse;
using He.PipelineAssessment.Data.ExtendedSinglePipeline;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.Data.RegionalFigs;
using He.PipelineAssessment.Data.RegionalIPU;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Data.VFM;
using He.PipelineAssessment.Data.VoaLandValues.Models.Agriculture;
using He.PipelineAssessment.Data.VoaLandValues.Models.Land;
using He.PipelineAssessment.Data.VoaLandValues.Models.Office;
using Npgsql.TypeMapping;

namespace Elsa.Server.Extensions
{
    public class CustomTypeDefinitionProvider : TypeDefinitionProvider
    {
        public override IEnumerable<Type> CollectTypes(TypeDefinitionContext context) => new[]
        {
            typeof(SinglePipelineData),
            typeof(LocalAuthority),
            typeof(LaHouseNeedData),
            typeof(PCSProfileData),
            typeof(VFMCalculationData),
            typeof(Question),
            typeof(SinglePipelineExtendedData),
            typeof(RegionalFigsData),
            typeof(RegionalIPUData),
            typeof(LandValues),
            typeof(AgricultureLandValues),
            typeof(OfficeLandValues),
            typeof(LocalAuthority),
            typeof(DataverseResults)
        };
    }
}

