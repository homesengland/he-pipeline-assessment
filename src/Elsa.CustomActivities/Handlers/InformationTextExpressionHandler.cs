using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.ParseModels;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Newtonsoft.Json;
using Polly;
using System.Runtime.CompilerServices;
using static Elsa.CustomActivities.Activities.Common.TextModel;

namespace Elsa.CustomActivities.Handlers
{
    public class InformationTextExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        public string Syntax => TextActivitySyntaxNames.TextActivity;

        public InformationTextExpressionHandler(IContentSerializer contentSerializer)
        {
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

        private async Task<List<TextRecord>> ElsaPropertiesToTextRecordList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            TextRecord?[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToTextRecord(x, evaluator, context)));
            return resultArray.Where(x => x != null).ToList()!;
        }

        private async Task<TextRecord?> ElsaPropertyToTextRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string value = await EvaluateFromExpressions<string>(evaluator, context, property, CancellationToken.None);
            var condition = await evaluator.EvaluateAsync<bool>(property.Expressions?[CustomSyntaxNames.Condition], SyntaxNames.JavaScript, context);
            if (!condition)
            {
                return null;
            }
            bool isParagraph = property.Expressions?[TextActivitySyntaxNames.Paragraph].ToLower() == "true";
            bool isGuidance = false;
            bool isHyperlink = false;
            string? url = string.Empty;

            if (property.Expressions!.ContainsKey(TextActivitySyntaxNames.Hyperlink))
            {
                isHyperlink = property.Expressions?[TextActivitySyntaxNames.Hyperlink].ToLower() == "true";
            };

            if (property.Expressions!.ContainsKey(TextActivitySyntaxNames.Guidance))
            {
                isGuidance = property.Expressions?[TextActivitySyntaxNames.Guidance].ToLower() == "true";
            };

            if (property.Expressions!.ContainsKey(TextActivitySyntaxNames.Url))
            {
                url = property.Expressions?[TextActivitySyntaxNames.Url];
            };

            return new TextRecord(value, isParagraph, isGuidance, isHyperlink, url);
        }

        public async Task<T> EvaluateFromExpressions<T>(IExpressionEvaluator evaluator, ActivityExecutionContext context, ElsaProperty property, CancellationToken cancellationToken = default)
        {
            var syntax = property.Syntax ?? SyntaxNames.Literal;
            if (property.Expressions != null && property.Expressions.Count > 0)
            {
                var expression = property.Expressions![syntax];
                var result = await evaluator.TryEvaluateAsync<T>(expression, syntax, context, cancellationToken);
                if (result.Value != null)
                {
                    return result.Value;
                }
                return default!;
            }
            else return default!;
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
