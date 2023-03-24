using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class TextQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public TextQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }


        public async Task<bool> AnswerEquals(string workflowInstanceId, string workflowName, string activityName, string questionId, string answerToCheck)
        {

            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var result = await _elsaCustomRepository.GetQuestionScreenQuestion(activity.Id,
                        workflowInstanceId, questionId, CancellationToken.None);

                    if (result != null && (result.QuestionType == QuestionTypeConstants.TextQuestion ||
                        result.QuestionType == QuestionTypeConstants.TextAreaQuestion) &&
                        result.Answers != null && result.Answers.Count == 1 && result.Answers.First().Answer.ToLower() == answerToCheck.ToLower())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> AnswerContains(string workflowInstanceId, string workflowName, string activityName, string questionId, string answerToCheck)
        {
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var result = await _elsaCustomRepository.GetQuestionScreenQuestion(activity.Id,
                        workflowInstanceId, questionId, CancellationToken.None);

                    if (result != null && (result.QuestionType == QuestionTypeConstants.TextQuestion ||
                        result.QuestionType == QuestionTypeConstants.TextAreaQuestion) &&
                        result.Answers != null && result.Answers.Count == 1 && result.Answers.First().Answer.ToLower().Contains(answerToCheck.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("textQuestionAnswerEquals", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEquals(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("textQuestionAnswerContains", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerContains(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, answerToCheck).Result));
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
