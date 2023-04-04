using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Activities.Scoring;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomActivities.Handlers.Scoring
{
    public class PotScoreCalculationHandler : IExpressionHandler
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<IExpressionHandler> _logger;
        public string Syntax => ScoringSyntaxNames.PotScore;


        public PotScoreCalculationHandler(ILogger<IExpressionHandler> logger, IContentSerializer contentSerializer)
        {
            _logger = logger;
            _contentSerializer = contentSerializer;
        }

        public async Task<object?> EvaluateAsync(string expression, Type returnType, ActivityExecutionContext context, CancellationToken cancellationToken)
        {
            var evaluator = context.GetService<IExpressionEvaluator>();
            PotScore result = new PotScore();
            var potScoreDictionary = TryDeserializeExpression(expression);
            string score = await ElsaPropertyToCalculatedPotScoreResult(potScoreDictionary, evaluator, context);
            result.Output = score;
            return result;
        }

        private async Task<string> ElsaPropertyToCalculatedPotScoreResult(ElsaProperty potScoreDictionary, IExpressionEvaluator evaluator, ActivityExecutionContext context)
        {
            string outcome = await potScoreDictionary.EvaluateFromExpressions<string>(evaluator, context, _logger, CancellationToken.None);
            return outcome;
        }

        private ElsaProperty TryDeserializeExpression(string expression)
        {
            try
            {
                return _contentSerializer.Deserialize<ElsaProperty>(expression);
            }
            catch
            {
                Dictionary<string, string> defaultExpressions = new Dictionary<string, string>();
                defaultExpressions.Add(SyntaxNames.Literal, string.Empty);
                return new ElsaProperty(defaultExpressions, SyntaxNames.Literal, string.Empty, string.Empty);
            }
        }
    }
}
