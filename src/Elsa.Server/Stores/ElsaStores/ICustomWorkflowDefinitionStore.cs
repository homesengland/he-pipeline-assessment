using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.EntityFramework.Core;
using Elsa.Persistence.Specifications;

namespace Elsa.Server.Stores.ElsaStores
{
    public interface ICustomWorkflowDefinitionStore : IWorkflowDefinitionStore
    {
        Task OnUnpublishDefinitions(string definitionId, CancellationToken token);
    }
}
