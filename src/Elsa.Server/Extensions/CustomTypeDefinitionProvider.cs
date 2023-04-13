using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Services;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Data.VFM;

namespace Elsa.Server.Extensions
{
    public class CustomTypeDefinitionProvider : TypeDefinitionProvider
    {
        public override IEnumerable<Type> CollectTypes(TypeDefinitionContext context) => new[]
        {
            typeof(SinglePipelineData),
            typeof(LaHouseNeedData),
            typeof(PCSProfileData),
            typeof(VFMCalculationData),
            typeof(Question)
        };
    }
}

