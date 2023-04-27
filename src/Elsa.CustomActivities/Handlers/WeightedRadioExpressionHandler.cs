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
        public string Syntax => CustomSyntaxNames.CheckboxList;


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
            var radioGroups = await ElsaPropertiesToWeightedRadioGroup(checkboxProperties, evaluator, context);
            result.Groups = radioGroups;
            return result;
        }

        public async Task<Dictionary<string, WeightedRadioGroup>> ElsaPropertiesToWeightedRadioGroup(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            WeightedRadioGroup[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToWeightedRadioGroup(x, evaluator, context)));
            return resultArray.ToDictionary(x => x.GroupIdentifier);
        }

        private async Task<WeightedRadioGroup> ElsaPropertyToWeightedRadioGroup(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string groupName = property.Name;
            int? maxGroupScore = await EvaluateMaxGroupScore(property, evaluator, context);
            List<int>? groupArrayScore = EvaluateGroupArrayScore(property, evaluator, context);
            List<WeightedRadioRecord> records = await EvaluateRadioRecords(property, evaluator, context);

            return new WeightedRadioGroup
            {
                GroupIdentifier = groupName,
                MaxGroupScore = maxGroupScore,
                Choices = records
            };
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

        public async Task<int?> EvaluateMaxGroupScore(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(ScoringSyntaxNames.MaxScore))
            {
                string expression = property.Expressions[ScoringSyntaxNames.MaxScore] ?? "-1";
                int? maxScore = await property.EvaluateFromExpressionsExplicit<int>(evaluator, 
                    context, 
                    _logger, 
                    expression, 
                    ScoringSyntaxNames.MaxScore);
                return maxScore ?? null;
            }
            return null;
        }

        private async Task<WeightedRadioRecord> ElsaPropertyToWeightedRadioRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string identifier = property.Name;
            string value = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
            bool isPrePopulated = await EvaluatePrePopulated(property, evaluator, context);
            int score = await EvaluateScore(property, evaluator, context);

            return new WeightedRadioRecord(identifier, value, score, isPrePopulated);

        }

        public List<int>? EvaluateGroupArrayScore(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(ScoringSyntaxNames.ScoreArray) && property.Expressions[ScoringSyntaxNames.ScoreArray]!= null)
            {
                string arrayString = property.Expressions![ScoringSyntaxNames.ScoreArray];
                List<int>? array = JsonConvert.DeserializeObject<List<int>>(arrayString);
                return array;
            }
            return null;
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

        public async Task<int> EvaluateScore(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(ScoringSyntaxNames.Score))
            {
                string expression = property.Expressions[ScoringSyntaxNames.Score] ?? "-1";
                int score = await property.EvaluateFromExpressionsExplicit<int>(evaluator,
                    context, _logger,
                    expression,
                    SyntaxNames.Literal,
                    CancellationToken.None);
                return score;
            }
            return -1;
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
