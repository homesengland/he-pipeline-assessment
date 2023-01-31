using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;
using System.Globalization;
using Elsa.CustomWorkflow.Sdk;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class DateQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public DateQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }


        public async Task<bool> AnswerEqualToOrGreaterThan(string WorkflowInstance, string workflowName, string activityName, string questionId, int day, int month, int year)
        {
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activity.Id,
                        WorkflowInstance, questionId, CancellationToken.None);

                    if (questionScreenAnswer != null && questionScreenAnswer.Answer != null &&
                        questionScreenAnswer.QuestionType == QuestionTypeConstants.DateQuestion)
                    {
                        DateTime.TryParseExact(questionScreenAnswer.Answer, CustomWorkflow.Sdk.Constants.DateFormat,
                            CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime answerDate);

                        var dateString =
                            $"{year}-{month}-{day}";
                        DateTime.TryParseExact(dateString, CustomWorkflow.Sdk.Constants.DateFormat, CultureInfo.InvariantCulture,
                            DateTimeStyles.AdjustToUniversal, out DateTime answerToCheckDateTime);

                        if (answerDate >= answerToCheckDateTime)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public async Task<bool> AnswerEqualToOrLessThan(string WorkflowInstance, string workflowName, string activityName, string questionId, int day, int month, int year)
        {
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activity.Id,
                        WorkflowInstance, questionId, CancellationToken.None);

                    if (questionScreenAnswer != null && questionScreenAnswer.Answer != null &&
                        questionScreenAnswer.QuestionType == QuestionTypeConstants.DateQuestion)
                    {
                        DateTime.TryParseExact(questionScreenAnswer.Answer, CustomWorkflow.Sdk.Constants.DateFormat,
                            CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime answerDate);

                        var dateString =
                            $"{year}-{month}-{day}";
                        DateTime.TryParseExact(dateString, CustomWorkflow.Sdk.Constants.DateFormat, CultureInfo.InvariantCulture,
                            DateTimeStyles.AdjustToUniversal, out DateTime answerToCheckDateTime);

                        if (answerDate <= answerToCheckDateTime)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("dateQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, int, int, int, bool>)((workflowName, activityName, questionId, day, month, year) => AnswerEqualToOrGreaterThan(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, day, month, year).Result));
            engine.SetValue("dateQuestionAnswerEqualToOrLessThan", (Func<string, string, string, int, int, int, bool>)((workflowName, activityName, questionId, day, month, year) => AnswerEqualToOrLessThan(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, day, month, year).Result));
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
