using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;
using System;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class CheckboxQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly Random random = new Random();

        public CheckboxQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }

        public async Task<string> GetAnswer(string correlationId, string workflowName, string activityName, string questionId)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, questionId, CancellationToken.None);

            if (question != null && question.Answers != null &&
                        (question.QuestionType == QuestionTypeConstants.CheckboxQuestion || question.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion))
            {
                return string.Join(",", question.Answers.Select(x => x.AnswerText));
            }
            return string.Empty;
        }

        public async Task<bool> AnswerEquals(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            bool result = false;
            var randomId = random.Next();
            var choiceIdsString = string.Join(", ", choiceIdsToCheck);
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, questionId, CancellationToken.None);
            if (question != null &&
                        (question.QuestionType == QuestionTypeConstants.CheckboxQuestion || question.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion) &&
                        question.Answers != null)
            {
                activityExecutionContext.JournalData.Add($"checkboxQuestionAnswerEquals Question Found for choices {choiceIdsString}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}");
                if (question.Answers.Count != choiceIdsToCheck.Length)
                    return false;
                foreach (var item in choiceIdsToCheck)
                {
                    randomId = random.Next();
                    var answerFound = question.Answers.FirstOrDefault(x => x.Choice?.Identifier == item);

                    if (answerFound != null)
                    {
                        activityExecutionContext.JournalData.Add($"checkboxQuestionAnswerEquals Answer Found for choices {choiceIdsString}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}, Answer: {answerFound.AnswerText}, Choice: {answerFound.Choice?.Identifier}");
                        result = true;
                    }
                    else
                    {
                        activityExecutionContext.JournalData.Add($"checkboxQuestionAnswerEquals Answer  not Found for choices {choiceIdsString}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId},  Choice: {item}");
                        result = false;
                    }
                }
            }
            else
            {
                activityExecutionContext.JournalData.Add($"checkboxQuestionAnswerEquals Question NULL for choices {choiceIdsToCheck}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdsToCheck}");
            }

            return result;
        }

        public async Task<bool> AnswerContains(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName,
            string questionId, string[] choiceIdsToCheck, bool containsAny = false)
        {
            var methodName = containsAny ? "checkboxQuestionAnswerContainsAny" : "checkboxQuestionAnswerContains";
            bool result = false;
            var randomId = random.Next();
            var choiceIdsString = string.Join(", ", choiceIdsToCheck);
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, questionId, CancellationToken.None);
            if (question != null &&
                        (question.QuestionType == QuestionTypeConstants.CheckboxQuestion || question.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion))
            {
                activityExecutionContext.JournalData.Add($"{methodName} Question Found for choices {choiceIdsString}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}");

                if (question.Answers != null)
                {
                    foreach (var item in choiceIdsToCheck)
                    {
                        randomId = random.Next();
                        var answerFound = question.Answers.FirstOrDefault(x => x.Choice?.Identifier == item);
                        if (answerFound != null)
                        {
                            activityExecutionContext.JournalData.Add($"{methodName} Answer Found for choices {choiceIdsString}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}, Answer: {answerFound.AnswerText}, Choice: {answerFound.Choice?.Identifier}");

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
                            activityExecutionContext.JournalData.Add($"{methodName} Answer  not Found for choices {choiceIdsString}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId},  Choice: {item}");
                            result = false;
                        }
                    }
                }
            }
            else
            {
                activityExecutionContext.JournalData.Add($"{methodName} Question NULL for choices {choiceIdsString}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdsToCheck}");
            }
            return result;
        }

        public async Task<int> AnswerCount(string correlationId, string workflowName, string activityName, string questionId)
        {
            int result = 0;
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, questionId, CancellationToken.None);
            if (question != null && (question.QuestionType == QuestionTypeConstants.CheckboxQuestion || question.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion))
            {
                if (question.Answers != null)
                {
                    return question.Answers.Count;
                }
            }
            return result;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("checkboxQuestionGetAnswer", (Func<string, string, string, string?>)((workflowName, activityName, questionId) => GetAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("checkboxQuestionAnswerEquals", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck).Result));
            engine.SetValue("checkboxQuestionAnswerContains", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerContains(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck).Result));
            engine.SetValue("checkboxQuestionAnswerContainsAny", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerContains(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck, true).Result));
            engine.SetValue("checkboxQuestionAnswerCount", (Func<string, string, string, int>)((workflowName, activityName, questionId) => AnswerCount(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
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
