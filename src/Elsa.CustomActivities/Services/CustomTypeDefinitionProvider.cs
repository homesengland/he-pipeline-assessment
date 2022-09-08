using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Scripting.JavaScript.Services;

namespace MyActivityLibrary.JavaScript
{
    public class CustomTypeDefinitionProvider : TypeDefinitionProvider
    {
        public override ValueTask<IEnumerable<Type>> CollectTypesAsync(TypeDefinitionContext context, CancellationToken cancellationToken = default)
        {
            var types = new[] { typeof(AssessmentQuestion) };
            return new ValueTask<IEnumerable<Type>>(types);
        }
    }
}