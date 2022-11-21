using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;
namespace Elsa.CustomActivities.Activities.Shared
{



    public class QuestionScreenQuery : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public QuestionScreenQuery(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;

            engine.SetValue("getQuestionAnswer", (Func<string, string, string, QuestionScreenAnswer?>)((workflowName, activityName, questionId) => GetAssessmentQuestion(activityExecutionContext, workflowName, activityName, questionId).Result));
            engine.SetValue("getAnswer", (Func<string, string, string, string?>)((workflowName, activityName, questionId) => GetAnswer(activityExecutionContext, workflowName, activityName, questionId).Result));
            engine.SetValue("hasAnswer", (Func<string, string, string, string[], bool>)((workflowName, activityName, questionId, possibleAnswers) => HasAnswer(activityExecutionContext, workflowName, activityName, questionId, possibleAnswers).Result));

        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare function getQuestionAnswer(workflowName: string, activityName:string, questionId:string ): QuestionScreenAnswer;");

            return Task.CompletedTask;
        }

        private async Task<QuestionScreenAnswer?> GetAssessmentQuestion(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Elsa.Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var result = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            return result;

        }

        private async Task<string?> GetAnswer(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Elsa.Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var result = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            return result.Answer;

        }

        private async Task<bool> HasAnswer(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName, string questionId, string[] answersToCheck)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Elsa.Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var activityId = workflowBlueprint!.Activities.FirstOrDefault(x => x.Name == activityName)!.Id;

            var questionInstance = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            bool areAllAnswersIncluded = false;
            foreach (var answer in answersToCheck)
            {
                if(questionInstance.Answer != null)
                {
                    questionInstance!.Answer.ToLower().Contains(answer.ToLower());
                    areAllAnswersIncluded = true;
                }
                else
                {
                    areAllAnswersIncluded = false;
                    break;
                }
            }
            return areAllAnswersIncluded;

        }
}
}

