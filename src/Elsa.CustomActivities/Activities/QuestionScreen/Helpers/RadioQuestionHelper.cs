﻿using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Persistence;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class RadioQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;

        public RadioQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowInstanceStore workflowInstanceStore)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowInstanceStore = workflowInstanceStore;
        }


        public async Task<bool> AnswerEquals(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string choiceIdToCheck)
        {
            bool result = false;
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);
            if (questionScreenAnswer != null && questionScreenAnswer.QuestionType == QuestionTypeConstants.RadioQuestion)
            {
                var choices = questionScreenAnswer.Choices;

                var singleChoice = choices.FirstOrDefault(x => x.Answer == questionScreenAnswer.Answer);

                if (singleChoice != null && choiceIdToCheck.Contains(singleChoice.Identifier))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return result;
        }

        public async Task<bool> AnswerIn(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            bool result = false;
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);
            if (questionScreenAnswer != null && questionScreenAnswer.QuestionType == QuestionTypeConstants.RadioQuestion)
            {
                var choices = questionScreenAnswer.Choices;

                foreach (var item in choiceIdsToCheck)
                {
                    var singleChoice = choices.FirstOrDefault(x => x.Identifier == item);
                    if (singleChoice != null)
                    {
                        var answerCheck = choices.Select(x => x.Identifier).Contains(item) && singleChoice.Answer == questionScreenAnswer.Answer;

                        if (answerCheck)
                        {
                            return true;
                        }
                        else
                        {
                            result = false;
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
            engine.SetValue("radioQuestionAnswerEquals", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, choiceIdToCheck) => AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, choiceIdToCheck).Result));
            engine.SetValue("radioQuestionAnswerIn", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerIn(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck).Result));
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
