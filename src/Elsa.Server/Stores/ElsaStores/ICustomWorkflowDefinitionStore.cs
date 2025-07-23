using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.EntityFramework.Core;
using Elsa.Persistence.Specifications;
using Elsa.Server.Models;

namespace Elsa.Server.Stores.ElsaStores
{
    public interface ICustomWorkflowDefinitionStore : IWorkflowDefinitionStore
    {
        Task UnpublishAll(string definitionId, CancellationToken token);
        Task Unpublish(WorkflowDefinition definition, CancellationToken token);

        Task RemoveLatest(WorkflowDefinition definition, CancellationToken token);
        Task Publish(WorkflowDefinition definition, CancellationToken token);

        Task<List<WorkflowDefinitionIdentifiers>> FindWorkflowDefinitionIdentifiersAsync(string definitionId, VersionOptions? options, CancellationToken cancellationToken = default);
    }
}
