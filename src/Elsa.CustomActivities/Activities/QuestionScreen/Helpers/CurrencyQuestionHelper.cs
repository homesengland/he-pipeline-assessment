using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class CurrencyQuestionHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {

        private readonly IElsaCustomRepository _elsaCustomRepository;

        public CurrencyQuestionHelper(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }


        public async Task<bool> AnswerEqualToOrGreaterThan(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, decimal answerToCheck)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            if (questionScreenAnswer != null && questionScreenAnswer.Answer != null && questionScreenAnswer.QuestionType == QuestionTypeConstants.CurrencyQuestion)
            {
                var answer = decimal.Parse(questionScreenAnswer.Answer);
                if (answer >= answerToCheck)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> AnswerEqualToOrLessThan(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, decimal answerToCheck)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionScreenAnswer = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            if (questionScreenAnswer != null && questionScreenAnswer.Answer != null && questionScreenAnswer.QuestionType == QuestionTypeConstants.CurrencyQuestion)
            {
                var answer = decimal.Parse(questionScreenAnswer.Answer);
                if (answer <= answerToCheck)
                {
                    return true;
                }
            }

            return false;
        }


        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;
            engine.SetValue("currencyQuestionAnswerEqualToOrGreaterThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrGreaterThan(activityExecutionContext, workflowName, activityName, questionId, answerToCheck).Result));
            engine.SetValue("currencyQuestionAnswerEqualToOrLessThan", (Func<string, string, string, decimal, bool>)((workflowName, activityName, questionId, answerToCheck) => AnswerEqualToOrLessThan(activityExecutionContext, workflowName, activityName, questionId, answerToCheck).Result));
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
