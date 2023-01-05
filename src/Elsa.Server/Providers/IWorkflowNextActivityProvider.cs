using Elsa.CustomModels;
using Elsa.Services.Models;

namespace Elsa.Server.Providers
{
    public interface IWorkflowNextActivityProvider
    {
        Task<IActivityBlueprint> GetNextActivity(string commandActivityId, string workflowInstanceId,
            List<QuestionScreenAnswer>? dbAssessmentQuestionList, string activityType,
            CancellationToken cancellationToken);
        Task<IActivityBlueprint?> GetStartWorkflowNextActivity(IActivityBlueprint commandActivityId,
            string workflowInstanceId,
            CancellationToken cancellationToken);
    }
}
