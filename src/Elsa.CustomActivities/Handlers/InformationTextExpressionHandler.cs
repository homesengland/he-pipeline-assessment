using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.ParseModels;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Jint.Native.Boolean;
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
            string value = await EvaluateTextFromExpressions<string>(evaluator, context, property, CancellationToken.None);
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

        public async Task<T> EvaluateTextFromExpressions<T>(IExpressionEvaluator evaluator, ActivityExecutionContext context, ElsaProperty property, CancellationToken cancellationToken = default)
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

        public async Task<bool> EvaluateCondition(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(CustomSyntaxNames.Condition))
            {
                var result = await evaluator.TryEvaluateAsync<bool>(property.Expressions?[CustomSyntaxNames.Condition], SyntaxNames.JavaScript, context);
                return result.Value;
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
