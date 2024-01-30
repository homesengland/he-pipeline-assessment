using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryGroup
{
    public class CreateDataDictionaryGroupCommand : IRequest<OperationResult<CreateDataDictionaryGroupCommandResponse>>
    {
        public string? Name {get; set;}
    }
}
