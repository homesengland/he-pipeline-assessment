using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;
using Elsa.CustomInfrastructure.Data.Repository;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Activities.Scoring.Helpers
{
    public class WeightedScoreCalculationHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<WeightedScoreCalculationHelper> _logger;

        public WeightedScoreCalculationHelper(IElsaCustomRepository elsaCustomRepository, ILogger<WeightedScoreCalculationHelper> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<decimal?> GetTotalWeightedScoreValue(string workflowInstanceId)
        {
            try
            {
                decimal? result = 0;

                var workflowQuestions = await _elsaCustomRepository.GetWorkflowInstanceQuestions(workflowInstanceId, CancellationToken.None);
                if (workflowQuestions.Count > 0)
                {
                    result = workflowQuestions.Where(question => question.Score.HasValue)
                        .Aggregate(result, (current, question) => current + question.Score);
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Error whilst retrieving Total Weighted Score: {0}", e);
                throw (new Exception(e.Message, e.InnerException));
            }

        }

        public async Task<double> GetWeightedScoreCalculation(string correlationId, string name)
        {
            try
            {
                double result = 0;

                var workflowInstances = await _elsaCustomRepository.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None);

                if (workflowInstances.Any())
                {
                    var latestCalculation = workflowInstances.OrderByDescending(x => x.CreatedDateTime).First();
                    if (latestCalculation.Score != null)
                    {
                        double.TryParse(latestCalculation.Score, out result);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(
                    $"Error whilst retrieving Total Weighted Score Calculation: '{name}' for Correlation Id {correlationId}", e);
                throw (new Exception(e.Message, e.InnerException));
            }
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("getTotalWeightedScoreValue", (Func<string, decimal?>)(_ => GetTotalWeightedScoreValue(activityExecutionContext.WorkflowInstance.Id).Result));
            engine.SetValue("getWeightedScoreCalculation", (Func<string, double>)((name) => GetWeightedScoreCalculation(activityExecutionContext.CorrelationId, name).Result));
            return Task.CompletedTask;
        }

       

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function getTotalWeightedScoreValue(): number;");
            output.AppendLine("declare function getWeightedScoreCalculation(name:string): number;");
            return Task.CompletedTask;
        }
    }
}
