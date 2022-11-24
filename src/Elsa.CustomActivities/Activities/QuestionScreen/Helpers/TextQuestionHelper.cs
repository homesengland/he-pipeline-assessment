using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class TextQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;

        public TextQuestionHelper(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }


        public async Task<bool> AnswerEquals(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string answerToCheck)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var result = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            if (result != null && result.Answer != null && result.Answer.ToLower() == answerToCheck.ToLower())
            {
                return true;
            }

            return false;
        }

        public async Task<bool> AnswerContains(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string answerToCheck)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var result = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            if (result != null && result.Answer != null && result.Answer.ToLower().Contains(answerToCheck.ToLower()))
            {
                return true;
            }

            return false;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("textQuestionAnswerEquals", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("textQuestionAnswerContains", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerContains(activityExecutionContext, workflowName, activityName, questionId, answerToCheck).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function textQuestionAnswerEquals(workflowName: string, activityName:string, questionId:string, answerToCheck:string ): boolean;");
            output.AppendLine("declare function textQuestionAnswerContains(workflowName: string, activityName:string, questionId:string, answerToCheck:string  ): boolean;");
            return Task.CompletedTask;
        }
    }
}
