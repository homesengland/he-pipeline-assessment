using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class WeightedCheckboxQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public WeightedCheckboxQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }

        public async Task<bool> AnswerEquals(string correlationId, string workflowName, string activityName, string questionId, string groupId, string[] choiceIdsToCheck)
        {
            bool result = false;
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, questionId, CancellationToken.None);

            if (question != null &&
                question.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion &&
                question.Answers != null)
            {
                var answers = question.Answers
                    .Where(x => x.Choice?.QuestionChoiceGroup?.GroupIdentifier == groupId).ToList();
                if (answers.Count != choiceIdsToCheck.Length)
                    return false;
                foreach (var item in choiceIdsToCheck)
                {
                    var answerFound = answers.FirstOrDefault(x => x.Choice?.Identifier == item);
                    if (answerFound != null)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        public async Task<bool> AnswerContains(string correlationId, string workflowName, string activityName,
            string questionId, string groupId, string[] choiceIdsToCheck, bool containsAny = false)
        {
            bool result = false;
            var question = await _elsaCustomRepository.GetQuestionByWorkflowAndActivityName(activityName,
                workflowName, questionId, CancellationToken.None);
            if (question != null && question.QuestionType == QuestionTypeConstants.WeightedCheckboxQuestion)
            {

                if (question.Answers != null)
                {
                    var answers = question.Answers
                        .Where(x => x.Choice?.QuestionChoiceGroup?.GroupIdentifier == groupId).ToList();

                    foreach (var item in choiceIdsToCheck)
                    {
                        var answerFound = answers.FirstOrDefault(x => x.Choice?.Identifier == item);
                        if (answerFound != null)
                        {
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
                            result = false;
                        }
                    }
                }
            }

            return result;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("weightedCheckboxQuestionAnswerEquals", (Func<string, string, string, string, string[], bool>)((workflowName, activityName, questionId, groupId, choiceIdsToCheck) => AnswerEquals(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, groupId, choiceIdsToCheck).Result));
            engine.SetValue("weightedCheckboxQuestionAnswerContains", (Func<string, string, string, string, string[], bool>)((workflowName, activityName, questionId, groupId, choiceIdsToCheck) => AnswerContains(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, groupId, choiceIdsToCheck).Result));
            engine.SetValue("weightedCheckboxQuestionAnswerContainsAny", (Func<string, string, string, string, string[], bool>)((workflowName, activityName, questionId, groupId, choiceIdsToCheck) => AnswerContains(activityExecutionContext.CorrelationId, workflowName, activityName, questionId, groupId, choiceIdsToCheck, true).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function weightedCheckboxQuestionAnswerEquals(workflowName: string, activityName:string, questionId:string, groupId:string, choiceIdsToCheck:string[] ): boolean;");
            output.AppendLine("declare function weightedCheckboxQuestionAnswerContains(workflowName: string, activityName:string, questionId:string, groupId:string, choiceIdsToCheck:string[]  ): boolean;");
            output.AppendLine("declare function weightedCheckboxQuestionAnswerContainsAny(workflowName: string, activityName:string, questionId:string, groupId:string, choiceIdsToCheck:string[]  ): boolean;");
            return Task.CompletedTask;
        }
    }
}
