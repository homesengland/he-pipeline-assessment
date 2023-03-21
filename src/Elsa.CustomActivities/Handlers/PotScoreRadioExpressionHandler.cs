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
    public class PotScoreRadioExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;
        public string Syntax => CustomSyntaxNames.PotScore;
        

        public PotScoreRadioExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)     
        {
            var evaluator = context.GetService<IExpressionEvaluator>();
            PotScoreRadioModel result = new PotScoreRadioModel();
            var checkboxProperties = TryDeserializeExpression(expression);
            var checkboxRecords = await ElsaPropertiesToPotScoreRadioRecordList(checkboxProperties, evaluator, context);
            result.Choices = checkboxRecords;
            return result;
        }

        public async Task<List<PotScoreRadioRecord>> ElsaPropertiesToPotScoreRadioRecordList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            PotScoreRadioRecord[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToPotScoreRadioRecord(x, evaluator, context)));
            return resultArray.ToList();
        }

        private async Task<PotScoreRadioRecord> ElsaPropertyToPotScoreRadioRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string identifier = property.Name;
            string value = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
            string potScore =  EvaluatePotScore(property);
            bool isPrePopulated = await EvaluatePrePopulated(property, evaluator, context);
            return new PotScoreRadioRecord(identifier, value, potScore, isPrePopulated);
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

        public string EvaluatePotScore(ElsaProperty property)
        {
            if (property.Expressions!.ContainsKey(CustomSyntaxNames.PotScore))
            {
                return property.Expressions![CustomSyntaxNames.PotScore];
            }

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
