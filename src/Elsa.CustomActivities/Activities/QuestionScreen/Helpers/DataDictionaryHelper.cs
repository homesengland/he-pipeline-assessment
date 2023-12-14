using System.Text;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;

namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
{
    public class DataDictionaryHelper : INotificationHandler<EvaluatingJavaScriptExpression>, INotificationHandler<RenderingTypeScriptDefinitions>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public DataDictionaryHelper(IElsaCustomRepository elsaCustomRepository)
        {
            _elsaCustomRepository = elsaCustomRepository;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Handle(RenderingTypeScriptDefinitions notification, CancellationToken cancellationToken)
        {
            var output = notification.Output;
            var dataDictionaryItems = await _elsaCustomRepository.GetQuestionDataDictionaryListAsync(cancellationToken);

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("declare const DataDictionary = {");

            foreach (var dataDictionary in dataDictionaryItems)
            {
                var group = dataDictionary.Group.Name.Replace(" ", "_");
                stringBuilder.Append(group + "_" +
                                     dataDictionary.Name + ": '" + dataDictionary.Id + "',");
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append("};");

            var dataDictionaryObject = stringBuilder.ToString();
            output.AppendLine(dataDictionaryObject);
        }
    }
}
