using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomWorkflow.Sdk.DataDictionaryHelpers;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Elsa.Server.DataDictionaryAccessors
{
    public class CachedDataDictionaryIntellisenseAccessor : IDataDictionaryIntellisenseAccessor
    {
        private readonly IDatabase _cache;
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly TimeSpan _expiryTime = TimeSpan.FromHours(1);

        private string _cacheKey = "DataDictionary";
        public CachedDataDictionaryIntellisenseAccessor(IConnectionMultiplexer conn, IElsaCustomRepository repo)
        {
            _cache = conn.GetDatabase();
            _elsaCustomRepository = repo;
        }

        public async Task<bool> ClearDictionaryCache(string cacheKey, CancellationToken cancellationToken)
        {
            var result = await _cache.KeyDeleteAsync(cacheKey);
            return result;
        }

        public async Task<List<DataDictionaryItem>?> GetDictionary(CancellationToken cancellationToken, string? cacheKey = null)
        {
            SetCacheKey(cacheKey);
            List<DataDictionaryItem>? dataDictionaryItems = await GetDictionaryFromCache(cancellationToken);
            return dataDictionaryItems;
        }

        private void SetCacheKey(string? cacheKey)
        {
            if (!string.IsNullOrEmpty(cacheKey))
            {
                _cacheKey = cacheKey;
            }
        }

        private async Task<List<DataDictionaryItem>?> GetDictionaryFromCache(CancellationToken cancellationToken)
        {
            var dataDictionaryInCache = await _cache.StringGetAsync(_cacheKey);
            if (string.IsNullOrEmpty(dataDictionaryInCache))
            {
                dataDictionaryInCache = await AddDataDictionaryItemsToCache(cancellationToken);
            }
            var dataDictionaryItems = JsonConvert.DeserializeObject<List<DataDictionaryItem>>(dataDictionaryInCache!);
            return dataDictionaryItems;
        }

        private async Task<RedisValue> AddDataDictionaryItemsToCache(CancellationToken cancellationToken)
        {
            var cacheItems = await GetDictionaryFromDatabase(cancellationToken);
            string json = JsonConvert.SerializeObject(cacheItems);
            await _cache.StringSetAsync(_cacheKey, json, _expiryTime);
            var dataDictionaryInCache = await _cache.StringGetAsync(_cacheKey);
            return dataDictionaryInCache;
        }

        private async Task<List<DataDictionaryItem>?> GetDictionaryFromDatabase(CancellationToken cancellationToken)
        {
            var dbDataDictionaryItems = await _elsaCustomRepository.GetDataDictionaryListAsync(true, cancellationToken);
            var cacheItems = dbDataDictionaryItems.Select(x => new DataDictionaryItem
            {
                Id = x.Id,
                Name = x.Name,
                Group = x.Group.Name
            }).ToList();
            return cacheItems;
        }
    }
}
