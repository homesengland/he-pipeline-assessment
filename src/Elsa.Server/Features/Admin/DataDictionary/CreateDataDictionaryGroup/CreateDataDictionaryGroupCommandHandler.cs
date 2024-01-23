using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.DataDictionary.ClearDictionaryCache;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryGroup
{
    public class CreateDataDictionaryGroupCommandHandler : IRequestHandler<CreateDataDictionaryGroupCommand, OperationResult<CreateDataDictionaryGroupCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<CreateDataDictionaryGroupCommandHandler> _logger;
        private readonly IMediator _mediator;

        public CreateDataDictionaryGroupCommandHandler(IMediator mediator,
            IElsaCustomRepository elsaCustomRepository,
            ILogger<CreateDataDictionaryGroupCommandHandler> logger)
        {
            _mediator = mediator;
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<CreateDataDictionaryGroupCommandResponse>> Handle(CreateDataDictionaryGroupCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<CreateDataDictionaryGroupCommandResponse>();
            try
            {
                if (!string.IsNullOrEmpty(request.Name))
                {
                    QuestionDataDictionaryGroup newGroup = new QuestionDataDictionaryGroup()
                    {
                        Name = request.Name
                    };
                    await _elsaCustomRepository.CreateDataDictionaryGroup(newGroup, cancellationToken);
                    await _mediator.Send(new ClearDictionaryCacheCommand());
                }
                else
                {
                    throw new Exception("Data dictionary group could not be created, becuase name was invalid.");
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
