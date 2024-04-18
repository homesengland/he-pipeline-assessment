﻿using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class NumericQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public NumericQuestionHelper(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<bool> AnswerEqualToOrGreaterThan(string correlationId, string workflowName, string activityName, string questionId, decimal answerToCheck)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
            workflowName, correlationId, questionId, CancellationToken.None);

            return AnswerEqualToOrGreaterThan(answerToCheck, question);
        }

        public async Task<bool> AnswerEqualToOrGreaterThan(string correlationId, int dataDictionaryId, decimal answerToCheck)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                   CancellationToken.None);

            return AnswerEqualToOrGreaterThan(answerToCheck, question);
        }

        private static bool AnswerEqualToOrGreaterThan(decimal answerToCheck, CustomModels.Question? question)
        {
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

            return AnswerEqualToOrLessThan(answerToCheck, question);
        }

        public async Task<bool> AnswerEqualToOrLessThan(string correlationId, int dataDictionaryId, decimal answerToCheck)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);

            return AnswerEqualToOrLessThan(answerToCheck, question);
        }

        private static bool AnswerEqualToOrLessThan(decimal answerToCheck, CustomModels.Question? question)
        {
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

            return GetDecimalAnswer(question);
        }

        public async Task<decimal?> GetDecimalAnswer(string correlationId, int dataDictionaryId)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);

            return GetDecimalAnswer(question);
        }

        private static decimal? GetDecimalAnswer(CustomModels.Question? question)
        {
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
            engine.SetValue("currencyAnswerEqualToOrGreaterThan", (Func<int, decimal, bool>)((dataDictionaryId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, dataDictionaryId, answerToCheck).Result));
            engine.SetValue("currencyAnswerEqualToOrLessThan", (Func<int, decimal, bool>)((dataDictionaryId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, dataDictionaryId, answerToCheck).Result));
            engine.SetValue("currencyGetAnswer", (Func<int, decimal?>)((dataDictionaryId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, dataDictionaryId).Result));

            engine.SetValue("decimalQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("decimalQuestionAnswerEqualToOrLessThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("decimalQuestionGetAnswer", (Func<string, string, string, decimal?>)((workflowName, activityName, questionId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("decimalAnswerEqualToOrGreaterThan", (Func<int, decimal, bool>)((dataDictionaryId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, dataDictionaryId, answerToCheck).Result));
            engine.SetValue("decimalAnswerEqualToOrLessThan", (Func<int, decimal, bool>)((dataDictionaryId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, dataDictionaryId, answerToCheck).Result));
            engine.SetValue("decimalGetAnswer", (Func<int, decimal?>)((dataDictionaryId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, dataDictionaryId).Result));

            engine.SetValue("percentageQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("percentageQuestionAnswerEqualToOrLessThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("percentageQuestionGetAnswer", (Func<string, string, string, decimal?>)((workflowName, activityName, questionId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("percentageAnswerEqualToOrGreaterThan", (Func<int, decimal, bool>)((dataDictionaryId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, dataDictionaryId, answerToCheck).Result));
            engine.SetValue("percentageAnswerEqualToOrLessThan", (Func<int, decimal, bool>)((dataDictionaryId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, dataDictionaryId, answerToCheck).Result));
            engine.SetValue("percentageGetAnswer", (Func<int, decimal?>)((dataDictionaryId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, dataDictionaryId).Result));

            engine.SetValue("integerQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("integerQuestionAnswerEqualToOrLessThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("integerQuestionGetAnswer", (Func<string, string, string, decimal?>)((workflowName, activityName, questionId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("integerAnswerEqualToOrGreaterThan", (Func<int, decimal, bool>)((dataDictionaryId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.CorrelationId, dataDictionaryId, answerToCheck).Result));
            engine.SetValue("integerAnswerEqualToOrLessThan", (Func<int, decimal, bool>)((dataDictionaryId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.CorrelationId, dataDictionaryId, answerToCheck).Result));
            engine.SetValue("integerGetAnswer", (Func<int, decimal?>)((dataDictionaryId) => GetDecimalAnswer(activityExecutionContext.CorrelationId, dataDictionaryId).Result));

            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function currencyQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function currencyQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function currencyQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): number;");
            output.AppendLine("declare function currencyAnswerEqualToOrGreaterThan(dataDictionaryId: number, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function currencyAnswerEqualToOrLessThan(dataDictionaryId: number, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function currencyGetAnswer(dataDictionaryId: number ): number;");

            output.AppendLine("declare function decimalQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function decimalQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function decimalQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): number;");
            output.AppendLine("declare function decimalAnswerEqualToOrGreaterThan(dataDictionaryId: number, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function decimalAnswerEqualToOrLessThan(dataDictionaryId: number, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function decimalGetAnswer(dataDictionaryId: number ): number;");

            output.AppendLine("declare function percentageQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function percentageQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function percentageQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): number;");
            output.AppendLine("declare function percentageAnswerEqualToOrGreaterThan(dataDictionaryId: number, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function percentageAnswerEqualToOrLessThan(dataDictionaryId: number, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function percentageGetAnswer(dataDictionaryId: number): number;");

            output.AppendLine("declare function integerQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function integerQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function integerQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): number;");
            output.AppendLine("declare function integerAnswerEqualToOrGreaterThan(dataDictionaryId: number, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function integerAnswerEqualToOrLessThan(dataDictionaryId: number, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function integerGetAnswer(dataDictionaryId: number): number;");

            return Task.CompletedTask;
        }
    }
}
