using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;
using System.Text.Json;
using Elsa.CustomActivities.Activities.Common;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{

    public class DataTableQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;

        public DataTableQuestionHelper(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<decimal?> GetDecimalAnswer(string correlationId, string workflowName, string activityName, string questionId, string tableCellIdentifier)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);
            return GetDecimalAnswer(tableCellIdentifier, question);
        }

        public async Task<decimal?> GetDecimalAnswer(string correlationId, int dataDictionaryId, string tableCellIdentifier)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);

            return GetDecimalAnswer(tableCellIdentifier, question);
        }

        private static decimal? GetDecimalAnswer(string tableCellIdentifier, CustomModels.Question? question)
        {
            try
            {
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
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);
            return GetDecimalAnswerArray(question);
        }

        public async Task<decimal?[]> GetDecimalAnswerArray(string correlationId, int dataDictionaryId)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);

            return GetDecimalAnswerArray(question);
        }

        private static decimal?[] GetDecimalAnswerArray(CustomModels.Question? question)
        {
            try
            {
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
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            return GetStringAnswer(tableCellIdentifier, question);
        }

        public async Task<string> GetStringAnswer(string correlationId, int dataDictionaryId, string tableCellIdentifier)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);

            return GetStringAnswer(tableCellIdentifier, question);
        }

        private static string GetStringAnswer(string tableCellIdentifier, CustomModels.Question? question)
        {
            try
            {
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
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            return GetStringAnswerArray(question);
        }
        public async Task<string?[]> GetStringAnswerArray(string correlationId, int dataDictionaryId)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);

            return GetStringAnswerArray(question);
        }

        private static string?[] GetStringAnswerArray(CustomModels.Question? question)
        {
            try
            {
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
            engine.SetValue("dataTableGetStringAnswer", (Func<int, string, string?>)((dataDictionaryId, tableCellIdentifier) => GetStringAnswer(activityExecutionContext.CorrelationId, dataDictionaryId, tableCellIdentifier).Result));
            engine.SetValue("dataTableGetStringAnswerArray", (Func<int, string?[]>)((dataDictionaryId) => GetStringAnswerArray(activityExecutionContext.CorrelationId, dataDictionaryId).Result));

            engine.SetValue("dataTableQuestionGetCurrencyAnswerArray", (Func<string, string, string, decimal?[]>)((workflowName, activityName, questionId) => GetDecimalAnswerArray(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("dataTableQuestionGetCurrencyAnswer", (Func<string, string, string, string, decimal?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));
            engine.SetValue("dataTableGetCurrencyAnswerArray", (Func<int, decimal?[]>)((dataDictionaryId) => GetDecimalAnswerArray(activityExecutionContext.CorrelationId, dataDictionaryId).Result));
            engine.SetValue("dataTableGetCurrencyAnswer", (Func<int, string, decimal?>)((dataDictionaryId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, dataDictionaryId, tableCellIdentifier).Result));

            engine.SetValue("dataTableQuestionGetIntegerAnswerArray", (Func<string, string, string, decimal?[]>)((workflowName, activityName, questionId) => GetDecimalAnswerArray(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("dataTableQuestionGetIntegerAnswer", (Func<string, string, string, string, decimal?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));
            engine.SetValue("dataTableGetIntegerAnswerArray", (Func<int, decimal?[]>)((dataDictionaryId) => GetDecimalAnswerArray(activityExecutionContext.CorrelationId, dataDictionaryId).Result));
            engine.SetValue("dataTableGetIntegerAnswer", (Func<int, string, decimal?>)((dataDictionaryId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, dataDictionaryId, tableCellIdentifier).Result));

            engine.SetValue("dataTableQuestionGetDecimalAnswerArray", (Func<string, string, string, decimal?[]>)((workflowName, activityName, questionId) => GetDecimalAnswerArray(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("dataTableQuestionGetDecimalAnswer", (Func<string, string, string, string, decimal?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));
            engine.SetValue("dataTableGetDecimalAnswerArray", (Func<int, decimal?[]>)((dataDictionaryId) => GetDecimalAnswerArray(activityExecutionContext.CorrelationId, dataDictionaryId).Result));
            engine.SetValue("dataTableGetDecimalAnswer", (Func<int, string, decimal?>)((dataDictionaryId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, dataDictionaryId, tableCellIdentifier).Result));

            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare function dataTableQuestionGetStringAnswerArray(workflowName: string, activityName:string, questionId:string): [];");
            output.AppendLine("declare function dataTableQuestionGetStringAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): string;");
            output.AppendLine("declare function dataTableGetStringAnswerArray(dataDictionaryId: number): [];");
            output.AppendLine("declare function dataTableGetStringAnswer(dataDictionaryId: number, tableCellIdentifier:string ): string;");

            output.AppendLine("declare function dataTableQuestionGetCurrencyAnswerArray(workflowName: string, activityName:string, questionId:string ): [];");
            output.AppendLine("declare function dataTableQuestionGetCurrencyAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): number;");
            output.AppendLine("declare function dataTableGetCurrencyAnswerArray(dataDictionaryId: number): [];");
            output.AppendLine("declare function dataTableGetCurrencyAnswer(dataDictionaryId: number, tableCellIdentifier:string ): number;");

            output.AppendLine("declare function dataTableQuestionGetIntegerAnswerArray(workflowName: string, activityName:string, questionId:string ): [];");
            output.AppendLine("declare function dataTableQuestionGetIntegerAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): number;");
            output.AppendLine("declare function dataTableGetIntegerAnswerArray(dataDictionaryId: number): [];");
            output.AppendLine("declare function dataTableGetIntegerAnswer(dataDictionaryId: number, tableCellIdentifier:string ): number;");

            output.AppendLine("declare function dataTableQuestionGetDecimalAnswerArray(workflowName: string, activityName:string, questionId:string ): [];");
            output.AppendLine("declare function dataTableQuestionGetDecimalAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): number;");
            output.AppendLine("declare function dataTableGetDecimalAnswerArray(dataDictionaryId: number): [];");
            output.AppendLine("declare function dataTableGetDecimalAnswer(dataDictionaryId: number, tableCellIdentifier:string ): number;");

            return Task.CompletedTask;
        }
    }
}
