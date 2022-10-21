using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.Server.Extensions
{
    public class WorkflowAssessmentQuestionQuery : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public WorkflowAssessmentQuestionQuery(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var activityExecutionContext = notification.ActivityExecutionContext;
            var engine = notification.Engine;

            engine.SetValue("getWorkflowAssessmentQuestion", (Func<string, string, AssessmentQuestion?>)((workflowName, activityName) => GetAssessmentQuestion(activityExecutionContext, workflowName, activityName).Result));

        }

        public Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;

            output.AppendLine("declare function getWorkflowAssessmentQuestion(workflowName: string, activityName:string ): AssessmentQuestion;");

            return Task.CompletedTask;
        }

        private async Task<AssessmentQuestion?> GetAssessmentQuestion(ActivityExecutionContext activityExecutionContext, string workflowName, string activityName)
        {
            var workflowRegistry = activityExecutionContext.GetService<IWorkflowRegistry>();
            var workflowBlueprint = workflowRegistry.FindByNameAsync(workflowName, Elsa.Models.VersionOptions.Published).Result;
            var workflowId = workflowBlueprint?.Id;

            var result = await _elsaCustomRepository.GetAssessmentQuestion(workflowId,
                activityExecutionContext.CorrelationId, activityName);

            return result;

        }
    }
}
