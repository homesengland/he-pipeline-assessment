using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomInfrastructure.Migrations;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryGroup
{
    public class UpdateDataDictionaryGroupCommandHandler : IRequestHandler<UpdateDataDictionaryGroupCommand, OperationResult<UpdateDataDictionaryGroupCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<UpdateDataDictionaryGroupCommandHandler> _logger;
        private readonly IMediator _mediator;

        public UpdateDataDictionaryGroupCommandHandler(IMediator mediator,
            IElsaCustomRepository elsaCustomRepository,
            ILogger<UpdateDataDictionaryGroupCommandHandler> logger)
        {
            _mediator = mediator;
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<UpdateDataDictionaryGroupCommandResponse>> Handle(UpdateDataDictionaryGroupCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UpdateDataDictionaryGroupCommandResponse>();
            try
            {
                if (request.group != null && !string.IsNullOrEmpty(request.group.Name))
                {
                    await _elsaCustomRepository.UpdateDataDictionaryGroup(request.group, cancellationToken);
                    await _mediator.Send(new ClearDictionaryCacheCommand());
                }
                else
                {
                    throw new Exception("Data dictionary group could not be updated, becuase name was invalid.");
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
