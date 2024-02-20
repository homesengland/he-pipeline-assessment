using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomInfrastructure.Migrations;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryRecord
{
    public class ArchiveDataDictionaryRecordCommandHandler : IRequestHandler<ArchiveDataDictionaryRecordCommand, OperationResult<ArchiveDataDictionaryRecordCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<ArchiveDataDictionaryRecordCommandHandler> _logger;
        private readonly IMediator _mediator;


        public ArchiveDataDictionaryRecordCommandHandler(IMediator mediator,
            IElsaCustomRepository elsaCustomRepository,
            ILogger<ArchiveDataDictionaryRecordCommandHandler> logger)
        {
            _mediator = mediator;
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<ArchiveDataDictionaryRecordCommandResponse>> Handle(ArchiveDataDictionaryRecordCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<ArchiveDataDictionaryRecordCommandResponse>();
            try
            {
                await _elsaCustomRepository.ArchiveDataDictionaryItem(request.Id, request.IsArchived, cancellationToken);
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
