using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class RadioQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public RadioQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }


        public async Task<bool> AnswerEquals(string workflowInstance, string workflowName, string activityName, string questionId, string choiceIdToCheck)
        {
            bool result = false;
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {

                    var question = await _elsaCustomRepository.GetQuestion(activity.Id,
                        workflowInstance, questionId, CancellationToken.None);
                    if (question != null &&
                        (question.QuestionType == QuestionTypeConstants.RadioQuestion || question.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion))
                    {
                        if (question.Answers != null && question.Answers.Count == 1)
                        {
                            var singleAnswer = question.Answers.First();

                            return choiceIdToCheck == singleAnswer.Choice?.Identifier;
                        }

                        return false;
                    }
                }
            }

            return result;
        }

        public async Task<bool> AnswerIn(string workflowInstance, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            bool result = false;
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var question = await _elsaCustomRepository.GetQuestion(activity.Id,
                        workflowInstance, questionId, CancellationToken.None);
                    if (question != null &&
                        (question.QuestionType == QuestionTypeConstants.RadioQuestion || question.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion))
                    {
                        var choices = question.Choices;

                        if (choices != null && question.Answers != null && question.Answers.Count == 1)
                        {
                            var singleAnswer = question.Answers.First();
                            return choiceIdsToCheck.Contains(singleAnswer.Choice?.Identifier);
                        }
                    }
                }
            }
            return result;
        }



        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("radioQuestionAnswerEquals", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, choiceIdToCheck) => AnswerEquals(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, choiceIdToCheck).Result));
            engine.SetValue("radioQuestionAnswerIn", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerIn(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, choiceIdsToCheck).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function radioQuestionAnswerEquals(workflowName: string, activityName:string, questionId:string, choiceIdToCheck:string ): boolean;");
            output.AppendLine("declare function radioQuestionAnswerIn(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[]  ): boolean;");
            return Task.CompletedTask;
        }
    }
}
