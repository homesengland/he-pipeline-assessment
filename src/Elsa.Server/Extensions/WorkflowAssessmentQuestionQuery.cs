using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;

namespace Elsa.Server.Extensions
{
    public class WorkflowAssessmentQuestionQuery : INotificationHandler<EvaluatingJavaScriptExpression>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public WorkflowAssessmentQuestionQuery(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var engine = notification.Engine;

            engine.SetValue("getWorkflowAssessmentQuestion", (Func<string, string, AssessmentQuestion?>)((activityId, workflowInstanceId) => _elsaCustomRepository.GetAssessmentQuestion(activityId, workflowInstanceId, cancellationToken).Result));

        }
    }
}
