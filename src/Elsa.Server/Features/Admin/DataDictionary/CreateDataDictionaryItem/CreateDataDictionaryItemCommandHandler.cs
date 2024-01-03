using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryItem 
{ 
    public class CreateDataDictionaryItemCommandHandler : IRequestHandler<CreateDataDictionaryItemCommand, OperationResult<CreateDataDictionaryItemCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<CreateDataDictionaryItemCommandHandler> _logger;


        public CreateDataDictionaryItemCommandHandler(
            IElsaCustomRepository elsaCustomRepository,
            ILogger<CreateDataDictionaryItemCommandHandler> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<CreateDataDictionaryItemCommandResponse>> Handle(CreateDataDictionaryItemCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<CreateDataDictionaryItemCommandResponse>();
            try
            {
                if (!string.IsNullOrEmpty(request.Name))
                {
                    QuestionDataDictionary newGroup = new QuestionDataDictionary()
                    {
                        Name = request.Name,
                        LegacyName = request.LegacyName,
                        QuestionDataDictionaryGroupId = request.DataDictionaryGroupId
                       
                    };
                    await _elsaCustomRepository.CreateDataDictionaryItem(newGroup, cancellationToken);
                }
                else
                {
                    throw new Exception("Data dictionary item could not be created, becuase name was invalid.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }
    }
}
