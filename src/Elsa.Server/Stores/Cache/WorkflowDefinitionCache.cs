using Elsa.Models;
using Elsa.Persistence.Specifications;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using Elsa.Server.Extensions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Elsa.Server.Stores.Cache
{
    public interface IWorkflowDefinitionCache
    {
        // Define methods for caching workflow definitions, e.g. Get, Set, Remove, etc.
        void AddDefinition(WorkflowDefinition workflowDefinition);
        Task<WorkflowDefinition?> GetDefinition(ISpecification<WorkflowDefinition> specification);
        void SaveDefinition(WorkflowDefinition workflowDefinition);
        void RemoveDefinition(WorkflowDefinition definition);
    }

    public class WorkflowDefinitionCache : IWorkflowDefinitionCache
    {
        private string _Key = "WorkflowDefinition";
        private IConnectionMultiplexer _cache;
        private JsonSerializerSettings _serializerSettings;

        public WorkflowDefinitionCache(IConnectionMultiplexer connectionMultiplexer)
        {
            _cache = connectionMultiplexer;
            _serializerSettings = new JsonSerializerSettings().ConfigureForInstants();
        }

        public void AddDefinition(WorkflowDefinition workflowDefinition)
        {
            if (workflowDefinition == null)
                throw new ArgumentNullException(nameof(workflowDefinition));

            var db = _cache.GetDatabase();
            string key = CacheKey(workflowDefinition);
            string workflowJson = JsonConvert.SerializeObject(workflowDefinition, _serializerSettings);
            db.StringSetAsync(key, workflowJson, TimeSpan.FromHours(1));
        }

        public async Task <WorkflowDefinition?> GetDefinition(ISpecification<WorkflowDefinition> specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));
            if (!TryGetCacheKeyFromSpecification(specification, out var cacheKey))
                return null;
            var db = _cache.GetDatabase();
            string? workflowJson = await db.StringGetAsync(cacheKey);
            if (string.IsNullOrEmpty(workflowJson))
                return null;
            return JsonConvert.DeserializeObject<WorkflowDefinition>(workflowJson, _serializerSettings);
        }

        public void SaveDefinition(WorkflowDefinition workflowDefinition)
        {
            var db = _cache.GetDatabase();
            string cacheKey = CacheKey(workflowDefinition);
            string workflowJson = JsonConvert.SerializeObject(workflowDefinition, _serializerSettings);


            if (workflowDefinition.IsPublished && workflowDefinition.IsLatest)
            {
                string key = $"{_Key}:{workflowDefinition.DefinitionId}:Latest";
                Task<bool> success = db.StringSetAsync(key, workflowJson, TimeSpan.FromHours(1));
            }
            else
            {
                Task<bool> success = db.StringSetAsync(cacheKey, workflowJson, TimeSpan.FromHours(1));
            }
        }

        public void RemoveDefinition(WorkflowDefinition definition)
        {
            string cacheKey = CacheKey(definition);
            var db = _cache.GetDatabase();
            db.KeyDelete(cacheKey);
        }

        private bool TryGetCacheKeyFromSpecification(ISpecification<WorkflowDefinition> specification, out string cacheKey)
        {
            cacheKey = string.Empty;
            if (specification is WorkflowDefinitionIdSpecification)
            {
                WorkflowDefinitionIdSpecification spec = (WorkflowDefinitionIdSpecification)specification;
                cacheKey = CacheKey(spec);
                return true;
            }
            if (specification is LatestOrPublishedWorkflowDefinitionIdSpecification)
            {
                LatestOrPublishedWorkflowDefinitionIdSpecification spec = (LatestOrPublishedWorkflowDefinitionIdSpecification)specification;
                cacheKey = CacheKey(spec);
                return true;
            }
            return false;
        }

        private string CacheKey(WorkflowDefinitionIdSpecification workflow)
        {
            if (workflow.VersionOptions == null)
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
            string version = workflow.IsLatest ? "Latest" : workflow.Version.ToString();
            string key = $"{_Key}:{workflow.DefinitionId}:{version}";
            return key;
        }
    }
}
