using AutoMapper;
using Elsa.Models;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.Specifications;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using Elsa.Serialization;
using StackExchange.Redis;
using System.Text.Json;

namespace Elsa.Server.Stores
{
    public class CachedEntityFrameworkWorkflowDefinitionStore : ElsaStores.EntityFrameworkWorkflowDefinitionStore
    {
        private IConnectionMultiplexer _cache;
        private ILogger<CachedEntityFrameworkWorkflowDefinitionStore> _logger;
        public CachedEntityFrameworkWorkflowDefinitionStore(IElsaContextFactory dbContextFactory, IMapper mapper, IContentSerializer contentSerializer, IConnectionMultiplexer connectionMultiplexer, ILogger<CachedEntityFrameworkWorkflowDefinitionStore> logger) : base(dbContextFactory, mapper, contentSerializer)
        {
            _cache = connectionMultiplexer;
            _logger = logger;
        }


        public override async Task<WorkflowDefinition?> FindAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
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
                    await db.StringSetAsync(workflowDefinition.DefinitionId, jsonWorkflowDefiniton);
                    _logger.LogInformation("Set In Cache");
                    var valueSavedInCache = await db.StringGetAsync(workflowDefinition.DefinitionId);
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

        public override async Task UpdateAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default)
        {
            if (workflowDefinition != null)
            {
                //Check if it's in the Cache
                var db = _cache.GetDatabase();
                string cacheKey = workflowDefinition.DefinitionId;
                try
                {
                    await base.UpdateAsync(workflowDefinition, cancellationToken);
                    string workflowJson = JsonSerializer.Serialize(workflowDefinition);
                    await db.StringSetAsync(cacheKey, workflowJson);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error whilst updating workflow: {cacheKey}.  Clearing from Cache");
                     await db.KeyDeleteAsync(cacheKey);
                    throw new Exception(ex.Message);
                }
            }
        }

        public override async Task AddAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken)
        {
            if (workflowDefinition != null)
            {
                //Check if it's in the Cache
                var db = _cache.GetDatabase();
                string cacheKey = workflowDefinition.DefinitionId;
                await base.AddAsync(workflowDefinition, cancellationToken);
                string workflowJson = JsonSerializer.Serialize(workflowDefinition);
                await db.StringSetAsync(cacheKey, workflowJson);
            }
        }

        public override async Task SaveAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken)
        {
            if (workflowDefinition != null)
            {
                //Check if it's in the Cache
                var db = _cache.GetDatabase();
                string cacheKey = workflowDefinition.DefinitionId;
                await base.SaveAsync(workflowDefinition, cancellationToken);
                string workflowJson = JsonSerializer.Serialize(workflowDefinition);
                await db.StringSetAsync(cacheKey, workflowJson);
            }
        }

        public override async Task AddManyAsync(IEnumerable<WorkflowDefinition> entities, CancellationToken cancellationToken = default)
        {
            await base.AddManyAsync(entities, cancellationToken);
        }

        public override async Task DeleteAsync(WorkflowDefinition entity, CancellationToken cancellationToken = default)
        {
            var db = _cache.GetDatabase();
            string cacheKey = entity.DefinitionId;
            await base.DeleteAsync(entity, cancellationToken);
            await db.KeyDeleteAsync(cacheKey);
        }

        public override async Task<int> DeleteManyAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        {
            return await base.DeleteManyAsync(specification, cancellationToken);
        }

        public override async Task<IEnumerable<WorkflowDefinition>> FindManyAsync(ISpecification<WorkflowDefinition> specification, IOrderBy<WorkflowDefinition>? orderBy = default, IPaging? paging = default, CancellationToken cancellationToken = default)
        {
            return await base.FindManyAsync(specification, orderBy, paging, cancellationToken);
        }
    }
}
