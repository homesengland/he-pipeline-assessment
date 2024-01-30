using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class DataDictionaryHelper : INotificationHandler<EvaluatingJavaScriptExpression>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly TimeSpan _expiryTime = TimeSpan.FromHours(1);
        private readonly IDatabase _cache;
        private readonly string _cacheKey = "DataDictionary";

        public DataDictionaryHelper(
            IElsaCustomRepository elsaCustomRepository, 
            IConnectionMultiplexer cache)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _cache = cache.GetDatabase();
        }

        public async Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var dataDictionaryInCache = await _cache.StringGetAsync(_cacheKey);
            if (string.IsNullOrEmpty(dataDictionaryInCache))
            {
                dataDictionaryInCache = await AddDataDictionaryItemsToCache(cancellationToken);
            }
            var dataDictionaryItems = JsonConvert.DeserializeObject<List<DataDictionaryCacheItem>>(dataDictionaryInCache);
            if (dataDictionaryItems != null)
            {
                var engine = notification.Engine;
                foreach (var dataDictionary in dataDictionaryItems)
                {
                    if (!string.IsNullOrEmpty(dataDictionary.Group) && !string.IsNullOrEmpty(dataDictionary.Name))
                    {
                        string name = DataDictionaryToJavascriptHelper.ToJintKey(dataDictionary.Group, dataDictionary.Name);
                        engine.SetValue(name, dataDictionary.Id);
                    }
                }
            }
        }

        private async Task<RedisValue> AddDataDictionaryItemsToCache(CancellationToken cancellationToken)
        {
            var dbDataDictionaryItems = await _elsaCustomRepository.GetDataDictionaryListAsync(cancellationToken);
            var cacheItems = dbDataDictionaryItems.Select(x => new DataDictionaryCacheItem
            {
                Id = x.Id,
                Name = x.Name,
                Group = x.Group.Name
            });
            string json = JsonConvert.SerializeObject(cacheItems);
            await _cache.StringSetAsync(_cacheKey, json, _expiryTime);
            var dataDictionaryInCache = await _cache.StringGetAsync(_cacheKey);
            return dataDictionaryInCache;
        }
    }
}
