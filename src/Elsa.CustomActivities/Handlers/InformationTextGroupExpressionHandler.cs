using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Handlers
{
    public class InformationTextGroupExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;
        private readonly InformationTextExpressionHandler _textHandler;
        public string Syntax => TextActivitySyntaxNames.TextGroup;


        public InformationTextGroupExpressionHandler(InformationTextExpressionHandler handler,ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _textHandler = handler;
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)
        {
            var evaluator = context.GetService<IExpressionEvaluator>();
            List<TextGroup> result = new List<TextGroup>();
            var textGroupProperties = TryDeserializeExpression(expression);
            var textGroups = await ElsaPropertiesToGroupedTextList(textGroupProperties, evaluator, context);
            result = textGroups;
            return result;
        }

        public async Task<List<TextGroup>> ElsaPropertiesToGroupedTextList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            TextGroup?[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToTextGroup(x, evaluator, context)));
            return resultArray.Where(x => x != null).ToList()!;
        }

        private async Task<TextGroup?> ElsaPropertyToTextGroup(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string value = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
            //string value = await EvaluateTextFromExpressions<string>(evaluator, context, property, CancellationToken.None);
            var condition = await EvaluateCondition(property, evaluator, context);
            if (!condition)
            {
                return null;
            }
            string? title = EvaluateString(property, TextActivitySyntaxNames.Title);
            bool isGuidance = EvaluateBoolean(property, TextActivitySyntaxNames.Guidance);
            bool isCollapsed = EvaluateBoolean(property, TextActivitySyntaxNames.Collapsed);
            bool isBullets = EvaluateBoolean(property, TextActivitySyntaxNames.Bulletpoint);
            TextModel records = await EvaluateTextRecords(property, context);

            return new TextGroup
            {
                Title = title,
                Guidance = isGuidance,
                Collapsed = isCollapsed,
                Bullets = isBullets,
                TextRecords = records.TextRecords.ToList()
            };
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

        public async Task<TextModel> EvaluateTextRecords(ElsaProperty property, ActivityExecutionContext context)
        {
            try
            {
                if (property.Expressions!.ContainsKey(TextActivitySyntaxNames.TextActivity))
                {
                    var result = await _textHandler.EvaluateAsync(property.Expressions[TextActivitySyntaxNames.TextActivity], typeof(TextModel), context, CancellationToken.None);
                    if (result != null)
                    {
                        TextModel? modelResult = result as TextModel;
                        return modelResult != null ? modelResult : new TextModel();
                    }
                    return new TextModel();
                }
                else
                {
                    return new TextModel();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("An exception was thrown whilst attempting to parse Text Activity", ex);
                return new TextModel();
            }

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
