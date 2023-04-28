using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Describers;
using Elsa.CustomInfrastructure.Data.Repository;
using MediatR;
using Newtonsoft.Json;

namespace Elsa.Server.Features.Activities.CustomActivityProperties
{
    public class CustomPropertyCommandHandler : IRequestHandler<CustomPropertyCommand, Dictionary<string, string>>
    {
        private readonly ICustomPropertyDescriber _describer;
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<CustomPropertyCommandHandler> _logger;
        public CustomPropertyCommandHandler(ICustomPropertyDescriber describer, ILogger<CustomPropertyCommandHandler> logger, IElsaCustomRepository elsaCustomRepository)
        {
            _describer = describer;
            _logger = logger;
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<Dictionary<string, string>> Handle(CustomPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = (await _elsaCustomRepository.GetQuestionDataDictionaryGroupsAsync(cancellationToken)).ToList();
                var jsonResult=  JsonConvert.SerializeObject(result);

                Dictionary<string, string> propertyResponses = new Dictionary<string, string>();
                propertyResponses.Add("QuestionProperties", JsonConvert.SerializeObject(_describer.DescribeInputProperties(typeof(Question))));
                propertyResponses.Add("DataDictionaryGroup", jsonResult);
                
                return await Task.FromResult(propertyResponses);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error thrown whilst obtaining custom properties");
                return await Task.FromResult(new Dictionary<string, string>());
            }
        }
    }
}
