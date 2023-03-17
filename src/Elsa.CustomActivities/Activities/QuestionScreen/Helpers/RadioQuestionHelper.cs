using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class RadioQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public RadioQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }


        public async Task<bool> AnswerEquals(string WorkflowInstance, string workflowName, string activityName, string questionId, string choiceIdToCheck)
        {
            bool result = false;
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {

                    var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activity.Id,
                        WorkflowInstance, questionId, CancellationToken.None);
                    if (questionScreenAnswer != null &&
                        (questionScreenAnswer.QuestionType == QuestionTypeConstants.RadioQuestion || questionScreenAnswer.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion))
                    {
                        var choices = questionScreenAnswer.Choices;

                        if (choices != null)
                        {
                            var singleChoice = choices.FirstOrDefault(x => x.Answer == questionScreenAnswer.Answer);

                            if (singleChoice != null && choiceIdToCheck.Contains(singleChoice.Identifier))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<bool> AnswerIn(string WorkflowInstance, string workflowName, string activityName, string questionId, string[] choiceIdsToCheck)
        {
            bool result = false;
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activity.Id,
                        WorkflowInstance, questionId, CancellationToken.None);
                    if (questionScreenAnswer != null &&
                        (questionScreenAnswer.QuestionType == QuestionTypeConstants.RadioQuestion || questionScreenAnswer.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion))
                    {
                        var choices = questionScreenAnswer.Choices;

                        if (choices != null)
                        {
                            foreach (var item in choiceIdsToCheck)
                            {
                                var singleChoice = choices.FirstOrDefault(x => x.Identifier == item);
                                if (singleChoice != null)
                                {
                                    var answerCheck = choices.Select(x => x.Identifier).Contains(item) &&
                                                      singleChoice.Answer == questionScreenAnswer.Answer;

                                    if (answerCheck)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        result = false;
                                    }
                                }
                            }
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
            engine.SetValue("radioQuestionAnswerEquals", (Func<string, string, string, string, bool>)((workflowName, activityName, questionId, choiceIdToCheck) => AnswerEquals(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, choiceIdToCheck).Result));
            engine.SetValue("radioQuestionAnswerIn", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, choiceIdsToCheck) => AnswerIn(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, choiceIdsToCheck).Result));
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
