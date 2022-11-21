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

            var activityId = workflowBlueprint.Activities.FirstOrDefault(x => x.Name == activityName).Id;

            var result = await _elsaCustomRepository.GetQuestionScreenAnswer(activityId, activityExecutionContext.CorrelationId, questionId, CancellationToken.None);

            return result;

        }
    }
}

