using Elsa.CustomInfrastructure.Data.Repository;
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
                var workflowBlueprint =
                    await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

                if (workflowBlueprint != null)
                {
                    var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                    if (activity != null)
                    {
                        var question = await _elsaCustomRepository.GetQuestionByCorrelationId(activity.Id,
                            correlationId, questionId, CancellationToken.None);

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
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public async Task<string> GetStringAnswer(string correlationId, string workflowName, string activityName, string questionId, string tableCellIdentifier)
        {
            try
            {
                var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

                if (workflowBlueprint != null)
                {
                    var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                    if (activity != null)
                    {
                        var question = await _elsaCustomRepository.GetQuestionByCorrelationId(activity.Id, correlationId, questionId, CancellationToken.None);

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
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("dataTableQuestionGetStringAnswer", (Func<string, string, string, string, string?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetStringAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));
            engine.SetValue("dataTableQuestionGetCurrencyAnswer", (Func<string, string, string, string, decimal?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));
            engine.SetValue("dataTableQuestionGetIntegerAnswer", (Func<string, string, string, string, decimal?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));
            engine.SetValue("dataTableQuestionGetDecimalAnswer", (Func<string, string, string, string, decimal?>)((workflowName, activityName, questionId, tableCellIdentifier) => GetDecimalAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, tableCellIdentifier).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function dataTableQuestionGetStringAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): string;");
            output.AppendLine("declare function dataTableQuestionGetCurrencyAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): number;");
            output.AppendLine("declare function dataTableQuestionGetIntegerAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): number;");
            output.AppendLine("declare function dataTableQuestionGetDecimalAnswer(workflowName: string, activityName:string, questionId:string, tableCellIdentifier:string ): number;");
            return Task.CompletedTask;
        }
    }
}
