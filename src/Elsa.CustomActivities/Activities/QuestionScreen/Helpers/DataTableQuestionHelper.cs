﻿using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Elsa.CustomActivities.Activities.Common;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{

    public class DataTableQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public DataTableQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }

        public async Task<decimal?> GetDecimalAnswer(string correlationId, string workflowName, string activityName, string questionId, string tableCellIdentifier)
        {
            try
            {
                var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                    workflowName, correlationId, questionId, CancellationToken.None);

                if (question != null && question.Answers != null &&
                    question.QuestionType == QuestionTypeConstants.DataTable)
                {
                    var answer = question.Answers.FirstOrDefault();
                    if (answer != null)
                    {
                        var dataTable = JsonSerializer.Deserialize<DataTable>(answer.AnswerText);
                        if (dataTable != null &&
                            (dataTable.InputType == DataTableInputTypeConstants.CurrencyDataTableInput ||
                             dataTable.InputType == DataTableInputTypeConstants.IntegerDataTableInput ||
                             dataTable.InputType == DataTableInputTypeConstants.DecimalDataTableInput))
                        {
                            var input = dataTable.Inputs.FirstOrDefault(
                                x => x.Identifier == tableCellIdentifier);

                            if (input != null && input.Input != null)
                            {
                                var answerText = decimal.Parse(input.Input);
                                return answerText;
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public async Task<decimal?[]> GetDecimalAnswerArray(string correlationId, string workflowName, string activityName, string questionId)
        {
            try
            {
                var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                    workflowName, correlationId, questionId, CancellationToken.None);

                if (question != null && question.Answers != null &&
                    question.QuestionType == QuestionTypeConstants.DataTable)
                {
                    var answer = question.Answers.FirstOrDefault();
                    if (answer != null)
                    {
                        var dataTable = JsonSerializer.Deserialize<DataTable>(answer.AnswerText);
                        if (dataTable != null &&
                            (dataTable.InputType == DataTableInputTypeConstants.CurrencyDataTableInput ||
                             dataTable.InputType == DataTableInputTypeConstants.IntegerDataTableInput ||
                             dataTable.InputType == DataTableInputTypeConstants.DecimalDataTableInput))
                        {
                            var input = dataTable.Inputs.Select(x => x.Input);

                            var returnList = new List<decimal?>();
                            foreach (var item in input)
                            {
                                if (item != null)
                                {
                                    var answerDecimal = decimal.Parse(item);
                                    returnList.Add(answerDecimal);
                                }
                                else
                                {
                                    returnList.Add(null);
                                }
                            }

                            return returnList.ToArray();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<decimal?>().ToArray();
            }

            return new List<decimal?>().ToArray();
        }

        public async Task<string> GetStringAnswer(string correlationId, string workflowName, string activityName, string questionId, string tableCellIdentifier)
        {
            try
            {
                var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                    workflowName, correlationId, questionId, CancellationToken.None);

                if (question != null && question.Answers != null && question.QuestionType == QuestionTypeConstants.DataTable)
                {
                    var answer = question.Answers.FirstOrDefault();
                    if (answer != null)
                    {
                        var dataTable = JsonSerializer.Deserialize<DataTable>(answer.AnswerText);
                        if (dataTable != null)
                        {
                            var input = dataTable.Inputs.FirstOrDefault(x => x.Identifier == tableCellIdentifier);

                            if (input != null && input.Input != null)
                            {
                                return input.Input;
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        public async Task<string?[]> GetStringAnswerArray(string correlationId, string workflowName, string activityName, string questionId)
        {
            try
            {
                var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                    workflowName, correlationId, questionId, CancellationToken.None);

                if (question != null && question.Answers != null && question.QuestionType == QuestionTypeConstants.DataTable)
                {
                    var answer = question.Answers.FirstOrDefault();
                    if (answer != null)
                    {
                        var dataTable = JsonSerializer.Deserialize<DataTable>(answer.AnswerText);
                        if (dataTable != null)
                        {
                            var input = dataTable.Inputs.Select(x => x.Input).ToArray();
                            return input;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new string[] { };
            }

            return new string[] { };
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;

            engine.SetValue("dataTableQuestionGetStringAnswer", (Func<string, string, string, string, string?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetStringAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));
            engine.SetValue("dataTableQuestionGetStringAnswerArray", (Func<string, string, string, string?[]>)((workflowName, activityName, questionId) => GetStringAnswerArray(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));

            engine.SetValue("dataTableQuestionGetCurrencyAnswerArray", (Func<string, string, string, decimal?[]>)((workflowName, activityName, questionId) => GetDecimalAnswerArray(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("dataTableQuestionGetCurrencyAnswer", (Func<string, string, string, string, decimal?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));

            engine.SetValue("dataTableQuestionGetIntegerAnswerArray", (Func<string, string, string, decimal?[]>)((workflowName, activityName, questionId) => GetDecimalAnswerArray(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("dataTableQuestionGetIntegerAnswer", (Func<string, string, string, string, decimal?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));

            engine.SetValue("dataTableQuestionGetDecimalAnswerArray", (Func<string, string, string, decimal?[]>)((workflowName, activityName, questionId) => GetDecimalAnswerArray(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("dataTableQuestionGetDecimalAnswer", (Func<string, string, string, string, decimal?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));

            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare function dataTableQuestionGetStringAnswerArray(workflowName: string, activityName:string, questionId:string): [];");
            output.AppendLine("declare function dataTableQuestionGetStringAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): string;");

            output.AppendLine("declare function dataTableQuestionGetCurrencyAnswerArray(workflowName: string, activityName:string, questionId:string ): [];");
            output.AppendLine("declare function dataTableQuestionGetCurrencyAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): number;");

            output.AppendLine("declare function dataTableQuestionGetIntegerAnswerArray(workflowName: string, activityName:string, questionId:string ): [];");
            output.AppendLine("declare function dataTableQuestionGetIntegerAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): number;");

            output.AppendLine("declare function dataTableQuestionGetDecimalAnswerArray(workflowName: string, activityName:string, questionId:string ): [];");
            output.AppendLine("declare function dataTableQuestionGetDecimalAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): number;");

            return Task.CompletedTask;
        }
    }
}
