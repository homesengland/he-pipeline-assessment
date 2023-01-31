using Elsa.CustomActivities.Constants;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using System.Runtime.CompilerServices;


namespace Elsa.CustomActivities.Handlers
{
    public class ConditionalTextListExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        public string Syntax => CustomSyntaxNames.ConditionalTextList;

        public ConditionalTextListExpressionHandler(IContentSerializer contentSerializer)
        {
            _contentSerializer = contentSerializer;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)     
        {
            var caseModels = TryDeserializeExpression(expression);
            var evaluatedCases = await EvaluateCasesAsync(caseModels, context, cancellationToken).ToListAsync(cancellationToken);
            return evaluatedCases;
        }

        private async IAsyncEnumerable<string> EvaluateCasesAsync(IEnumerable<ConditionalTextModel> caseModels, ActivityExecutionContext context, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var validCaseModels = ValidCaseModels(caseModels);
            var evaluator = context.GetService<IExpressionEvaluator>();

            foreach (var caseModel in validCaseModels)
            {
                var text = await EvaluateText(evaluator, context, caseModel, cancellationToken);
                var condition = await EvaluateCondition(evaluator, context, caseModel, cancellationToken);
                if (condition)
                {
                    yield return text;
                }
            }
        }

        private async Task<string> EvaluateText(IExpressionEvaluator evaluator, ActivityExecutionContext context, ConditionalTextModel caseModel, CancellationToken cancellationToken)
        {
            var syntax = caseModel.Text.Syntax!;
            var expression = caseModel.Text.Expressions![syntax];
            var result = await evaluator.TryEvaluateAsync<string>(expression, syntax, context, cancellationToken);
            return result.Value ?? "";
        }

        private async Task<bool> EvaluateCondition(IExpressionEvaluator evaluator, ActivityExecutionContext context, ConditionalTextModel caseModel, CancellationToken cancellationToken)
        {
            var syntax = caseModel.Condition.Syntax!;
            var expression = caseModel.Condition.Expressions![syntax];
            var result = await evaluator.TryEvaluateAsync<bool>(expression, syntax, context, cancellationToken);
            var caseResult = result.Success && result.Value;
            return caseResult;
        }

        private IEnumerable<ConditionalTextModel> ValidCaseModels(IEnumerable<ConditionalTextModel> caseModels)
        {
            List<ConditionalTextModel> validCaseModels = new List<ConditionalTextModel>();
            validCaseModels = caseModels.Where(x => x.Text.Expressions != null && !string.IsNullOrWhiteSpace(x.Text.Syntax) && x.Text.Expressions.ContainsKey(x.Text.Syntax)).ToList();
            var filteredValidCaseModels = validCaseModels.Where(x => x.Condition.Expressions != null && !string.IsNullOrWhiteSpace(x.Condition.Syntax) && x.Condition.Expressions.ContainsKey(x.Condition.Syntax)).ToList();
            return filteredValidCaseModels;
        }

        private IList<ConditionalTextModel> TryDeserializeExpression(string expression)
        {
            try
            {
                return _contentSerializer.Deserialize<IList<ConditionalTextModel>>(expression);
            }
            catch
            {
                return new List<ConditionalTextModel>();
            }
        }


        public record ConditionalTextModel
        {
            public ConditionalTextElement Text { get; set; } = null!;

            public ConditionalTextElement Condition { get; set; } = null!;
        }

        public record ConditionalTextElement(IDictionary<string, string>? Expressions, string? Syntax);
    }
}
