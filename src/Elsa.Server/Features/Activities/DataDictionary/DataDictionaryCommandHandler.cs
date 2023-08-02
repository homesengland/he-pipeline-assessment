using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Describers;
using Elsa.CustomInfrastructure.Data.Repository;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

                return await Task.FromResult(dataDictionaryJsonResult);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error thrown whilst obtaining custom properties");
                return await Task.FromResult("");
            }
        }
    }
}
