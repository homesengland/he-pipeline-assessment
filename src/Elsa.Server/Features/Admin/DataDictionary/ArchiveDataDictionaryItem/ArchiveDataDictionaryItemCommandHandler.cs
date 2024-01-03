using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomInfrastructure.Migrations;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryGroup;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionary.ArchiveDataDictionaryItem
{
    public class ArchiveDataDictionaryItemCommandHandler : IRequestHandler<ArchiveDataDictionaryItemCommand, OperationResult<ArchiveDataDictionaryItemCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<ArchiveDataDictionaryItemCommandHandler> _logger;


        public ArchiveDataDictionaryItemCommandHandler(
            IElsaCustomRepository elsaCustomRepository,
            ILogger<ArchiveDataDictionaryItemCommandHandler> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<ArchiveDataDictionaryItemCommandResponse>> Handle(ArchiveDataDictionaryItemCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<ArchiveDataDictionaryItemCommandResponse>();
            try
            {
                await _elsaCustomRepository.ArchiveDataDictionaryItem(request.id, cancellationToken);
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
