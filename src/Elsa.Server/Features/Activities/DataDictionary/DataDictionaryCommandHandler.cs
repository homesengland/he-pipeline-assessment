using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.DataDictionaryHelpers;
using MediatR;
using Newtonsoft.Json;
using System.Text;

namespace Elsa.Server.Features.Activities.DataDictionaryProvider
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
                var dataDictionaryResult = (await _elsaCustomRepository.GetDataDictionaryGroupsAsync(request.IncludeArchived, cancellationToken)).ToList();
                var dataDictionaryList = await _elsaCustomRepository.GetDataDictionaryListAsync(false, cancellationToken);
                string dataDictionaryJsonResult =  JsonConvert.SerializeObject(dataDictionaryResult);
                string intellisenseLibrary = ToIntellisenseLibrary(dataDictionaryList);
                Dictionary<string, string> dictionaryResult = new Dictionary<string, string>()
                {
                    {"Dictionary", dataDictionaryJsonResult },
                    { "Intellisense", intellisenseLibrary }
                };

                string combinedResult = JsonConvert.SerializeObject(dictionaryResult);

                return await Task.FromResult(combinedResult);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error thrown whilst obtaining custom properties");
                return await Task.FromResult("");
            }
        }

        private string ToIntellisenseLibrary(List<DataDictionary> dictionaryItems)
        {
            var stringBuilder = new StringBuilder();

            foreach (var dataDictionary in dictionaryItems)
            {
                stringBuilder.Append(DataDictionaryToJavascriptHelper.JintDeclaration(dataDictionary.Group.Name, dataDictionary.Name, dataDictionary.Id));
                stringBuilder.Append("\n");
            }
            var dataDictionaryObject = stringBuilder.ToString();
            return dataDictionaryObject;
        }
    }
}
