using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
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


        public async Task<bool> AnswerEqualToOrGreaterThan(string correlationId, string workflowName, string activityName, string questionId, int day, int month, int year)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            return AnswerEqualToOrGreaterThan(day, month, year, question);
        }

        public async Task<bool> AnswerEqualToOrGreaterThan(string correlationId, int dataDictionaryId, int day, int month, int year)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);

            return AnswerEqualToOrGreaterThan(day, month, year, question);
        }

        private static bool AnswerEqualToOrGreaterThan(int day, int month, int year, CustomModels.Question? question)
        {
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

            return AnswerEqualToOrLessThan(day, month, year, question);
        }

        public async Task<bool> AnswerEqualToOrLessThan(string correlationId, int dataDictionaryId, int day, int month, int year)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);

            return AnswerEqualToOrLessThan(day, month, year, question);
        }

        private static bool AnswerEqualToOrLessThan(int day, int month, int year, CustomModels.Question? question)
        {
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
            engine.SetValue("dateAnswerEqualToOrGreaterThan", (Func<int, int, int, int, bool>)((dataDictionaryId, day, month, year) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, dataDictionaryId, day, month, year).Result));
            engine.SetValue("dateAnswerEqualToOrLessThan", (Func<int, int, int, int, bool>)((dataDictionaryId, day, month, year) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, dataDictionaryId, day, month, year).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function dateQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, day:number, month: number, year:number ): boolean;");
            output.AppendLine("declare function dateQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, day:number, month: number, year:number ): boolean;");
            output.AppendLine("declare function dateAnswerEqualToOrGreaterThan(dataDictionaryId: number, day:number, month: number, year:number ): boolean;");
            output.AppendLine("declare function dateAnswerEqualToOrLessThan(dataDictionaryId: number, day:number, month: number, year:number ): boolean;");
            return Task.CompletedTask;
        }
    }
}
