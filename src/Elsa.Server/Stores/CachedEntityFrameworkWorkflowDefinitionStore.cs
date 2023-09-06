using AutoMapper;
using Elsa.Models;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using Elsa.Persistence.Specifications;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
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


        public new async Task<WorkflowDefinition?> FindAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        {
            var db = _cache.GetDatabase();
            _logger.LogInformation($"Specification to map: {specification}");
            WorkflowDefinitionIdSpecification spec = (WorkflowDefinitionIdSpecification)specification;
            var workflowId = spec.Id;
            _logger.LogInformation($"Attempting to Retrieve Workflow From Cache: {workflowId}");
                var result = await db.StringGetAsync(workflowId);
            if (string.IsNullOrEmpty(result))
            {
                _logger.LogInformation($"No workflow found for Id: {workflowId}");
                _logger.LogInformation("Retrieving from Database");
                WorkflowDefinition? workflowDefinition = await base.FindAsync(specification!, cancellationToken);
                if (workflowDefinition != null)
                {
                    _logger.LogInformation($"Workflow retrieved from DB.  Setting cache value for Id: {workflowDefinition.Id}");
                    string jsonWorkflowDefiniton = JsonSerializer.Serialize(workflowDefinition);
                    await db.StringSetAsync(workflowDefinition.Id, jsonWorkflowDefiniton);
                    _logger.LogInformation("Set In Cache");
                    var valueSavedInCache = await db.StringGetAsync(workflowDefinition.Id);
                    _logger.LogInformation($"Value stored in Cache: {valueSavedInCache}");
                }
                return workflowDefinition;
            }
            else
            {
                _logger.LogInformation($"Workflow found in cache for Id: {workflowId}");
                WorkflowDefinition? parsedWorkflowDefinition = JsonSerializer.Deserialize<WorkflowDefinition>(result);
                return parsedWorkflowDefinition;
            }
        }
    }
}
