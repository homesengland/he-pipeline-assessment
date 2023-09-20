using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;
using System.Globalization;

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


        public async Task<bool> AnswerEqualToOrGreaterThan(string correlationId, string workflowName, string activityName, string questionId, int day, int month, int year)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            if (question != null && question.Answers != null &&
                        question.QuestionType == QuestionTypeConstants.DateQuestion)
            {
                var answer = question.Answers.FirstOrDefault();
                if (answer != null)
                {
                    DateTime.TryParseExact(answer.AnswerText, CustomWorkflow.Sdk.Constants.DateFormat,
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

            return false;
        }

        public async Task<bool> AnswerEqualToOrLessThan(string correlationId, string workflowName, string activityName, string questionId, int day, int month, int year)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            if (question != null && question.Answers != null &&
                        question.QuestionType == QuestionTypeConstants.DateQuestion)
            {
                var answer = question.Answers.FirstOrDefault();
                if (answer != null)
                {
                    DateTime.TryParseExact(answer.AnswerText, CustomWorkflow.Sdk.Constants.DateFormat,
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

            return false;
        }


        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("dateQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, int, int, int, bool>)((workflowName, activityName, questionId, day, month, year) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, day, month, year).Result));
            engine.SetValue("dateQuestionAnswerEqualToOrLessThan", (Func<string, string, string, int, int, int, bool>)((workflowName, activityName, questionId, day, month, year) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, day, month, year).Result));
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
