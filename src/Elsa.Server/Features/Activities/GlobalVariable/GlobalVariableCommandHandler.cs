using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.DataDictionaryHelpers;
using Elsa.Server.Features.Activities.GlobalVariableProvider;
using MediatR;
using Newtonsoft.Json;
using System.Text;

namespace Elsa.Server.Features.Activities.DataDictionaryProvider
{
    public class GlobalVariableCommandHandler : IRequestHandler<GlobalVariableCommand, string>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<GlobalVariableCommandHandler> _logger;
        public GlobalVariableCommandHandler(ILogger<GlobalVariableCommandHandler> logger, IElsaCustomRepository elsaCustomRepository)
        {
            _logger = logger;
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<string> Handle(GlobalVariableCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var globalVariableResult = (await _elsaCustomRepository.GetGlobalVariableGroupsAsync(request.IncludeArchived, cancellationToken)).ToList();
                var globalVariableList = await _elsaCustomRepository.GetGlobalVariableListAsync(false, cancellationToken);
                string globalVariableJsonResult =  JsonConvert.SerializeObject(globalVariableResult);
                string intellisenseLibrary = ToIntellisenseLibrary(globalVariableList);
                Dictionary<string, string> dictionaryResult = new Dictionary<string, string>()
                {
                    {"Dictionary", globalVariableJsonResult },
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

        private string ToIntellisenseLibrary(List<GlobalVariable> globalVariableItems)
        {
            var stringBuilder = new StringBuilder();

            foreach (var dataDictionary in globalVariableItems)
            {
                //Data dictionary helper should work. Verify this and then rename to be more generic.
                stringBuilder.Append(DataDictionaryToJavascriptHelper.JintDeclaration(dataDictionary.Group.Name, dataDictionary.Name, dataDictionary.Id));
                stringBuilder.Append("\n");
            }
            var dataDictionaryObject = stringBuilder.ToString();
            return dataDictionaryObject;
        }
    }
}
