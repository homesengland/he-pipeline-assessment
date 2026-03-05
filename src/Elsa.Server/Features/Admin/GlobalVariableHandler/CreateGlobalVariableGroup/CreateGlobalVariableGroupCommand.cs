using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.GlobalVariableHandler.CreateGlobalVariableGroup
{
    public class CreateGlobalVariableGroupCommand : IRequest<OperationResult<CreateGlobalVariableGroupCommandResponse>>
    {
        public string? Name {get; set;}
        public string? Type { get; set; }
    }
}
