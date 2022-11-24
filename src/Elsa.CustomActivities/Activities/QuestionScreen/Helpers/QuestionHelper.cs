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

        public QuestionHelper(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }


        public async Task<string> GetAnswer(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var result = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            return result.Answer;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("questionGetAnswer", (Func<string, string, string, string?>)((workflowName, activityName, questionId) => GetAnswer(activityExecutionContext, workflowName, activityName, questionId).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function questionGetAnswer(workflowName: string, activityName:string, questionId:string ): string;");
            return Task.CompletedTask;
        }
    }
}
