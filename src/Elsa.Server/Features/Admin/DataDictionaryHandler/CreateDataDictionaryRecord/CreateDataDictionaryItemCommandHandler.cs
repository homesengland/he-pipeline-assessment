using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryRecord
{ 
    public class CreateDataDictionaryRecordCommandHandler : IRequestHandler<CreateDataDictionaryRecordCommand, OperationResult<CreateDataDictionaryRecordCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<CreateDataDictionaryRecordCommandHandler> _logger;
        private readonly IMediator _mediator;

        public CreateDataDictionaryRecordCommandHandler(IMediator mediator,
            IElsaCustomRepository elsaCustomRepository,
            ILogger<CreateDataDictionaryRecordCommandHandler> logger)
        {
            _mediator = mediator;
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<CreateDataDictionaryRecordCommandResponse>> Handle(CreateDataDictionaryRecordCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<CreateDataDictionaryRecordCommandResponse>();
            try
            {
                if (request.DictionaryRecord != null && !string.IsNullOrEmpty(request.DictionaryRecord.Name) && request.DictionaryRecord.DataDictionaryGroupId != 0)
                {
                    DataDictionary newGroup = new DataDictionary()
                    {
                        Name = request.DictionaryRecord!.Name,
                        LegacyName = request.DictionaryRecord.LegacyName,
                        Type = request.DictionaryRecord.Type,
                        Description = request.DictionaryRecord.Description,
                        DataDictionaryGroupId = request.DictionaryRecord.DataDictionaryGroupId
                       
                    };
                    int id = await _elsaCustomRepository.CreateDataDictionaryItem(newGroup, cancellationToken);
                    await _mediator.Send(new ClearDictionaryCacheCommand());
                    result.Data = new CreateDataDictionaryRecordCommandResponse { Id = id };
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
