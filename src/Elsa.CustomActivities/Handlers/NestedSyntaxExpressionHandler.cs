using Elsa.CustomActivities.Resolver;
using Elsa.Expressions;
using Elsa.Services.Models;
using static Elsa.CustomActivities.Handlers.ConditionalTextListExpressionHandler;

namespace Elsa.CustomActivities.Handlers
{
    public record IElsaProperty(IDictionary<string, string>? Expressions, string? Syntax, string Value, string Name);

    public interface INestedSyntaxExpressionHandler
    {
        Type GetReturnType(string typeHint);
        Task<T> EvaluateFromExpressions<T>(IExpressionEvaluator evaluator, ActivityExecutionContext context, IElsaProperty property, CancellationToken cancellationToken);

    }

    public class NestedSyntaxExpressionHandler : INestedSyntaxExpressionHandler
    {
        
        public async Task<T> EvaluateFromExpressions<T>(IExpressionEvaluator evaluator, ActivityExecutionContext context, IElsaProperty property, CancellationToken cancellationToken = default)
        {
            var syntax = property.Syntax ?? SyntaxNames.Literal;
            if(property.Expressions != null && property.Expressions.Count > 0)
            {
                var expression = property.Expressions![syntax];
                var result = await evaluator.TryEvaluateAsync<T>(expression, syntax, context, cancellationToken);
                if (result.Value != null)
                {
                    return result.Value;
                }
                return default(T)!;
            }
            else return default(T)!;
        }

        public Type GetReturnType(string typeHint)
        {
            return OutputTypeHintResolver.GetTypeFromTypeHint(typeHint);
        }
    }
}