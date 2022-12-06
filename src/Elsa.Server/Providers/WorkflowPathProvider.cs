using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.Server.Providers
{
    public class WorkflowPathProvider : IWorkflowPathProvider
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IWorkflowRegistry _workflowRegistry;

        public WorkflowPathProvider(IElsaCustomRepository elsaCustomRepository, IWorkflowRegistry workflowRegistry)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _workflowRegistry = workflowRegistry;
        }

        public async Task<CustomActivityNavigation?> GetChangedPathCustomNavigation(string workflowInstanceId,
            string currentActivityId, string nextActivityId, CancellationToken cancellationToken)
        {
            var changedPathNavigation = await
                _elsaCustomRepository.GetChangedPathNavigation(workflowInstanceId, currentActivityId, nextActivityId, cancellationToken);

            return changedPathNavigation;
        }

        public async Task<List<string>> GetPreviousPathActivities(string definitionId,
            string changedPathActivityId, CancellationToken cancellationToken)
        {
            var workflow =
                await _workflowRegistry.FindAsync(definitionId, VersionOptions.Published,
                    cancellationToken: cancellationToken);

            var targetConnections = GetTargetConnections(changedPathActivityId, workflow, new List<string>() { changedPathActivityId });

            return targetConnections.ToList();
        }

        private static IEnumerable<string> GetTargetConnections(string activityId, IWorkflowBlueprint? workflow,
            ICollection<string> list)
        {
            var connections = workflow!.Connections
                .Where(x => x.Source.Activity.Id == activityId &&
                            !list.Contains(x.Target.Activity.Id)).Select(x => x.Target.Activity.Id).ToList();

            var additionalConnections = new List<string>(list);
            foreach (var connection in connections)
            {
                additionalConnections.AddRange(GetTargetConnections(connection, workflow, connections));
            }

            connections.AddRange(additionalConnections);
            return connections;
        }
    }
}
