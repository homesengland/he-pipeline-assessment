using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk.GlobalVariableHelpers;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Elsa.Server.GlobalVariableAccessors
{
    public class CachedGlobalVariableIntellisenseAccessor : IGlobalVariableIntellisenseAccessor
    {
        private readonly IDatabase _cache;
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly TimeSpan _expiryTime = TimeSpan.FromHours(1);

        private string _cacheKey = "GlobalVariable";
        public CachedGlobalVariableIntellisenseAccessor(IConnectionMultiplexer conn, IElsaCustomRepository repo)
        {
            _cache = conn.GetDatabase();
            _elsaCustomRepository = repo;
        }

        public async Task<bool> ClearGlobalVariableCache(string cacheKey, CancellationToken cancellationToken)
        {
            var result = await _cache.KeyDeleteAsync(cacheKey);
            return result;
        }

        public async Task<List<GlobalVariableItem>?> GetGlobalVariable(CancellationToken cancellationToken, string? cacheKey = null)
        {
            SetCacheKey(cacheKey);
            List<GlobalVariableItem>? globalVariableItems = await GetGlobalVariableFromCache(cancellationToken);
            return globalVariableItems;
        }

        private void SetCacheKey(string? cacheKey)
        {
            if (!string.IsNullOrEmpty(cacheKey))
            {
                _cacheKey = cacheKey;
            }
        }

        private async Task<List<GlobalVariableItem>?> GetGlobalVariableFromCache(CancellationToken cancellationToken)
        {
            var globalVariableInCache = await _cache.StringGetAsync(_cacheKey);
            if (string.IsNullOrEmpty(globalVariableInCache))
            {
                globalVariableInCache = await AddGlobalVariableItemsToCache(cancellationToken);
            }
            var globalVariableItems = JsonConvert.DeserializeObject<List<GlobalVariableItem>>(globalVariableInCache);
            return globalVariableItems;
        }

        private async Task<RedisValue> AddGlobalVariableItemsToCache(CancellationToken cancellationToken)
        {
            var cacheItems = await GetGlobalVariableFromDatabase(cancellationToken);
            string json = JsonConvert.SerializeObject(cacheItems);
            await _cache.StringSetAsync(_cacheKey, json, _expiryTime);
            var globalVariableInCache = await _cache.StringGetAsync(_cacheKey);
            return globalVariableInCache;
        }

        private async Task<List<GlobalVariableItem>?> GetGlobalVariableFromDatabase(CancellationToken cancellationToken)
        {
            var dbGlobalVariableItems = await _elsaCustomRepository.GetGlobalVariableListAsync(true, cancellationToken);
            var cacheItems = dbGlobalVariableItems.Select(x => new GlobalVariableItem
            {
                Id = x.Id,
                Name = x.Name,
                Group = x.Group.Name
            }).ToList();
            return cacheItems;
        }
    }
}
