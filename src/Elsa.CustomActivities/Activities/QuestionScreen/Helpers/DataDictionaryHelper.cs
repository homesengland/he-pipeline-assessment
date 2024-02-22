using Elsa.CustomWorkflow.Sdk.DataDictionaryHelpers;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class DataDictionaryHelper : INotificationHandler<EvaluatingJavaScriptExpression>
    {
        private readonly IDataDictionaryIntellisenseAccessor _accessor;
        private readonly string _cacheKey = "DataDictionary";

        public DataDictionaryHelper(IDataDictionaryIntellisenseAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            List<DataDictionaryItem>? dataDictionaryItems = await _accessor.GetDictionary(cancellationToken, _cacheKey);

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

        //private async Task<List<DataDictionaryItem>?> GetDictionary(CancellationToken cancellationToken)
        //{
        //    List<DataDictionaryItem>? dataDictionaryItems = null;
        //    if (_useCache)
        //    {
        //        dataDictionaryItems = await GetDictionaryFromCache(cancellationToken);
        //    }
        //    else
        //    {
        //        dataDictionaryItems = await GetDictionaryFromDatabase(cancellationToken);
        //    }
        //    return dataDictionaryItems;
        //}

        //private async Task<List<DataDictionaryItem>?> GetDictionaryFromCache(CancellationToken cancellationToken)
        //{
        //    var dataDictionaryInCache = await _cache.StringGetAsync(_cacheKey);
        //    if (string.IsNullOrEmpty(dataDictionaryInCache))
        //    {
        //        dataDictionaryInCache = await AddDataDictionaryItemsToCache(cancellationToken);
        //    }
        //    var dataDictionaryItems = JsonConvert.DeserializeObject<List<DataDictionaryItem>>(dataDictionaryInCache);
        //    return dataDictionaryItems;
        //}

        //private async Task<RedisValue> AddDataDictionaryItemsToCache(CancellationToken cancellationToken)
        //{
        //    var cacheItems = await GetDictionaryFromDatabase(cancellationToken);
        //    string json = JsonConvert.SerializeObject(cacheItems);
        //    await _cache.StringSetAsync(_cacheKey, json, _expiryTime);
        //    var dataDictionaryInCache = await _cache.StringGetAsync(_cacheKey);
        //    return dataDictionaryInCache;
        //}

        //private async Task<List<DataDictionaryItem>?> GetDictionaryFromDatabase(CancellationToken cancellationToken)
        //{
        //    var dbDataDictionaryItems = await _elsaCustomRepository.GetDataDictionaryListAsync(true, cancellationToken);
        //    var cacheItems = dbDataDictionaryItems.Select(x => new DataDictionaryItem
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        Group = x.Group.Name
        //    }).ToList();
        //    return cacheItems;
        //}
    }
}
