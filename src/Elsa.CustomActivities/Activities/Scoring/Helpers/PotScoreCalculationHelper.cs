using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
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
        private readonly IWorkflowRegistry _workflowRegistry;

        public PotScoreCalculationHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowInstanceStore workflowInstanceStore, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }

        public async Task<double> GetTotalPotValue(string workflowInstanceId, string workflowName, string potValue)
        {
            double failedResult = -1;
            List<double> totalSelectedScores = new List<double>();
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);
            if (workflowBlueprint != null)
            {
                List<CustomModels.Question> workflowQuestions = new List<CustomModels.Question>();
                workflowQuestions = await _elsaCustomRepository.GetQuestions(workflowInstanceId, CancellationToken.None);
                if(workflowQuestions != null && workflowQuestions.Count > 0)
                {
                    foreach(var question in workflowQuestions)
                    {
                        double? questionScore = question.Answers?.Where(a => a.Choice?.PotScoreCategory?.ToLower() == potValue)
                            .Select(a => a.Question.Weighting).FirstOrDefault();
                        if(questionScore != null)
                        {
                            totalSelectedScores.Add(questionScore.Value);
                        }
                    }
                    return totalSelectedScores.Sum();
                }
                return failedResult;
            }
            return failedResult;
        }
        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("getTotalPotValue", (Func<string, string, double>)((workflowName, potName) => GetTotalPotValue(activityExecutionContext.WorkflowInstance.Id, workflowName, potName).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function getTotalPotValue(workflowName: string, potValueName:string ): number;");
            return Task.CompletedTask;
        }
    }
}
