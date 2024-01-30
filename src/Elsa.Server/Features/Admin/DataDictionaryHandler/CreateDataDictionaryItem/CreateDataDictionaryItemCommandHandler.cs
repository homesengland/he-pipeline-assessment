using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryItem 
{ 
    public class CreateDataDictionaryItemCommandHandler : IRequestHandler<CreateDataDictionaryItemCommand, OperationResult<CreateDataDictionaryItemCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<CreateDataDictionaryItemCommandHandler> _logger;
        private readonly IMediator _mediator;

        public CreateDataDictionaryItemCommandHandler(IMediator mediator,
            IElsaCustomRepository elsaCustomRepository,
            ILogger<CreateDataDictionaryItemCommandHandler> logger)
        {
            _mediator = mediator;
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
                    DataDictionary newGroup = new DataDictionary()
                    {
                        Name = request.Name,
                        LegacyName = request.LegacyName,
                        DataDictionaryGroupId = request.DataDictionaryGroupId
                       
                    };
                    int id = await _elsaCustomRepository.CreateDataDictionaryItem(newGroup, cancellationToken);
                    await _mediator.Send(new ClearDictionaryCacheCommand());
                    result.Data!.Id = id;
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
