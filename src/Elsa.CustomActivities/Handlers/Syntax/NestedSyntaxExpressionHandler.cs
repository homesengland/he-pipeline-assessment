using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.ParseModels;
using Elsa.CustomActivities.Resolver;
using Elsa.Expressions;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Elsa.CustomActivities.Handlers.Syntax
{


    public interface INestedSyntaxExpressionHandler
    {
        Type GetReturnType(string typeHint);
        //Task<T> EvaluateFromExpressions<T>(IExpressionEvaluator evaluator, ActivityExecutionContext context, ElsaProperty property, CancellationToken cancellationToken);
        Task<object?> EvaluateModel(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context, Type propertyType);
    }

    public class NestedSyntaxExpressionHandler : INestedSyntaxExpressionHandler
    {
        private ILogger<NestedSyntaxExpressionHandler> _logger;
        public NestedSyntaxExpressionHandler(ILogger<NestedSyntaxExpressionHandler> logger)
        {
            _logger = logger;
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
                    List<CheckboxRecord> records = await ElsaPropertiesToCheckboxRecordList(parsedProperties, evaluator, context);
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
                    List<RadioRecord> records = await ElsaPropertiesToRadioRecordList(parsedProperties, evaluator, context);
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

        private async Task<T> EvaluateFromExpressions<T>(IExpressionEvaluator evaluator, ActivityExecutionContext context, ElsaProperty property, CancellationToken cancellationToken = default)
        {
            try
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
            catch(KeyNotFoundException e)
            {
                _logger.LogError(e, "Incorrect data structure.  Expression did not contain correct Syntax");
                return default!;
            }
            
        }

        private List<ElsaProperty> ParseToCheckboxModel(ElsaProperty property)
        {
            if(property.Expressions != null)
            {
                var checkboxJsonList = JsonConvert.DeserializeObject<List<ElsaProperty>>(property.Expressions[SyntaxNames.Json]);
                return checkboxJsonList ?? new List<ElsaProperty>();
            }
            return new List<ElsaProperty>();
        }

        private List<ElsaProperty> ParseToRadioModel(ElsaProperty property)
        {
            if(property.Expressions != null)
            {
                var radioJson = JsonConvert.DeserializeObject<List<ElsaProperty>>(property.Expressions[SyntaxNames.Json]);
                return radioJson ?? new List<ElsaProperty>();
            }
            return new List<ElsaProperty>();

        }

        private async Task<List<CheckboxRecord>> ElsaPropertiesToCheckboxRecordList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            CheckboxRecord[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToCheckboxRecord(x, evaluator, context)));
            return resultArray.ToList();
        }

        private async Task<List<RadioRecord>> ElsaPropertiesToRadioRecordList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            RadioRecord[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToRadioRecord(x, evaluator, context)));
            return resultArray.ToList();
        }

        private async Task<CheckboxRecord> ElsaPropertyToCheckboxRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string identifier = property.Name;
            string value = await EvaluateFromExpressions<string>(evaluator, context, property, CancellationToken.None);
            bool isSingle = property.Expressions?[CustomSyntaxNames.Checked].ToLower() == "true";
            return new CheckboxRecord(identifier, value, isSingle);
        }

        private async Task<RadioRecord> ElsaPropertyToRadioRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            var identifier = property.Name;
            var value = await EvaluateFromExpressions<string>(evaluator, context, property, CancellationToken.None);
            return new RadioRecord(identifier, value);
        }

        public Type GetReturnType(string typeHint)
        {
            return OutputTypeHintResolver.GetTypeFromTypeHint(typeHint);
        }
    }
}