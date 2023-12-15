using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Describers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Threading;

namespace Elsa.Server.Features.Activities.DataDictionary
{
    public class DataDictionaryCommandHandler : IRequestHandler<DataDictionaryCommand, string>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<DataDictionaryCommandHandler> _logger;
        public DataDictionaryCommandHandler(ILogger<DataDictionaryCommandHandler> logger, IElsaCustomRepository elsaCustomRepository)
        {
            _logger = logger;
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<string> Handle(DataDictionaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dataDictionaryResult = (await _elsaCustomRepository.GetQuestionDataDictionaryGroupsAsync(cancellationToken)).ToList();
                string dataDictionaryJsonResult =  JsonConvert.SerializeObject(dataDictionaryResult);
                string intellisenseLibrary = ToIntellisenseLibrary(dataDictionaryResult);
                Dictionary<string, string> dictionaryResult = new Dictionary<string, string>()
                {
                    {"Dictionary", dataDictionaryJsonResult },
                    { "Intellisense", intellisenseLibrary }
                };

                string combinedResult = JsonConvert.SerializeObject(dictionaryResult);

                return await Task.FromResult(dataDictionaryJsonResult);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error thrown whilst obtaining custom properties");
                return await Task.FromResult("");
            }
        }

        private string ToIntellisenseLibrary(List<QuestionDataDictionaryGroup> group)
        {
            var stringBuilder = new StringBuilder();

            //stringBuilder.Append("declare const DataDictionary = {");

            foreach (var dataDictionary in group)
            {
                var groupName = dataDictionary.Name.Replace(" ", "_");
                stringBuilder.Append("declare const " + group + "_" + dataDictionary.Name + ": number;");
                stringBuilder.Append("\\n");
                //stringBuilder.Append(group + "_" +
                //                     dataDictionary.Name + ": '" + dataDictionary.Id + "',");
            }

            //stringBuilder.Remove(stringBuilder.Length - 1, 1);
            //stringBuilder.Append("};");

            var dataDictionaryObject = stringBuilder.ToString();
            return dataDictionaryObject;
        }
    }
}
