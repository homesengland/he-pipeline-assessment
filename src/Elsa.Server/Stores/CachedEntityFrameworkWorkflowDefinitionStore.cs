using AutoMapper;
using Elsa.Activities.Workflows.Workflow;
using Elsa.Models;
using Elsa.Persistence.EntityFramework.Core;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.Specifications;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using Elsa.Serialization;
using Elsa.Server.Extensions;
using Elsa.Server.Features.Dashboard;
using Elsa.Server.Helpers;
using Elsa.Server.Stores.Cache;
using Google.Protobuf;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Serialization.JsonNet;
using NodaTime.Serialization.SystemTextJson;
using NodaTime.Xml;
using StackExchange.Redis;
using System;
using System.Text.Json;

namespace Elsa.Server.Stores
{
    public class CachedEntityFrameworkWorkflowDefinitionStore : ElsaStores.EntityFrameworkWorkflowDefinitionStore
    {
        //private IConnectionMultiplexer _cache;
        private ILogger<CachedEntityFrameworkWorkflowDefinitionStore> _logger;
        private IWorkflowDefinitionCache _workflowDefinitionCache;
        public CachedEntityFrameworkWorkflowDefinitionStore(IElsaContextFactory dbContextFactory, IMapper mapper, IContentSerializer contentSerializer, IConnectionMultiplexer connectionMultiplexer, ILogger<CachedEntityFrameworkWorkflowDefinitionStore> logger, IWorkflowDefinitionCache cache) : base(dbContextFactory, mapper, contentSerializer)
        {
            //_cache = connectionMultiplexer;
            _logger = logger;
            _workflowDefinitionCache = cache;
        }


        public override async Task<WorkflowDefinition?> FindAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        {
            try
            {
                WorkflowDefinition? definition = await _workflowDefinitionCache.GetDefinition(specification);
                if (definition == null)
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
                        WorkflowDefinition? parsedWorkflowDefinition = JsonConvert.DeserializeObject<WorkflowDefinition>(result!, _serializerSettings);
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
                try
                {
                    await base.UpdateAsync(workflowDefinition, cancellationToken);
                    await _workflowDefinitionCache.SaveDefinition(workflowDefinition);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error whilst updating workflow: {workflowDefinition.DefinitionId}.  Clearing from Cache");
                    _workflowDefinitionCache.RemoveDefinition(workflowDefinition);
                    throw new Exception(ex.Message);
                }
            }
        }

        public override async Task AddAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken)
        {
            if (workflowDefinition != null)
            {
                await base.AddAsync(workflowDefinition, cancellationToken);
                _workflowDefinitionCache.AddDefinition(workflowDefinition);
            }
        }

        public override async Task SaveAsync(WorkflowDefinition workflowDefinition, CancellationToken cancellationToken)
        {
            if (workflowDefinition != null)
            {
                await base.SaveAsync(workflowDefinition, cancellationToken);
                await _workflowDefinitionCache.SaveDefinition(workflowDefinition);
            }
        }

        public override async Task AddManyAsync(IEnumerable<WorkflowDefinition> entities, CancellationToken cancellationToken = default)
        {
            await base.AddManyAsync(entities, cancellationToken);
        }

        public override async Task DeleteAsync(WorkflowDefinition entity, CancellationToken cancellationToken = default)
        {
            await base.DeleteAsync(entity, cancellationToken);
            _workflowDefinitionCache.RemoveDefinition(entity);
        }

        public override async Task<int> DeleteManyAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        {
            return await base.DeleteManyAsync(specification, cancellationToken);
        }

        public override async Task UnpublishAll(string definitionId, CancellationToken token)
        {
            _workflowDefinitionCache.RemoveDefinition(definitionId);
            await base.UnpublishAll(definitionId, token);
        }

        public override async Task Unpublish(WorkflowDefinition definition, CancellationToken token)
        {
            await base.Unpublish(definition, token);
            _workflowDefinitionCache.RemoveDefinition(definition);
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
            
    }
}
