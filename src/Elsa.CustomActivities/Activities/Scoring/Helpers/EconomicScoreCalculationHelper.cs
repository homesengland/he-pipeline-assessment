using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Activities.Scoring.Helpers
{
    public class EconomicScoreCalculationHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<EconomicScoreCalculationHelper> _logger;

        public EconomicScoreCalculationHelper(IElsaCustomRepository elsaCustomRepository, ILogger<EconomicScoreCalculationHelper> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<double> GetEconomicCalculation(string correlationId, string name)
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
            catch(Exception e)
            {
                _logger.LogError(
                    $"Error whilst retrieving Economic Calculation: '{name}' for Correlation Id {correlationId}", e);
                throw (new Exception(e.Message, e.InnerException));
            }
        }

        public async Task<string> GetEconomicCalculationAsString(string correlationId, string name)
        {
            try
            {
                string result = string.Empty;

                var workflowInstances = await _elsaCustomRepository.GetQuestionWorkflowInstancesByName(correlationId, name, CancellationToken.None);

                if (workflowInstances.Any())
                {
                    var latestCalculation = workflowInstances.OrderByDescending(x => x.CreatedDateTime).First();
                    if (latestCalculation.Score != null)
                    {
                        result = latestCalculation.Score;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(
                    $"Error whilst retrieving Economic Calculation: '{name}' for Correlation Id {correlationId}", e);
                throw (new Exception(e.Message, e.InnerException));
            }
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("getEconomicCalculation", (Func<string, double>)((name) => GetEconomicCalculation(activityExecutionContext.CorrelationId, name).Result));
            engine.SetValue("getEconomicCalculationAsString", (Func<string, string>)((name) => GetEconomicCalculationAsString(activityExecutionContext.CorrelationId, name).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function getEconomicCalculation(name:string): number;");
            output.AppendLine("declare function getEconomicCalculationAsString(name:string): string;");
            return Task.CompletedTask;
        }
    }
}
