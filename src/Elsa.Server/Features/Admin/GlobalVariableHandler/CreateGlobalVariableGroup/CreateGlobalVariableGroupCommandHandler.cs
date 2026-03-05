using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Admin.GlobalVariableHandler.ClearGlobalVariableCache;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.GlobalVariableHandler.CreateGlobalVariableGroup
{
    public class CreateGlobalVariableGroupCommandHandler : IRequestHandler<CreateGlobalVariableGroupCommand, OperationResult<CreateGlobalVariableGroupCommandResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILogger<CreateGlobalVariableGroupCommandHandler> _logger;
        private readonly IMediator _mediator;

        public CreateGlobalVariableGroupCommandHandler(IMediator mediator,
            IElsaCustomRepository elsaCustomRepository,
            ILogger<CreateGlobalVariableGroupCommandHandler> logger)
        {
            _mediator = mediator;
            _elsaCustomRepository = elsaCustomRepository;
            _logger = logger;
        }

        public async Task<OperationResult<CreateGlobalVariableGroupCommandResponse>> Handle(CreateGlobalVariableGroupCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<CreateGlobalVariableGroupCommandResponse>();
            try
            {
                if (!string.IsNullOrEmpty(request.Name))
                {
                    GlobalVariableGroup newGroup = new GlobalVariableGroup()
                    {
                        Name = request.Name,
                        Type = request.Type
                    };
                    int id = await _elsaCustomRepository.CreateGlobalVariableGroup(newGroup, cancellationToken);
                    await _mediator.Send(new ClearGlobalVariableCacheCommand());
                    result.Data = new CreateGlobalVariableGroupCommandResponse { Id = id };
                }
                else
                {
                    throw new Exception("Global variable group could not be created, becuase name was invalid.");
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
