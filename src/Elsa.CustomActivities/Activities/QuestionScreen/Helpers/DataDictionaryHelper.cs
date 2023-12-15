using System.Text;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using Elsa.Services.Models;
using Jint.Native;
using Jint.Runtime;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class DataDictionaryHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private List<QuestionDataDictionary> _dataDictionaryItems;

        public DataDictionaryHelper(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var engine = notification.Engine;
            _dataDictionaryItems = await _elsaCustomRepository.GetQuestionDataDictionaryListAsync(cancellationToken);
            foreach (var dataDictionary in _dataDictionaryItems)
            {
                var group = dataDictionary.Group.Name.Replace(" ", "_");

                engine.SetValue(group + "_" + dataDictionary.Name, dataDictionary.Id);
            }
            //engine.SetValue("DataDictionary", _dataDictionaryItems);
        }

        public async Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            _dataDictionaryItems = await _elsaCustomRepository.GetQuestionDataDictionaryListAsync(cancellationToken);

            var stringBuilder = new StringBuilder();

            //stringBuilder.Append("declare const DataDictionary = {");

            foreach (var dataDictionary in _dataDictionaryItems)
            {
                var group = dataDictionary.Group.Name.Replace(" ", "_");
                stringBuilder.Append("declare const " + group + "_" + dataDictionary.Name + " = null;");
                //stringBuilder.Append(group + "_" +
                //                     dataDictionary.Name + ": '" + dataDictionary.Id + "',");
            }

            //stringBuilder.Remove(stringBuilder.Length - 1, 1);
            //stringBuilder.Append("};");

            var dataDictionaryObject = stringBuilder.ToString();
            output.AppendLine(dataDictionaryObject);
        }
    }
}
