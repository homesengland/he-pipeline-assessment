using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.ParseModels;
using Elsa.CustomActivities.Resolver;
using Elsa.Expressions;
using Elsa.Services.Models;
using Newtonsoft.Json;

namespace Elsa.CustomActivities.Handlers.Syntax
{


    public interface INestedSyntaxExpressionHandler
    {
        Type GetReturnType(string typeHint);
        Task<T> EvaluateFromExpressions<T>(IExpressionEvaluator evaluator, ActivityExecutionContext context, ElsaProperty property, CancellationToken cancellationToken);
        Task<object?> EvaluateModel(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context, Type propertyType);

    }

    public class NestedSyntaxExpressionHandler : INestedSyntaxExpressionHandler
    {

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

        public async Task<object?> EvaluateModel(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context, Type propertyType)
        {

            if (propertyType != null && propertyType == typeof(string))
            {
                string result = await EvaluateFromExpressions<string>(evaluator, context, property, CancellationToken.None);
                return result;
            }
            if (propertyType != null && propertyType == typeof(bool))
            {
                bool result = await EvaluateFromExpressions<bool>(evaluator, context, property, CancellationToken.None);
                return result;
            }
            if (propertyType != null && propertyType == typeof(int) || propertyType == typeof(int?))
            {
                int result = await EvaluateFromExpressions<int>(evaluator, context, property, CancellationToken.None);
                return result;
            }
            if (propertyType != null && propertyType == typeof(CheckboxModel))
            {
                CheckboxModel result = new CheckboxModel();
                var parsedProperties = ParseToCheckboxModel(property);
                if(parsedProperties != null)
                {
                    return ElsaPropertiesToCheckboxRecordList(parsedProperties);
                    List<CheckboxRecord> records = await ElsaPropertiesToCheckboxRecordList(parsedProperties);
                    result.Choices = records;
                }
                return result;
                
            }
            if (propertyType != null && propertyType == typeof(RadioModel))
            {
                RadioModel result = new RadioModel();
                var parsedProperties = ParseToRadioModel(property);
                if (parsedProperties != null)
                {
                    return ElsaPropertiesToCheckboxRecordList(parsedProperties);
                    List<RadioRecord> records = await ElsaPropertiesToCheckboxRecordList(parsedProperties);
                    result.Choices = records;
                }
                return result;

            }
            else
            {
                string result = await EvaluateFromExpressions<string>(evaluator, context, property, CancellationToken.None);
                return result;
            }
        }

        private List<ElsaProperty> ParseToCheckboxModel(ElsaProperty property)
        {
            var checkboxJsonList = JsonConvert.DeserializeObject<List<ElsaProperty>>(property.Expressions[SyntaxNames.Json]);
            return checkboxJsonList ?? new List<ElsaProperty>();
        }

        private List<ElsaProperty> ParseToRadioModel(ElsaProperty property)
        {
            var radioJson = JsonConvert.DeserializeObject<List<ElsaProperty>>(property.Expressions[SyntaxNames.Json]);
            return radioJson ?? new List<ElsaProperty>();
        }

        private async Task<List<CheckboxRecord>> ElsaPropertiesToCheckboxRecordList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            List<CheckboxRecord> result = new List<CheckboxRecord>();
            foreach(ElsaProperty property in properties)
            {
                result.Add(ElsaPropertyToCheckboxRecord(property, evaluator, context));
            }
            return result;
        }

        private async Task<List<RadioRecord>> ElsaPropertiesToRadioRecordList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            List<RadioRecord> result = new List<RadioRecord>();
            foreach(ElsaProperty property in properties)
            {
                result.Add(ElsaPropertyToRadioRecord(property, evaluator, context));
            }
            return result;
        }

        private CheckboxRecord ElsaPropertyToCheckboxRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            property = property;
            return new CheckboxRecord();
        }

        private RadioRecord ElsaPropertyToRadioRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            property = property;
            return new RadioRecord();
        }

        public Type GetReturnType(string typeHint)
        {
            return OutputTypeHintResolver.GetTypeFromTypeHint(typeHint);
        }
    }
}