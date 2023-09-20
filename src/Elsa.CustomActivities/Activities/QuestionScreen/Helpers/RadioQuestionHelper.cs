using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class RadioQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ILogger<RadioQuestionHelper> _logger;
        private readonly Random random = new Random();

        public RadioQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry, ILogger<RadioQuestionHelper> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
            _logger = logger;
        }


        public async Task<bool> AnswerEquals(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string choiceIdToCheck)
        {
            bool result = false;

            var randomId = random.Next();

            activityExecutionContext.JournalData.Add($"Running RadioQuestionAnswerEquals for choice {choiceIdToCheck}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdToCheck}");
            _logger.LogWarning($"Running RadioQuestionAnswerEquals - CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdToCheck}");


            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            if (question != null &&
                        (question.QuestionType == QuestionTypeConstants.RadioQuestion || question.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion || question.QuestionType == QuestionTypeConstants.WeightedRadioQuestion))
            {
                activityExecutionContext.JournalData.Add($"RadioQuestionAnswerEquals Question Found for choice {choiceIdToCheck}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}");
                _logger.LogWarning($"RadioQuestionAnswerEquals Question Found - CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}");

                if (question.Answers != null && question.Answers.Count == 1)
                {
                    var singleAnswer = question.Answers.First();
                    activityExecutionContext.JournalData.Add($"RadioQuestionAnswerEquals Answers Found for choice {choiceIdToCheck}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}, Answer: {question.Answers.First().AnswerText}, Choice: {question.Answers.First().Choice?.Identifier}");
                    _logger.LogWarning($"RadioQuestionAnswerEquals Answers Found - CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}, Answer: {question.Answers.First().AnswerText}, Choice: {question.Answers.First().Choice?.Identifier}");

                    return choiceIdToCheck == singleAnswer.Choice?.Identifier;
                }

                return false;
            }
            else
            {
                activityExecutionContext.JournalData.Add($"RadioQuestionAnswerEquals Question NULL for choice {choiceIdToCheck}. (random id - {randomId})", $"CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdToCheck}");
                _logger.LogWarning($"RadioQuestionAnswerEquals Question NULL possibly - CorrelationID: {activityExecutionContext.CorrelationId}, WorkflowName: {workflowName}, ActivityName: {activityName}, QuestionID: {questionId}, ChoicesToCheck: {choiceIdToCheck}");
            }

            return result;
        }

        public async Task<bool> AnswerIn(string correlationId, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            bool result = false;
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);
            if (question != null &&
                        (question.QuestionType == QuestionTypeConstants.RadioQuestion || question.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion || question.QuestionType == QuestionTypeConstants.WeightedRadioQuestion))
            {
                var choices = question.Choices;

                if (choices != null && question.Answers != null && question.Answers.Count == 1)
                {
                    var singleAnswer = question.Answers.First();
                    return choiceIdsToCheck.Contains(singleAnswer.Choice?.Identifier);
                }
            }
            return result;
        }



        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("radioQuestionAnswerEquals", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, choiceIdToCheck) => AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, choiceIdToCheck).Result));
            engine.SetValue("radioQuestionAnswerIn", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerIn(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, choiceIdsToCheck).Result));
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
