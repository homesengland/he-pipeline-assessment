using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Services;
using He.PipelineAssessment.Data.SinglePipeline;

namespace Elsa.Server.Extensions
{
    public class CustomTypeDefinitionProvider : TypeDefinitionProvider
    {
        public override IEnumerable<Type> CollectTypes(TypeDefinitionContext context) => new[]
        {
            typeof(SinglePipelineData),
            typeof(QuestionScreenQuestion)

        };
    }
}

