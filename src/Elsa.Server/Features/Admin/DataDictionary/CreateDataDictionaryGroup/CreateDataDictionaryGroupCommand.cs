using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryGroup
{
    public class CreateDataDictionaryGroupCommand : IRequest<OperationResult<CreateDataDictionaryGroupCommandResponse>>
    {
        public String? Name {get; set;}
    }
}
