using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class NumericQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public NumericQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }


        public async Task<bool> AnswerEqualToOrGreaterThan(string correlationId, string workflowName, string activityName, string questionId, decimal answerToCheck)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            if (question != null && question.Answers != null &&
                        (question.QuestionType == QuestionTypeConstants.CurrencyQuestion ||
                         question.QuestionType == QuestionTypeConstants.DecimalQuestion ||
                         question.QuestionType == QuestionTypeConstants.PercentageQuestion ||
                         question.QuestionType == QuestionTypeConstants.IntegerQuestion
                        ))
            {
                var answer = question.Answers.FirstOrDefault();
                if (answer != null)
                {
                    var answerText = decimal.Parse(answer.AnswerText);
                    if (answerText >= answerToCheck)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<bool> AnswerEqualToOrLessThan(string correlationId, string workflowName, string activityName, string questionId, decimal answerToCheck)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            if (question != null && question.Answers != null &&
                        (
                        question.QuestionType == QuestionTypeConstants.CurrencyQuestion ||
                        question.QuestionType == QuestionTypeConstants.DecimalQuestion ||
                        question.QuestionType == QuestionTypeConstants.PercentageQuestion ||
                        question.QuestionType == QuestionTypeConstants.IntegerQuestion))
            {
                var answer = question.Answers.FirstOrDefault();
                if (answer != null)
                {
                    var answerText = decimal.Parse(answer.AnswerText);
                    if (answerText <= answerToCheck)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<decimal?> GetDecimalAnswer(string correlationId, string workflowName, string activityName, string questionId)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            if (question != null && question.Answers != null &&
                        (
                            question.QuestionType == QuestionTypeConstants.CurrencyQuestion ||
                            question.QuestionType == QuestionTypeConstants.DecimalQuestion ||
                            question.QuestionType == QuestionTypeConstants.PercentageQuestion ||
                            question.QuestionType == QuestionTypeConstants.IntegerQuestion))
            {
                var answer = question.Answers.FirstOrDefault();
                if (answer != null)
                {
                    var answerText = decimal.Parse(answer.AnswerText);
                    return answerText;
                }
            }
            return null;
        }




        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;

            engine.SetValue("currencyQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("currencyQuestionAnswerEqualToOrLessThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("currencyQuestionGetAnswer", (Func<string, string, string, decimal?>)((workflowName, activityName, questionId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));

            engine.SetValue("decimalQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("decimalQuestionAnswerEqualToOrLessThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("decimalQuestionGetAnswer", (Func<string, string, string, decimal?>)((workflowName, activityName, questionId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));

            engine.SetValue("percentageQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("percentageQuestionAnswerEqualToOrLessThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("percentageQuestionGetAnswer", (Func<string, string, string, decimal?>)((workflowName, activityName, questionId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));

            engine.SetValue("integerQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("integerQuestionAnswerEqualToOrLessThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("integerQuestionGetAnswer", (Func<string, string, string, decimal?>)((workflowName, activityName, questionId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));

            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function currencyQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function currencyQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function currencyQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): number;");

            output.AppendLine("declare function decimalQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function decimalQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function decimalQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): number;");

            output.AppendLine("declare function percentageQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function percentageQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function percentageQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): number;");

            output.AppendLine("declare function integerQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function integerQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function integerQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): number;");

            return Task.CompletedTask;
        }
    }
}
