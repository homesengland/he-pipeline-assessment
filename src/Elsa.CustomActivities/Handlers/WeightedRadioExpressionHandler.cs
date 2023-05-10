using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Elsa.CustomActivities.Handlers
{
    public class WeightedRadioExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;
        public string Syntax => CustomSyntaxNames.RadioList;


        public WeightedRadioExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)
        {
            var evaluator = context.GetService<IExpressionEvaluator>();
            WeightedRadioModel result = new WeightedRadioModel();
            var checkboxProperties = TryDeserializeExpression(expression);
            var checkboxRecords = await ElsaPropertiesToWeightedRadioRecordList(checkboxProperties, evaluator, context);
            result.Choices = checkboxRecords;
            return result;
        }

        public async Task<List<WeightedRadioRecord>> ElsaPropertiesToWeightedRadioRecordList(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            WeightedRadioRecord[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToWeightedRadioRecord(x, evaluator, context)));
            return resultArray.ToList();
        }

        public async Task<List<WeightedRadioRecord>> EvaluateRadioRecords(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(SyntaxNames.Json))
            {
                var elsaCheckboxProperties = JsonConvert.DeserializeObject<List<ElsaProperty>>(property.Expressions[SyntaxNames.Json]);
                List<ElsaProperty> propertiesToEvaluate = elsaCheckboxProperties != null ? elsaCheckboxProperties : new List<ElsaProperty>();
                WeightedRadioRecord[] weightedRecords = await Task.WhenAll(propertiesToEvaluate
                    .Select(x => ElsaPropertyToWeightedRadioRecord(x, evaluator, context)));
                return weightedRecords.ToList();
            }
            return new List<WeightedRadioRecord>();
        }

        private async Task<WeightedRadioRecord> ElsaPropertyToWeightedRadioRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string identifier = property.Name;
            string value = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
            bool isPrePopulated = await EvaluatePrePopulated(property, evaluator, context);
            decimal score = await EvaluateScore(property, evaluator, context);

            return new WeightedRadioRecord(identifier, value, score, isPrePopulated);

        }

        public async Task<bool> EvaluatePrePopulated(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(RadioSyntaxNames.PrePopulated))
            {
                string expression = property.Expressions[RadioSyntaxNames.PrePopulated] ?? "false";
                bool prePopulated = await property.EvaluateFromExpressionsExplicit<bool>(evaluator,
                    context, _logger,
                    expression,
                    SyntaxNames.JavaScript,
                    CancellationToken.None);
                return prePopulated;
            }
            return false;
        }

        public async Task<decimal> EvaluateScore(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(ScoringSyntaxNames.Score))
            {
                string expression = property.Expressions[ScoringSyntaxNames.Score] ?? "-1";
                decimal score = await property.EvaluateFromExpressionsExplicit<decimal>(evaluator,
                    context, _logger,
                    expression,
                    SyntaxNames.Literal,
                    CancellationToken.None);
                return score;
            }
            return 0;
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
