using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.ParseModels;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Handlers
{
    public class RadioExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;
        public string Syntax => CustomSyntaxNames.RadioList;
        

        public RadioExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)     
        {
            var evaluator = context.GetService<IExpressionEvaluator>();
            RadioModel result = new RadioModel();
            var checkboxProperties = TryDeserializeExpression(expression);
            var checkboxRecords = await ElsaPropertiesToRadioRecordList(checkboxProperties, evaluator, context);
            result.Choices = checkboxRecords;
            return result;
        }

        public async Task<List<RadioRecord>> ElsaPropertiesToRadioRecordList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            RadioRecord[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToRadioRecord(x, evaluator, context)));
            return resultArray.ToList();
        }

        private async Task<RadioRecord> ElsaPropertyToRadioRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string identifier = property.Name;
            string value = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
            bool isPrePopulated = await EvaluatePrePopulated(property, evaluator, context);
            return new RadioRecord(identifier, value, isPrePopulated);
        }

        public async Task<bool> EvaluatePrePopulated(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(RadioSyntaxNames.PrePopulated))
            {
                string expression = property.Expressions[RadioSyntaxNames.PrePopulated] ?? "false";
                bool isSingle = await property.EvaluateFromExpressionsExplicit<bool>(evaluator, 
                    context, _logger, 
                    expression.ToLower(), 
                    SyntaxNames.JavaScript, 
                    CancellationToken.None);
                return isSingle;
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
