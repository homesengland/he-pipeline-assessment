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
            bool isParagraph = EvaluateParagraph(property);
            bool isGuidance = EvaluateGuidance(property);
            bool isHyperlink = EvaluateHyperlink(property);
            string? url = EvaluateUrl(property);

            return new TextRecord(value, isParagraph, isGuidance, isHyperlink, url);
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

        public bool EvaluateParagraph(ElsaProperty property)
        {
            if (property.Expressions!.ContainsKey(TextActivitySyntaxNames.Paragraph))
            {
                bool value = property.Expressions?[TextActivitySyntaxNames.Paragraph].ToLower() == "true";
                return value;
            };
            return false;
        }

        public bool EvaluateGuidance(ElsaProperty property)
        {
            if (property.Expressions!.ContainsKey(TextActivitySyntaxNames.Guidance))
            {
                bool value = property.Expressions?[TextActivitySyntaxNames.Guidance].ToLower() == "true";
                return value;
            };
            return false;
        }

        public bool EvaluateHyperlink(ElsaProperty property)
        {
            if (property.Expressions!.ContainsKey(TextActivitySyntaxNames.Hyperlink))
            {
                bool value = property.Expressions?[TextActivitySyntaxNames.Hyperlink].ToLower() == "true";
                return value;
            };
            return false;
        }

        public string? EvaluateUrl(ElsaProperty property)
        {
            if (property.Expressions!.ContainsKey(TextActivitySyntaxNames.Url))
            {
                string? value = property.Expressions?[TextActivitySyntaxNames.Url];
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
