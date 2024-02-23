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
    }
}
