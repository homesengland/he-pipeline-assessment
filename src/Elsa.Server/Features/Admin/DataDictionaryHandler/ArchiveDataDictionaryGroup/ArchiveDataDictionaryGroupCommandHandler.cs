using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomInfrastructure.Migrations;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryGroup
{
    public class ArchiveDataDictionaryGroupCommandHandler : IRequestHandler<ArchiveDataDictionaryGroupCommand, OperationResult<ArchiveDataDictionaryGroupCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<ArchiveDataDictionaryGroupCommandHandler> _logger;
        private readonly IMediator _mediator;


        public ArchiveDataDictionaryGroupCommandHandler(IMediator mediator,
            IElsaCustomRepository elsaCustomRepository,
            ILogger<ArchiveDataDictionaryGroupCommandHandler> logger)
        {
            _mediator = mediator;
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<ArchiveDataDictionaryGroupCommandResponse>> Handle(ArchiveDataDictionaryGroupCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<ArchiveDataDictionaryGroupCommandResponse>();
            try
            {
                await _elsaCustomRepository.ArchiveDataDictionaryGroup(request.Id, cancellationToken);
                await _mediator.Send(new ClearDictionaryCacheCommand());
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
