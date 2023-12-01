using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Handlers
{
    public class InformationTextExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;
        public string Syntax => TextActivitySyntaxNames.TextActivity;


        public InformationTextExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)
        {
            var evaluator = context.GetService<IExpressionEvaluator>();
            TextModel result = new TextModel();
            var informatinTextProperties = TryDeserializeExpression(expression);
            var textRecords = await ElsaPropertiesToTextRecordList(informatinTextProperties, evaluator, context);
            result.TextRecords = textRecords;
            return result;
        }

        public async Task<List<TextRecord>> ElsaPropertiesToTextRecordList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            TextRecord?[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToTextRecord(x, evaluator, context)));
            return resultArray.Where(x => x != null).ToList()!;
        }

        private async Task<TextRecord?> ElsaPropertyToTextRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string value = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
            //string value = await EvaluateTextFromExpressions<string>(evaluator, context, property, CancellationToken.None);
            var condition = await EvaluateCondition(property, evaluator, context);
            if (!condition)
            {
                return null;
            }
            bool isParagraph = EvaluateBoolean(property, TextActivitySyntaxNames.Paragraph);
            bool isGuidance = EvaluateBoolean(property, TextActivitySyntaxNames.Guidance);
            bool isHyperlink = EvaluateBoolean(property, TextActivitySyntaxNames.Hyperlink);
            bool isBold = EvaluateBoolean(property, TextActivitySyntaxNames.Bold);
            string? url = EvaluateString(property, TextActivitySyntaxNames.Url);

            return new TextRecord(value, isParagraph, isGuidance, isHyperlink, url, isBold);
        }

        public bool EvaluateBoolean(ElsaProperty property, string key)
        {
            if (property.Expressions!.ContainsKey(key))
            {
                bool value = property.Expressions?[key].ToLower() == "true";
                return value;
            };
            return false;
        }

        public async Task<bool> EvaluateCondition(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(CustomSyntaxNames.Condition))
            {
                string expression = property.Expressions[CustomSyntaxNames.Condition] ?? "false";
                bool conditionResult = await property.EvaluateFromExpressionsExplicit<bool>(evaluator,
                    context, _logger,
                    expression,
                    SyntaxNames.JavaScript,
                    CancellationToken.None);
                return conditionResult;
            }
            return true;
        }

        public string? EvaluateString(ElsaProperty property, string key)
        {
            if (property.Expressions!.ContainsKey(key))
            {
                string? value = property.Expressions?[key];
                return value;
            };
            return string.Empty;
        }

        private List<ElsaProperty> TryDeserializeExpression(string expression)
        {
            try
            {
                return _contentSerializer.Deserialize<List<ElsaProperty>>(expression);
            }
            catch
            {
                return new List<ElsaProperty>();
            }
        }




    }
}
