using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class CurrencyQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public CurrencyQuestionHelper(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }


        public async Task<bool> AnswerEqualToOrGreaterThan(string workflowInstanceId, string workflowName, string activityName, string questionId, decimal answerToCheck)
        {
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var questionScreenQuestion = await _elsaCustomRepository.GetQuestionScreenQuestion(activity.Id,
                        workflowInstanceId, questionId, CancellationToken.None);

                    if (questionScreenQuestion != null && questionScreenQuestion.Answers != null &&
                        questionScreenQuestion.QuestionType == QuestionTypeConstants.CurrencyQuestion)
                    {
                        var questionScreenAnswer = questionScreenQuestion.Answers.FirstOrDefault();
                        if (questionScreenAnswer != null)
                        {
                            var answer = decimal.Parse(questionScreenAnswer.Answer);
                            if (answer >= answerToCheck)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public async Task<bool> AnswerEqualToOrLessThan(string workflowInstanceId, string workflowName, string activityName, string questionId, decimal answerToCheck)
        {
            var workflowBlueprint = await _workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published);

            if (workflowBlueprint != null)
            {
                var activity = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName);
                if (activity != null)
                {
                    var questionScreenQuestion = await _elsaCustomRepository.GetQuestionScreenQuestion(activity.Id,
                        workflowInstanceId, questionId, CancellationToken.None);

                    if (questionScreenQuestion != null && questionScreenQuestion.Answers != null &&
                        questionScreenQuestion.QuestionType == QuestionTypeConstants.CurrencyQuestion)
                    {
                        var questionScreenAnswer = questionScreenQuestion.Answers.FirstOrDefault();
                        if (questionScreenAnswer != null)
                        {
                            var answer = decimal.Parse(questionScreenAnswer.Answer);
                            if (answer <= answerToCheck)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }


        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("currencyQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("currencyQuestionAnswerEqualToOrLessThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext.WorkflowInstance.Id, workflowName, activityName, questionId, answerToCheck).Result));
            return Task.CompletedTask;
        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            output.AppendLine("declare function currencyQuestionAnswerEqualToOrGreaterThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            output.AppendLine("declare function currencyQuestionAnswerEqualToOrLessThan(workflowName: string, activityName:string, questionId:string, answerToCheck:decimal ): boolean;");
            return Task.CompletedTask;
        }
    }
}
