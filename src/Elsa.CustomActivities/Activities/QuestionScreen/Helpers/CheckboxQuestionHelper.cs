using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Persistence;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class CheckboxQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public CheckboxQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowInstanceStore workflowInstanceStore, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }

        public async Task<string> GetAnswer(string workflowInstanceId, string workflowName, string activityName, string questionId)
        {
            string result = String.Empty;
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var questionScreenQuestion = await _elsaCustomRepository.GetQuestion(activity.Id,
                        workflowInstanceId, questionId, CancellationToken.None);

                    if (questionScreenQuestion != null && questionScreenQuestion.Answers != null &&
                        questionScreenQuestion.QuestionType == QuestionTypeConstants.CheckboxQuestion)
                    {
                        return string.Join(",", questionScreenQuestion.Answers.Select(x => x.AnswerText));
                    }
                }
            }

            return result;
        }

        public async Task<bool> AnswerEquals(string workflowInstanceId, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            bool result = false;
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var questionScreenQuestion = await _elsaCustomRepository.GetQuestion(activity.Id,
                        workflowInstanceId, questionId, CancellationToken.None);
                    if (questionScreenQuestion != null &&
                        questionScreenQuestion.QuestionType == QuestionTypeConstants.CheckboxQuestion &&
                        questionScreenQuestion.Choices != null)
                    {
                        var choices = questionScreenQuestion.Choices;
                        if (choices != null && questionScreenQuestion.Answers != null)
                        {
                            var answerList = questionScreenQuestion.Answers.Select(x => x.AnswerText).ToList();
                            foreach (var item in answerList)
                            {
                                var singleChoice = choices.FirstOrDefault(x => x.Answer == item);

                                if (singleChoice != null && choiceIdsToCheck.Contains(singleChoice.Identifier))
                                {
                                    result = true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public async Task<bool> AnswerContains(string workflowInstanceId, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            bool result = false;
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {

                    var questionScreenQuestion = await _elsaCustomRepository.GetQuestion(activity.Id, workflowInstanceId, questionId, CancellationToken.None);
                    if (questionScreenQuestion != null &&
                        questionScreenQuestion.QuestionType == QuestionTypeConstants.CheckboxQuestion)
                    {
                        var choices = questionScreenQuestion.Choices;
                        if (choices != null && questionScreenQuestion.Answers != null)
                        {
                            var answerList = questionScreenQuestion.Answers.Select(x => x.AnswerText).ToList();
                            foreach (var item in choiceIdsToCheck)
                            {
                                var singleChoice = choices.FirstOrDefault(x => x.Identifier == item);
                                if (singleChoice != null)
                                {
                                    var answerCheck = choices.Select(x => x.Identifier).Contains(item) &&
                                                      answerList.Contains(singleChoice.Answer);

                                    if (answerCheck)
                                    {
                                        result = true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
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
            engine.SetValue("checkboxQuestionGetAnswer", (Func<string, string, string, string?>)((workflowName, activityName, questionId) => GetAnswer(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId).Result));
            engine.SetValue("checkboxQuestionAnswerEquals", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerEquals(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, choiceIdsToCheck).Result));
            engine.SetValue("checkboxQuestionAnswerContains", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerContains(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, choiceIdsToCheck).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function checkboxQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): string;");
            output.AppendLine("declare function checkboxQuestionAnswerEquals(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[] ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerContains(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[]  ): boolean;");
            return Task.CompletedTask;
        }
    }
}
