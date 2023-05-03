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
    public class WeightedCheckboxExpressionHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;
        public string Syntax => CustomSyntaxNames.CheckboxList;


        public WeightedCheckboxExpressionHandler(ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)
        {
            var evaluator = context.GetService<IExpressionEvaluator>();
            WeightedCheckboxModel result = new WeightedCheckboxModel();
            var checkboxProperties = TryDeserializeExpression(expression);
            var checkboxGroups = await ElsaPropertiesToWeightedCheckboxGroup(checkboxProperties, evaluator, context);
            result.Groups = checkboxGroups;
            return result;
        }

        public async Task<Dictionary<string, WeightedCheckboxGroup>> ElsaPropertiesToWeightedCheckboxGroup(List<ElsaProperty> properties, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            WeightedCheckboxGroup[] resultArray = await Task.WhenAll(properties.Select(x => ElsaPropertyToWeightedCheckboxGroup(x, evaluator, context)));
            return resultArray.ToDictionary(x => x.GroupIdentifier);
        }

        private async Task<WeightedCheckboxGroup> ElsaPropertyToWeightedCheckboxGroup(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string groupName = property.Name;
            int? maxGroupScore = await EvaluateMaxGroupScore(property, evaluator, context);
            List<decimal>? groupArrayScore = EvaluateGroupArrayScore(property, evaluator, context);
            List<WeightedCheckboxRecord> records = await EvaluateCheckboxRecords(property, evaluator, context);

            return new WeightedCheckboxGroup
            {
                GroupIdentifier = groupName,
                MaxGroupScore = maxGroupScore,
                GroupArrayScore = groupArrayScore,
                Choices = records
            };
        }



        public async Task<List<WeightedCheckboxRecord>> EvaluateCheckboxRecords(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(SyntaxNames.Json))
            {
                var elsaCheckboxProperties = JsonConvert.DeserializeObject<List<ElsaProperty>>(property.Expressions[SyntaxNames.Json]);
                List<ElsaProperty> propertiesToEvaluate = elsaCheckboxProperties != null ? elsaCheckboxProperties : new List<ElsaProperty>();
                WeightedCheckboxRecord[] weightedRecords = await Task.WhenAll(propertiesToEvaluate
                    .Select(x => ElsaPropertyToWeightedCheckboxRecord(x, evaluator, context)));
                return weightedRecords.ToList();
            }
            return new List<WeightedCheckboxRecord>();
        }

        public async Task<int?> EvaluateMaxGroupScore(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(ScoringSyntaxNames.MaxGroupScore))
            {
                string expression = property.Expressions[ScoringSyntaxNames.MaxGroupScore] ?? "0";
                int? maxScore = await property.EvaluateFromExpressionsExplicit<int>(evaluator, 
                    context, 
                    _logger, 
                    expression, 
                    SyntaxNames.Literal);
                return maxScore ?? null;
            }
            return null;
        }

        private async Task<WeightedCheckboxRecord> ElsaPropertyToWeightedCheckboxRecord(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string identifier = property.Name;
            string value = await property.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
            bool isSingle = EvaluateIsSingle(property, evaluator, context);
            bool isPrePopulated = await EvaluatePrePopulated(property, evaluator, context);
            int score = await EvaluateScore(property, evaluator, context);

            return new WeightedCheckboxRecord(identifier, value, isSingle, score, isPrePopulated);

        }

        public List<decimal>? EvaluateGroupArrayScore(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(ScoringSyntaxNames.GroupArrayScore))
            {
                ElsaProperty? elsaProperty = JsonConvert.DeserializeObject<ElsaProperty>(property.Expressions![ScoringSyntaxNames.GroupArrayScore]);
                if (elsaProperty != null)
                {
                    List<decimal>? array = JsonConvert.DeserializeObject<List<decimal>>(elsaProperty.Expressions![SyntaxNames.Json]);
                    return array;
                }
            }
            return null;
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

        public async Task<int> EvaluateScore(ElsaProperty property, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            if (property.Expressions!.ContainsKey(ScoringSyntaxNames.Score))
            {
                string expression = property.Expressions[ScoringSyntaxNames.Score] ?? "0";
                int score = await property.EvaluateFromExpressionsExplicit<int>(evaluator,
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
