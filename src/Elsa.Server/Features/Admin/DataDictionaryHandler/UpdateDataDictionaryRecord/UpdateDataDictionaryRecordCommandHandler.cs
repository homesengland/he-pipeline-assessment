using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomInfrastructure.Migrations;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache;
using Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryRecord
{
    public class UpdateDataDictionaryRecordCommandHandler : IRequestHandler<UpdateDataDictionaryRecordCommand, OperationResult<UpdateDataDictionaryRecordCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<UpdateDataDictionaryRecordCommandHandler> _logger;
        private readonly IMediator _mediator;


        public UpdateDataDictionaryRecordCommandHandler(IMediator mediator,
            IElsaCustomRepository elsaCustomRepository,
            ILogger<UpdateDataDictionaryRecordCommandHandler> logger)
        {
            _mediator = mediator;
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<UpdateDataDictionaryRecordCommandResponse>> Handle(UpdateDataDictionaryRecordCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UpdateDataDictionaryRecordCommandResponse>();
            try
            {
                if (request.Record != null && !string.IsNullOrEmpty(request.Record.Name) && !string.IsNullOrEmpty(request.Record.LegacyName))
                {
                    await _elsaCustomRepository.UpdateDataDictionaryItem(request.Record, cancellationToken);
                    await _mediator.Send(new ClearDictionaryCacheCommand());
                }
                else
                {
                    throw new Exception("Data dictionary group could not be updated, becuase name or legacy name were invalid.");
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
