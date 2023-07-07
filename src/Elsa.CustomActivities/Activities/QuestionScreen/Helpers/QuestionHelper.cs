using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class QuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public QuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }


        public async Task<string> GetAnswer(string correlationId, string workflowName, string activityName, string questionId)
        {
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var result = await _elsaCustomRepository.GetQuestionByCorrelationId(activity.Id, correlationId, questionId, CancellationToken.None);

                    if (result?.Answers != null)
                    {
                        return string.Join(',', result.Answers.Select(x => x.AnswerText));
                    }
                }

            }
            return string.Empty;

        }

        public  Task<string> WriteJournalData(string key, string message,ActivityExecutionContext context)
        {
            context.JournalData.Add(key,message);

            return Task.FromResult(string.Empty);
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("questionGetAnswer", (Func<string, string, string, string?>)((workflowName, activityName, questionId) => GetAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("writeJournalData", (Func<string,string, string>)((key, message) => WriteJournalData(key,message,activityExecutionContext).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function questionGetAnswer(workflowName: string, activityName:string, questionId:string ): string;");
            output.AppendLine("declare function writeJournalData(key: string, message:string): string;");
            return Task.CompletedTask;
        }
    }
}
