﻿using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;
using System.Globalization;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class DateQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;

        public DateQuestionHelper(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }


        public async Task<bool> AnswerEqualToOrGreaterThan(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, int day, int month, int year)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            if (questionScreenAnswer != null && questionScreenAnswer.Answer != null && questionScreenAnswer.QuestionType == QuestionTypeConstants.DateQuestion)
            {
                DateTime.TryParseExact(questionScreenAnswer.Answer, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime answerDate);

                var dateString =
                    $"{year}-{month}-{day}";
                DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime answerToCheckDateTime);

                if (answerDate >= answerToCheckDateTime)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> AnswerEqualToOrLessThan(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, int day, int month, int year)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            if (questionScreenAnswer != null && questionScreenAnswer.Answer != null && questionScreenAnswer.QuestionType == QuestionTypeConstants.DateQuestion)
            {
                DateTime.TryParseExact(questionScreenAnswer.Answer, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime answerDate);

                var dateString =
                    $"{year}-{month}-{day}";
                DateTime.TryParseExact(dateString, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime answerToCheckDateTime);

                if (answerDate <= answerToCheckDateTime)
                {
                    return true;
                }
            }

            return false;
        }


        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("dateQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, int, int, int, bool>)((workflowName, activityName, questionId, day, month, year) => AnswerEqualToOrGreaterThan(activityExecutionContext, workflowName, activityName, questionId, day, month, year).Result));
            engine.SetValue("dateQuestionAnswerEqualToOrLessThan", (Func<string, string, string, int, int, int, bool>)((workflowName, activityName, questionId, day, month, year) => AnswerEqualToOrLessThan(activityExecutionContext, workflowName, activityName, questionId, day, month, year).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function dateQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function dateQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            return Task.CompletedTask;
        }
    }
}