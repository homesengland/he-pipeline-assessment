using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomInfrastructure.Migrations;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryGroup;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionary.UpdateDataDictionaryItem
{
    public class UpdateDataDictionaryItemCommandHandler : IRequestHandler<UpdateDataDictionaryItemCommand, OperationResult<UpdateDataDictionaryItemCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<UpdateDataDictionaryItemCommandHandler> _logger;


        public UpdateDataDictionaryItemCommandHandler(
            IElsaCustomRepository elsaCustomRepository,
            ILogger<UpdateDataDictionaryItemCommandHandler> logger)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<UpdateDataDictionaryItemCommandResponse>> Handle(UpdateDataDictionaryItemCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UpdateDataDictionaryItemCommandResponse>();
            try
            {
                if (request.item != null)
                {
                    await _elsaCustomRepository.UpdateDataDictionaryItem(request.item, cancellationToken);
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
