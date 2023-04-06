using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;

namespace Elsa.CustomActivities.Activities.Scoring.Helpers
{
    public class PotScoreCalculationHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;

        public PotScoreCalculationHelper(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<double> GetTotalPotValue(string workflowInstanceId, string potValue)
        {
            double failedResult = -1;
            List<double> totalSelectedScores = new List<double>();

            var workflowQuestions = await _elsaCustomRepository.GetQuestions(workflowInstanceId, CancellationToken.None);
            if(workflowQuestions.Count > 0)
            {
                foreach(var question in workflowQuestions)
                {
                    double? questionScore = question.Answers?.Where(a => a.Choice?.PotScoreCategory?.ToLower() == potValue.ToLower())
                        .Select(_ => question.Weighting).FirstOrDefault();
                    if(questionScore != null)
                    {
                        totalSelectedScores.Add(questionScore.Value);
                    }
                }
                return totalSelectedScores.Sum();
            }
            return failedResult;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("getTotalPotValue", (Func<string, string, double>)((workflowName, potName) => GetTotalPotValue(activityExecutionContext.WorkflowInstance.Id, potName).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function getTotalPotValue(workflowInstanceId: string, potValueName:string ): number;");
            return Task.CompletedTask;
        }
    }
}
