using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Handlers
{
    public class CheckboxExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;
        public string Syntax => CustomSyntaxNames.CheckboxList;


        public CheckboxExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)
        {
            var evaluator = context.GetService<IExpressionEvaluator>();
            CheckboxModel result = new CheckboxModel();
            var checkboxProperties = TryDeserializeExpression(expression);
            var checkboxRecords = await ElsaPropertiesToCheckboxRecordList(checkboxProperties, evaluator, context);
            result.Choices = checkboxRecords;
            return result;
        }

        public async Task<List<CheckboxRecord>> ElsaPropertiesToCheckboxRecordList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            CheckboxRecord[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToCheckboxRecord(x, evaluator, context)));
            return resultArray.ToList();
        }

        private async Task<CheckboxRecord> ElsaPropertyToCheckboxRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string identifier = property.Name;
            string value = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
            bool isSingle = EvaluateIsSingle(property, evaluator, context);
            bool isPrePopulated = await EvaluatePrePopulated(property, evaluator, context);
            bool isExclusiveToQuestion = EvaluateExclusiveToQuestion(property, evaluator, context);
            return new CheckboxRecord(identifier, value, isSingle, isPrePopulated, isExclusiveToQuestion);
        }

        public bool EvaluateIsSingle(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(CheckboxSyntaxNames.Single))
            {
                bool isSingle = property.Expressions?[CheckboxSyntaxNames.Single].ToLower() == "true";
                return isSingle;
            }
            return false;
        }

        public bool EvaluateExclusiveToQuestion(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(CheckboxSyntaxNames.Single))
            {
                bool isExclusiveToQuestion = property.Expressions?[CheckboxSyntaxNames.Single].ToLower() == "true";
                return isExclusiveToQuestion;
            }
            return false;
        }

        public async Task<bool> EvaluatePrePopulated(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(CheckboxSyntaxNames.PrePopulated))
            {
                string expression = property.Expressions[CheckboxSyntaxNames.PrePopulated] ?? "false";
                bool prePopulated = await property.EvaluateFromExpressionsExplicit<bool>(evaluator,
                    context, _logger,
                    expression,
                    SyntaxNames.JavaScript,
                    CancellationToken.None);
                return prePopulated;
            }
            return false;
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
