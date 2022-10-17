using Elsa.Activities.Workflows;
using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Services;

namespace Elsa.CustomActivities.Services
{
    public class CustomTypeDefinitionProvider : TypeDefinitionProvider
    {
        public override ValueTask<IEnumerable<Type>> CollectTypesAsync(TypeDefinitionContext context, CancellationToken cancellationToken = default)
        {
            var types = new[] { typeof(AssessmentQuestion), typeof(FinishedWorkflowModel) };
            return new ValueTask<IEnumerable<Type>>(types);
        }
    }
}