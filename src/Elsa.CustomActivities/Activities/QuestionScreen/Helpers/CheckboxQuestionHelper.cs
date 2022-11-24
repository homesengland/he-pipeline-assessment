using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;
using System.Text.Json;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class CheckboxQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;

        public CheckboxQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowInstanceStore workflowInstanceStore)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowInstanceStore = workflowInstanceStore;
        }


        public async Task<bool> AnswerEquals(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            bool result = false;
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);
            if (questionScreenAnswer != null && questionScreenAnswer.QuestionType == QuestionTypeConstants.CheckboxQuestion)
            {
                var workflowSpecification =
                    new WorkflowInstanceIdSpecification(questionScreenAnswer.WorkflowInstanceId);
                var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, CancellationToken.None);

                if (workflowInstance != null)
                {
                    var dictionList = workflowInstance.ActivityData
                        .FirstOrDefault(x => x.Key == questionScreenAnswer.ActivityId).Value;

                    var dictionaryQuestions = (AssessmentQuestions?)dictionList.FirstOrDefault(x => x.Key == "Questions").Value;
                    if (dictionaryQuestions != null)
                    {
                        var questionList = (List<Question>)dictionaryQuestions.Questions;
                        var question = questionList.FirstOrDefault(x => x.Id == questionScreenAnswer.QuestionId);

                        if (question != null)
                        {
                            var choices = question.Checkbox.Choices;

                            var answerList = JsonSerializer.Deserialize<string[]>(questionScreenAnswer.Answer!);
                            if (answerList != null)
                            {

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
            }
            return result;
        }

        public async Task<bool> AnswerContains(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            bool result = false;
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);
            if (questionScreenAnswer != null && questionScreenAnswer.QuestionType == QuestionTypeConstants.CheckboxQuestion)
            {
                var workflowSpecification =
                    new WorkflowInstanceIdSpecification(questionScreenAnswer.WorkflowInstanceId);
                var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, CancellationToken.None);

                if (workflowInstance != null)
                {
                    var dictionList = workflowInstance.ActivityData
                        .FirstOrDefault(x => x.Key == questionScreenAnswer.ActivityId).Value;

                    var dictionaryQuestions = (AssessmentQuestions?)dictionList.FirstOrDefault(x => x.Key == "Questions").Value;
                    if (dictionaryQuestions != null)
                    {
                        var questionList = (List<Question>)dictionaryQuestions.Questions;
                        var question = questionList.FirstOrDefault(x => x.Id == questionScreenAnswer.QuestionId);

                        if (question != null)
                        {
                            var choices = question.Checkbox.Choices;

                            var answerList = JsonSerializer.Deserialize<string[]>(questionScreenAnswer.Answer!);
                            if (answerList != null)
                            {
                                foreach (var item in choiceIdsToCheck)
                                {
                                    var singleChoice = choices.FirstOrDefault(x => x.Identifier == item);
                                    if (singleChoice != null)
                                    {
                                        var answerCheck = choices.Select(x => x.Identifier).Contains(item) && answerList.Contains(singleChoice.Answer);

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

            }
            return result;
        }

        public async Task<bool> AnswerContains(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string choiceIdToCheck)
        {
            bool result = false;
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);
            if (questionScreenAnswer != null && questionScreenAnswer.QuestionType == QuestionTypeConstants.CheckboxQuestion)
            {
                var workflowSpecification =
                    new WorkflowInstanceIdSpecification(questionScreenAnswer.WorkflowInstanceId);
                var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, CancellationToken.None);

                if (workflowInstance != null)
                {
                    var dictionList = workflowInstance.ActivityData
                        .FirstOrDefault(x => x.Key == questionScreenAnswer.ActivityId).Value;

                    var dictionaryQuestions = (AssessmentQuestions?)dictionList.FirstOrDefault(x => x.Key == "Questions").Value;
                    if (dictionaryQuestions != null)
                    {
                        var questionList = (List<Question>)dictionaryQuestions.Questions;
                        var question = questionList.FirstOrDefault(x => x.Id == questionScreenAnswer.QuestionId);

                        if (question != null)
                        {
                            var choices = question.Checkbox.Choices;

                            var answerList = JsonSerializer.Deserialize<string[]>(questionScreenAnswer.Answer!);
                            if (answerList != null)
                            {
                                foreach (var item in answerList)
                                {
                                    var singleChoice = choices.FirstOrDefault(x => x.Answer == item);

                                    if (singleChoice != null && singleChoice.Identifier == choiceIdToCheck)
                                    {
                                        return true;
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
            engine.SetValue("checkboxQuestionAnswerEquals", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck).Result));
            engine.SetValue("checkboxQuestionAnswerContainsMultiple", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerContains(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck).Result));
            engine.SetValue("checkboxQuestionAnswerContainsSingle", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, choiceIdToCheck) => AnswerContains(activityExecutionContext, workflowName, activityName, questionId, choiceIdToCheck).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function checkboxQuestionAnswerEquals(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[] ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerContainsMultiple(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[]  ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerContainsSingle(workflowName: string, activityName:string, questionId:string, choiceIdToCheck:string  ): boolean;");
            return Task.CompletedTask;
        }
    }
}
