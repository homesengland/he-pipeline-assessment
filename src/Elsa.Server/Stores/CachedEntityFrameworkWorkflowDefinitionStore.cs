using AutoMapper;
using Elsa.Models;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using Elsa.Persistence.Specifications;
using Elsa.Serialization;
using He.AspNetCore.Mvc.Gds.Components.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;

namespace Elsa.Server.Stores
{
    public class CachedEntityFrameworkWorkflowDefinitionStore : EntityFrameworkWorkflowDefinitionStore
    {
        private IConnectionMultiplexer _cache;
        private ILogger<CachedEntityFrameworkWorkflowDefinitionStore> _logger;
        public CachedEntityFrameworkWorkflowDefinitionStore(IElsaContextFactory dbContextFactory, IMapper mapper, IContentSerializer contentSerializer, IConnectionMultiplexer connectionMultiplexer, ILogger<CachedEntityFrameworkWorkflowDefinitionStore> logger) : base(dbContextFactory, mapper, contentSerializer)
        {
            _cache = connectionMultiplexer;
            _logger = logger;
        }


        new public async Task<WorkflowDefinition?> FindAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        {
            var db = _cache.GetDatabase();
            if (specification != null)
            {
                var result = await db.StringGetAsync(specification.ToString());
                if (string.IsNullOrEmpty(result))
                {
                    return await base.FindAsync(specification, cancellationToken);
                }
                else
                {
                    WorkflowDefinition? parsedWorkflowDefinition = JsonSerializer.Deserialize<WorkflowDefinition>(result);
                    return parsedWorkflowDefinition;
                }
            }

            WorkflowDefinition? workflowDefinition = await base.FindAsync(specification!, cancellationToken);
            if(workflowDefinition != null)
            {
                string jsonWorkflowDefiniton = JsonSerializer.Serialize(workflowDefinition);
                await db.StringSetAsync(workflowDefinition.Id, jsonWorkflowDefiniton);
            }
            return workflowDefinition;
        }
    }
}
