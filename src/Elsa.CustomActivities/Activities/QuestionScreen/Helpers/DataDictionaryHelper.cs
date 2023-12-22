using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class DataDictionaryHelper : INotificationHandler<EvaluatingJavaScriptExpression>/*, INotificationHandler<RenderingTypeScriptDefinitions>*/
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
                var dbDataDictionaryItems = await _elsaCustomRepository.GetQuestionDataDictionaryListAsync(cancellationToken);
                var cacheItems = dbDataDictionaryItems.Select(x => new DataDictionaryCacheItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Group = x.Group.Name
                });
                string json = JsonConvert.SerializeObject(cacheItems);
                await _cache.StringSetAsync(_cacheKey, json, _expiryTime);
                dataDictionaryInCache = await _cache.StringGetAsync(_cacheKey);
            }
            var dataDictionaryItems = JsonConvert.DeserializeObject<List<DataDictionaryCacheItem>>(dataDictionaryInCache);
            if (dataDictionaryItems != null)
            {
                var engine = notification.Engine;
                foreach (var dataDictionary in dataDictionaryItems)
                {
                    string name = DataDictionaryToJavascriptHelper.ToJintKey(dataDictionary.Group, dataDictionary.Name);

                    engine.SetValue(name, dataDictionary.Id);
                }
            }

            //engine.SetValue("DataDictionary", _dataDictionaryItems);
        }

        private class DataDictionaryCacheItem
        {
            public int Id{ get; set; }
            public string Name{ get; set; }
            public string Group{ get; set; }
        }

        //public async Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        //{
        //    //var output = notification.Output;
        //    //_dataDictionaryItems = await _elsaCustomRepository.GetQuestionDataDictionaryListAsync(cancellationToken);

        //    //var stringBuilder = new StringBuilder();

        //    ////stringBuilder.Append("declare const DataDictionary = {");

        //    //foreach (var dataDictionary in _dataDictionaryItems)
        //    //{
        //    //    var group = dataDictionary.Group.Name.Replace(" ", "_");
        //    //    stringBuilder.Append("declare const " + group + "_" + dataDictionary.Name + " = null;");
        //    //    //stringBuilder.Append(group + "_" +
        //    //    //                     dataDictionary.Name + ": '" + dataDictionary.Id + "',");
        //    //}

        //    ////stringBuilder.Remove(stringBuilder.Length - 1, 1);
        //    ////stringBuilder.Append("};");

        //    //var dataDictionaryObject = stringBuilder.ToString();
        //    //output.AppendLine(dataDictionaryObject);
        //}
    }
}
