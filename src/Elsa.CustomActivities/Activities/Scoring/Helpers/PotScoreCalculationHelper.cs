using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Persistence;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Activities.Scoring.Helpers
{
    public class PotScoreCalculationHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<PotScoreCalculationHelper> _logger;

        public PotScoreCalculationHelper(IElsaCustomRepository elsaCustomRepository, ILogger<PotScoreCalculationHelper> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<double> GetTotalPotValue(string workflowInstanceId, string potValue)
        {
            try
            {
                double failedResult = -1;
                List<double> totalSelectedScores = new List<double>();

                var workflowQuestions = await _elsaCustomRepository.GetQuestions(workflowInstanceId, CancellationToken.None);
                if (workflowQuestions.Count > 0)
                {
                    foreach (var question in workflowQuestions)
                    {
                        double? questionScore = question.Answers?.Where(a => a.Choice?.PotScoreCategory?.ToLower() == potValue.ToLower())
                            .Select(_ => question.Weighting).FirstOrDefault();
                        if (questionScore != null)
                        {
                            totalSelectedScores.Add(questionScore.Value);
                        }
                    }
                    return totalSelectedScores.Sum();
                }
                return failedResult;
            }
            catch(Exception e)
            {
                _logger.LogError(string.Format("Error whilst retrieving Pot Value: {0}", potValue), e);
                throw (e);
            }

        }

        public async Task<string> GetPotScore(string workflowInstanceId)
        {
            string failedResult = string.Empty;

            QuestionWorkflowInstance? workflow = await _elsaCustomRepository.GetQuestionWorkflowInstance(workflowInstanceId, CancellationToken.None);
            if(workflow != null && workflow.Score != null)
            {
                return workflow.Score;
            }
            else
            {
                return failedResult;
            }
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("getTotalPotValue", (Func<string, string, double>)((workflowName, potName) => GetTotalPotValue(activityExecutionContext.WorkflowInstance.Id, potName).Result));
            engine.SetValue("getPotScore", (Func<string, string>)((workflowInstanceId) => GetPotScore(activityExecutionContext.WorkflowInstance.Id).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function getTotalPotValue(workflowInstanceId: string, potValueName:string ): number;");
            output.AppendLine("declare function getPotScore(): string;");
            return Task.CompletedTask;
        }
    }
}
