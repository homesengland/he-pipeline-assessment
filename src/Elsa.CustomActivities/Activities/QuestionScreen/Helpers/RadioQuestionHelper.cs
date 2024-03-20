using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class RadioQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<RadioQuestionHelper> _logger;
        private readonly Random random = new Random();

        public RadioQuestionHelper(IElsaCustomRepository elsaCustomRepository, ILogger<RadioQuestionHelper> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }


        public async Task<bool> AnswerEquals(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string choiceIdToCheck)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            return AnswerEquals(activityExecutionContext, choiceIdToCheck, question);
        }

        public async Task<bool> AnswerEquals(ActivityExecutionContext activityExecutionContext, int dataDictionaryId, string choiceIdToCheck)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId,
                    dataDictionaryId,
                    CancellationToken.None);

            return AnswerEquals(activityExecutionContext, choiceIdToCheck, question);
        }

        private bool AnswerEquals(ActivityExecutionContext activityExecutionContext, string choiceIdToCheck, CustomModels.Question? question)
        {
            bool result = false;

            var randomId = random.Next();

            activityExecutionContext.JournalData.Add(
                $"Running RadioQuestionAnswerEquals for choice {choiceIdToCheck}. (random id - {randomId})",
                $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdToCheck}");
            _logger.LogWarning(
                $"Running RadioQuestionAnswerEquals - CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdToCheck}");


            if (question != null &&
                (question.QuestionType == QuestionTypeConstants.RadioQuestion ||
                 question.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion ||
                 question.QuestionType == QuestionTypeConstants.WeightedRadioQuestion))
            {
                activityExecutionContext.JournalData.Add(
                    $"RadioQuestionAnswerEquals Question Found for choice {choiceIdToCheck}. (random id - {randomId})",
                    $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck:  {choiceIdToCheck}");
                _logger.LogWarning(
                    $"RadioQuestionAnswerEquals Question Found - CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck:  {choiceIdToCheck}");

                if (question.Answers != null && question.Answers.Count == 1)
                {
                    var singleAnswer = question.Answers.First();
                    activityExecutionContext.JournalData.Add(
                        $"RadioQuestionAnswerEquals Answers Found for choice {choiceIdToCheck}. (random id - {randomId})",
                        $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck:  {choiceIdToCheck}");
                    _logger.LogWarning(
                        $"RadioQuestionAnswerEquals Answers Found - CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck:   {choiceIdToCheck}");

                    return choiceIdToCheck == singleAnswer.Choice?.Identifier;
                }

                return false;
            }
            else
            {
                activityExecutionContext.JournalData.Add(
                    $"RadioQuestionAnswerEquals Question NULL for choice {choiceIdToCheck}. (random id - {randomId})",
                    $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdToCheck}");
                _logger.LogWarning(
                    $"RadioQuestionAnswerEquals Question NULL possibly - CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdToCheck}");
            }

            return result;
        }

        public async Task<bool> AnswerIn(string correlationId, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            return AnswerIn(choiceIdsToCheck, question);
        }

        public async Task<bool> AnswerIn(string correlationId, int dataDictionaryId, string[] choiceIdsToCheck)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);

            return AnswerIn(choiceIdsToCheck, question);
        }

        private static bool AnswerIn(string[] choiceIdsToCheck, CustomModels.Question? question)
        {
            bool result = false;
            if (question != null &&
                (question.QuestionType == QuestionTypeConstants.RadioQuestion ||
                 question.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion ||
                 question.QuestionType == QuestionTypeConstants.WeightedRadioQuestion))
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
            engine.SetValue("radioAnswerEquals", (Func<int, string, bool>)((dataDictionaryId, choiceIdToCheck) => AnswerEquals(activityExecutionContext, dataDictionaryId, choiceIdToCheck).Result));
            engine.SetValue("radioAnswerIn", (Func<int, string[], bool>)((dataDictionaryId, choiceIdsToCheck) => AnswerIn(activityExecutionContext.CorrelationId, dataDictionaryId, choiceIdsToCheck).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function radioQuestionAnswerEquals(workflowName: string, activityName:string, questionId:string, choiceIdToCheck:string ): boolean;");
            output.AppendLine("declare function radioQuestionAnswerIn(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[]  ): boolean;");
            output.AppendLine("declare function radioAnswerEquals(dataDictionaryId: number, choiceIdToCheck:string ): boolean;");
            output.AppendLine("declare function radioAnswerIn(dataDictionaryId: number, choiceIdsToCheck:string[]  ): boolean;");
            return Task.CompletedTask;
        }
    }
}
