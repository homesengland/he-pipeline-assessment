using AutoMapper;
using Elsa.Models;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.Specifications;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using Elsa.Serialization;
using Elsa.Server.Extensions;
using Elsa.Server.Features.Dashboard;
using Elsa.Server.Helpers;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Serialization.JsonNet;
using NodaTime.Serialization.SystemTextJson;
using NodaTime.Xml;
using StackExchange.Redis;
using System.Text.Json;

namespace Elsa.Server.Stores
{
    public class CachedEntityFrameworkWorkflowDefinitionStore : ElsaStores.EntityFrameworkWorkflowDefinitionStore
    {
        private IConnectionMultiplexer _cache;
        private ILogger<CachedEntityFrameworkWorkflowDefinitionStore> _logger;
        private string _Key = "WorkflowDefinition";
        private TimeSpan _expiryTime = TimeSpan.FromHours(1);
        private JsonSerializerSettings _serializerSettings;
        public CachedEntityFrameworkWorkflowDefinitionStore(IElsaContextFactory dbContextFactory, IMapper mapper, IContentSerializer contentSerializer, IConnectionMultiplexer connectionMultiplexer, ILogger<CachedEntityFrameworkWorkflowDefinitionStore> logger) : base(dbContextFactory, mapper, contentSerializer)
        {
            _cache = connectionMultiplexer;
            _logger = logger;
            _serializerSettings = new JsonSerializerSettings().ConfigureForInstants();
        }


        public override async Task<WorkflowDefinition?> FindAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        {
            try
            {
                var db = _cache.GetDatabase();
                _logger.LogInformation($"Specification to map: {specification}");
                bool shouldUseCache = TryGetCacheKeyFromSpecification(specification, out string cacheKey);
                if (shouldUseCache)
                {
                    _logger.LogInformation($"Attempting to Retrieve Workflow From Cache: {cacheKey}");
                    var result = await db.StringGetAsync(cacheKey);
                    if (string.IsNullOrEmpty(result))
                    {
                        _logger.LogInformation($"No workflow found for Id: {cacheKey}");
                        _logger.LogInformation("Retrieving from Database");
                        WorkflowDefinition? workflowDefinition = await base.FindAsync(specification!, cancellationToken);
                        if (workflowDefinition != null)
                        {
                            _logger.LogInformation($"Workflow retrieved from DB.  Setting cache value for Id: {workflowDefinition.Id}");
                            string jsonWorkflowDefiniton = JsonConvert.SerializeObject(workflowDefinition, _serializerSettings);
                            await db.StringSetAsync(cacheKey, jsonWorkflowDefiniton, _expiryTime);
                            _logger.LogInformation("Set In Cache");
                            var valueSavedInCache = await db.StringGetAsync(workflowDefinition.DefinitionId);
                            _logger.LogInformation($"Value stored in Cache: {valueSavedInCache}");
                        }
                        return workflowDefinition;
                    }
                    else
                    {
                        _logger.LogInformation($"Workflow found in cache for Key: {cacheKey}");
                        WorkflowDefinition? parsedWorkflowDefinition = JsonConvert.DeserializeObject<WorkflowDefinition>(result, _serializerSettings);
                        return parsedWorkflowDefinition;
                    }
                }
                else
                {
                    return await base.FindAsync(specification, cancellationToken);
                }
            }
            catch(Exception ex)
            {
                var errorString = ex.Message;
                throw new Exception(errorString, ex);
            }

        }

        public override async Task UpdateAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken = default)
        {
            if (workflowDefinition != null)
            {
                //Check if it's in the Cache
                var db = _cache.GetDatabase();
                string cacheKey = CacheKey(workflowDefinition);
                try
                {
                    await base.UpdateAsync(workflowDefinition, cancellationToken);
                    string workflowJson = JsonConvert.SerializeObject(workflowDefinition, _serializerSettings);
                    await db.StringSetAsync(cacheKey, workflowJson, _expiryTime);
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
                string cacheKey = CacheKey(workflowDefinition);
                await base.AddAsync(workflowDefinition, cancellationToken);
                string workflowJson = JsonConvert.SerializeObject(workflowDefinition, _serializerSettings);
                await db.StringSetAsync(cacheKey, workflowJson, _expiryTime);
            }
        }

        public override async Task SaveAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken)
        {
            if (workflowDefinition != null)
            {
                //Check if it's in the Cache
                var db = _cache.GetDatabase();
                string cacheKey = CacheKey(workflowDefinition); ;
                await base.SaveAsync(workflowDefinition, cancellationToken);
                string workflowJson = JsonConvert.SerializeObject(workflowDefinition, _serializerSettings);
                await db.StringSetAsync(cacheKey, workflowJson, _expiryTime);
            }
        }

        public override async Task AddManyAsync(IEnumerable<WorkflowDefinition> entities, CancellationToken cancellationToken = default)
        {
            await base.AddManyAsync(entities, cancellationToken);
        }

        public override async Task DeleteAsync(WorkflowDefinition entity, CancellationToken cancellationToken = default)
        {
            var db = _cache.GetDatabase();
            string cacheKey = CacheKey(entity);
            await base.DeleteAsync(entity, cancellationToken);
            await db.KeyDeleteAsync(cacheKey);
        }

        public override async Task<int> DeleteManyAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        {
            return await base.DeleteManyAsync(specification, cancellationToken);
        }

        public override async Task<IEnumerable<WorkflowDefinition>> FindManyAsync(ISpecification<WorkflowDefinition> specification, IOrderBy<WorkflowDefinition>? orderBy = default, IPaging? paging = default, CancellationToken cancellationToken = default)
        {
            if(specification is VersionHistorySpecification historySpecification)
            {
                return await base.GetHistory(historySpecification,cancellationToken);
            }
            if (specification is WorkflowDefinitionListSpecification listSpecification)
            {
                return await base.GetWorkflowDefinitionList(listSpecification, orderBy, paging, cancellationToken);
            }
            return await base.FindManyAsync(specification, orderBy, paging, cancellationToken);
        }

        private bool TryGetCacheKeyFromSpecification(ISpecification<WorkflowDefinition> specification, out string cacheKey)
        {
            cacheKey = string.Empty;
            if(specification is WorkflowDefinitionIdSpecification)
            {
                WorkflowDefinitionIdSpecification spec = (WorkflowDefinitionIdSpecification)specification;
                cacheKey = CacheKey(spec);
                return true;
            }
            if(specification is LatestOrPublishedWorkflowDefinitionIdSpecification)
            {
                LatestOrPublishedWorkflowDefinitionIdSpecification spec = (LatestOrPublishedWorkflowDefinitionIdSpecification)specification;
                cacheKey = CacheKey(spec);
                return true;
            }
            return false;
        }

        private string CacheKey(WorkflowDefinitionIdSpecification workflow)
        {
            if(workflow.VersionOptions == null)
            {
                return $"{_Key}:{workflow.Id}:Latest";
            }
            else
            {
                return $"{_Key}:{workflow.Id}:{workflow.VersionOptions.ToString()}";
            }
        }

        private string CacheKey(LatestOrPublishedWorkflowDefinitionIdSpecification workflow)
        {
            return $"{_Key}:{workflow.WorkflowDefinitionId}:Latest";
        }

        private string CacheKey(WorkflowDefinition workflow)
        {
            string version = workflow.IsLatest ? "Latest": workflow.Version.ToString();
            string key = $"{_Key}:{workflow.DefinitionId}:{version}";
            return key;
        }
            
    }
}
