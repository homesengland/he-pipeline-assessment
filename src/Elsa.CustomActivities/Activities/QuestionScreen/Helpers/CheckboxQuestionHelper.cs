using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class CheckboxQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly Random random = new Random();

        public CheckboxQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }

        public async Task<string> GetAnswer(string correlationId, string workflowName, string activityName, string questionId)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);

            return GetAnswer(question);
        }

        public async Task<string> GetAnswer(string correlationId, int dataDictionaryId)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);

            return GetAnswer(question);
        }

        private static string GetAnswer(CustomModels.Question? question)
        {
            if (question != null && question.Answers != null &&
                (question.QuestionType == QuestionTypeConstants.CheckboxQuestion ||
                 question.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion))
            {
                return string.Join(",", question.Answers.Select(x => x.AnswerText));
            }

            return string.Empty;
        }

        public async Task<bool> AnswerEquals(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);
            return AnswerEquals(activityExecutionContext, choiceIdsToCheck, question);
        }

        public async Task<bool> AnswerEquals(ActivityExecutionContext activityExecutionContext, int dataDictionaryId, string[] choiceIdsToCheck)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId,
                    CancellationToken.None);

            return AnswerEquals(activityExecutionContext, choiceIdsToCheck, question);
        }

        private bool AnswerEquals(ActivityExecutionContext activityExecutionContext, string[] choiceIdsToCheck, CustomModels.Question? question)
        {
            bool result = false;
            var randomId = random.Next();
            var choiceIdsString = string.Join(", ", choiceIdsToCheck);
            if (question != null &&
                (question.QuestionType == QuestionTypeConstants.CheckboxQuestion ||
                 question.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion) &&
                question.Answers != null)
            {
                activityExecutionContext.JournalData.Add(
                    $"checkboxQuestionAnswerEquals Question Found for choices {choiceIdsString}. (random id - {randomId})",
                    $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}");
                if (question.Answers.Count != choiceIdsToCheck.Length)
                    return false;
                foreach (var item in choiceIdsToCheck)
                {
                    randomId = random.Next();
                    var answerFound = question.Answers.FirstOrDefault(x => x.Choice?.Identifier == item);

                    if (answerFound != null)
                    {
                        activityExecutionContext.JournalData.Add(
                            $"checkboxQuestionAnswerEquals Answer Found for choices {choiceIdsString}. (random id - {randomId})",
                            $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}, Answer: {answerFound.AnswerText}, Choice: {answerFound.Choice?.Identifier}");
                        result = true;
                    }
                    else
                    {
                        activityExecutionContext.JournalData.Add(
                            $"checkboxQuestionAnswerEquals Answer  not Found for choices {choiceIdsString}. (random id - {randomId})",
                            $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId},  Choice: {item}");
                        result = false;
                    }
                }
            }
            else
            {
                activityExecutionContext.JournalData.Add(
                    $"checkboxQuestionAnswerEquals Question NULL for choices {choiceIdsToCheck}. (random id - {randomId})",
                    $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdsToCheck}");
            }

            return result;
        }

        public async Task<bool> AnswerContains(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName,
            string questionId, string[] choiceIdsToCheck, bool containsAny = false)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            return AnswerContains(activityExecutionContext, choiceIdsToCheck, containsAny, question);
        }

        public async Task<bool> AnswerContains(ActivityExecutionContext activityExecutionContext, int dataDictionaryId,
            string[] choiceIdsToCheck, bool containsAny = false)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(activityExecutionContext.CorrelationId, dataDictionaryId,
                    CancellationToken.None);

            return AnswerContains(activityExecutionContext, choiceIdsToCheck, containsAny, question);
        }

        private bool AnswerContains(ActivityExecutionContext activityExecutionContext, string[] choiceIdsToCheck,
            bool containsAny, CustomModels.Question? question)
        {
            var methodName = containsAny ? "checkboxQuestionAnswerContainsAny" : "checkboxQuestionAnswerContains";
            bool result = false;
            var randomId = random.Next();
            var choiceIdsString = string.Join(", ", choiceIdsToCheck);
            if (question != null &&
                (question.QuestionType == QuestionTypeConstants.CheckboxQuestion ||
                 question.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion))
            {
                activityExecutionContext.JournalData.Add(
                    $"{methodName} Question Found for choices {choiceIdsString}. (random id - {randomId})",
                    $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}");

                if (question.Answers != null)
                {
                    foreach (var item in choiceIdsToCheck)
                    {
                        randomId = random.Next();
                        var answerFound = question.Answers.FirstOrDefault(x => x.Choice?.Identifier == item);
                        if (answerFound != null)
                        {
                            activityExecutionContext.JournalData.Add(
                                $"{methodName} Answer Found for choices {choiceIdsString}. (random id - {randomId})",
                                $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId}, Answer: {answerFound.AnswerText}, Choice: {answerFound.Choice?.Identifier}");

                            if (containsAny)
                            {
                                return true;
                            }
                            else
                            {
                                result = true;
                            }
                        }
                        else
                        {
                            activityExecutionContext.JournalData.Add(
                                $"{methodName} Answer  not Found for choices {choiceIdsString}. (random id - {randomId})",
                                $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdsToCheck}, DBQuestionID: {question.Id}, DBInstanceID: {question.WorkflowInstanceId},  Choice: {item}");
                            result = false;
                        }
                    }
                }
            }
            else
            {
                activityExecutionContext.JournalData.Add(
                    $"{methodName} Question NULL for choices {choiceIdsString}. (random id - {randomId})",
                    $"CorrelationID: {activityExecutionContext.CorrelationId}, ChoicesToCheck: {choiceIdsToCheck}");
            }

            return result;
        }

        public async Task<int> AnswerCount(string correlationId, string workflowName, string activityName, string questionId)
        {
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, correlationId, questionId, CancellationToken.None);
            return AnswerCount(question);
        }

        public async Task<int> AnswerCount(string correlationId, int dataDictionaryId)
        {
            var question =
                await _elsaCustomRepository.GetQuestionByDataDictionary(correlationId, dataDictionaryId,
                    CancellationToken.None);
            return AnswerCount(question);
        }

        private static int AnswerCount(CustomModels.Question? question)
        {
            int result = 0;

            if (question != null && (question.QuestionType == QuestionTypeConstants.CheckboxQuestion ||
                                     question.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion))
            {
                if (question.Answers != null)
                {
                    return question.Answers.Count;
                }
            }

            return result;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("checkboxQuestionGetAnswer", (Func<string, string, string, string?>)((workflowName, activityName, questionId) => GetAnswer(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("checkboxQuestionAnswerEquals", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerEquals(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck).Result));
            engine.SetValue("checkboxQuestionAnswerContains", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerContains(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck).Result));
            engine.SetValue("checkboxQuestionAnswerContainsAny", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerContains(activityExecutionContext, workflowName, activityName, questionId, choiceIdsToCheck, true).Result));
            engine.SetValue("checkboxQuestionAnswerCount", (Func<string, string, string, int>)((workflowName, activityName, questionId) => AnswerCount(activityExecutionContext.CorrelationId, workflowName, activityName, questionId).Result));
            engine.SetValue("checkboxQuestionGetAnswer", (Func<int, string?>)((dataDictionaryId) => GetAnswer(activityExecutionContext.CorrelationId, dataDictionaryId).Result));
            engine.SetValue("checkboxQuestionAnswerEquals", (Func<int, string[], bool>)((dataDictionaryId, choiceIdsToCheck) => AnswerEquals(activityExecutionContext, dataDictionaryId, choiceIdsToCheck).Result));
            engine.SetValue("checkboxQuestionAnswerContains", (Func<int, string[], bool>)((dataDictionaryId, choiceIdsToCheck) => AnswerContains(activityExecutionContext, dataDictionaryId, choiceIdsToCheck).Result));
            engine.SetValue("checkboxQuestionAnswerContainsAny", (Func<int, string[], bool>)((dataDictionaryId, choiceIdsToCheck) => AnswerContains(activityExecutionContext, dataDictionaryId, choiceIdsToCheck, true).Result));
            engine.SetValue("checkboxQuestionAnswerCount", (Func<int, int>)((dataDictionaryId) => AnswerCount(activityExecutionContext.CorrelationId, dataDictionaryId).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function checkboxQuestionGetAnswer(workflowName: string, activityName:string, questionId:string ): string;");
            output.AppendLine("declare function checkboxQuestionAnswerEquals(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[] ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerContains(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[]  ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerContainsAny(workflowName: string, activityName:string, questionId:string, choiceIdsToCheck:string[]  ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerCount(workflowName: string, activityName:string, questionId:string ): number;");
            output.AppendLine("declare function checkboxQuestionGetAnswer(dataDictionaryId: number): string;");
            output.AppendLine("declare function checkboxQuestionAnswerEquals(dataDictionaryId: number, choiceIdsToCheck:string[] ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerContains(dataDictionaryId: number, choiceIdsToCheck:string[]  ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerContainsAny(dataDictionaryId: number, choiceIdsToCheck:string[]  ): boolean;");
            output.AppendLine("declare function checkboxQuestionAnswerCount(dataDictionaryId: number): number;");
            return Task.CompletedTask;
        }
    }
}
