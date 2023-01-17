using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Models;
using Elsa.Server.Providers;
using Elsa.Services.Models;

namespace Elsa.Server.Services
{
    public interface IDeleteChangedWorkflowPathService
    {
        Task DeleteChangedWorkflowPath(string workflowInstanceId, string activityId,
             IActivityBlueprint nextActivity, WorkflowInstance workflowInstance, CancellationToken cancellationToken);
    }

    public class DeleteChangedWorkflowPathService : IDeleteChangedWorkflowPathService
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowPathProvider _workflowPathProvider;

        public DeleteChangedWorkflowPathService(IElsaCustomRepository elsaCustomRepository, IWorkflowPathProvider workflowPathProvider)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowPathProvider = workflowPathProvider;
        }
        public async Task DeleteChangedWorkflowPath(string workflowInstanceId, string activityId,
             IActivityBlueprint nextActivity, WorkflowInstance workflowInstance, CancellationToken cancellationToken)
        {
            var changedPathCustomNavigation =
                await _workflowPathProvider.GetChangedPathCustomNavigation(workflowInstanceId, activityId,
                    nextActivity.Id, cancellationToken);

            if (changedPathCustomNavigation != null)
            {
                var previousPathActivities =
                    await _workflowPathProvider.GetPreviousPathActivities(workflowInstance.DefinitionId,
                        changedPathCustomNavigation.ActivityId, cancellationToken);

                await _elsaCustomRepository.DeleteCustomNavigations(previousPathActivities, workflowInstanceId, cancellationToken);

                await _elsaCustomRepository.DeleteQuestionScreenAnswers(
                    changedPathCustomNavigation.WorkflowInstanceId, previousPathActivities, cancellationToken);
            }
        }
    }
}
