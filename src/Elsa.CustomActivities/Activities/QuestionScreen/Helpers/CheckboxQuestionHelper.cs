using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;
using Newtonsoft.Json;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class CheckboxQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public CheckboxQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
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
                    var question = await _elsaCustomRepository.GetQuestion(activity.Id,
                        workflowInstanceId, questionId, CancellationToken.None);

                    if (question != null && question.Answers != null &&
                        question.QuestionType == QuestionTypeConstants.CheckboxQuestion)
                    {
                        return string.Join(",", question.Answers.Select(x => x.AnswerText));
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
                    var question = await _elsaCustomRepository.GetQuestion(activity.Id,
                        workflowInstanceId, questionId, CancellationToken.None);
                    if (question != null &&
                        question.QuestionType == QuestionTypeConstants.CheckboxQuestion &&
                        question.Choices != null)
                    {
                        var choices = question.Choices;
                        if (choices != null && question.Answers != null)
                        {
                            var answerList = question.Answers.Select(x => x.AnswerText).ToList();
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

        public async Task<bool> AnswerContains(string workflowInstanceId, string workflowName, string activityName,
            string questionId, string[] choiceIdsToCheck, bool containsAny = false)
        {
            bool result = false;
            var workflowBlueprint =
                await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {

                    var question = await _elsaCustomRepository.GetQuestion(activity.Id, workflowInstanceId, questionId,
                        CancellationToken.None);
                    if (question != null &&
                        question.QuestionType == QuestionTypeConstants.CheckboxQuestion)
                    {
                        //var choices = question.Choices;
                        if (/*choices != null &&*/ question.Answers != null)
                        {
                            foreach (var item in choiceIdsToCheck)
                            {
                                var answerFound = question.Answers.FirstOrDefault(x => x.Choice?.Identifier == item);
                                if (answerFound != null)
                                {
                                    if (containsAny)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<int> AnswerCount(string workflowInstanceId, string workflowName, string activityName, string questionId)
        {
            int result = -1;
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {

                    var question = await _elsaCustomRepository.GetQuestion(activity.Id, workflowInstanceId, questionId, CancellationToken.None);
                    if (question != null && question.QuestionType == QuestionTypeConstants.CheckboxQuestion)
                    {
                        if (question.Answers != null)
                        {
                            return question.Answers.Count;
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
            engine.SetValue("checkboxQuestionAnswerContainsAny", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerContains(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, choiceIdsToCheck, true).Result));
            engine.SetValue("checkboxQuestionAnswerCount", (Func<string, string, string, int>)((workflowName, activityName, questionId) => AnswerCount(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function checkboxQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): string;");
            output.AppendLine("declare function checkboxQuestionAnswerEquals(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[] ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerContains(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[]  ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerContainsAny(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[]  ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerCount(workflowName: string, activityName:string, questionId:string ): int;");
            return Task.CompletedTask;
        }
    }
}
