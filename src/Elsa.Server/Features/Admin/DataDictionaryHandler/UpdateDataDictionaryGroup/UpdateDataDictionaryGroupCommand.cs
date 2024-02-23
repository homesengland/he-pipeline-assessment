using Elsa.CustomModels;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryGroup
{
    public class UpdateDataDictionaryGroupCommand : IRequest<OperationResult<UpdateDataDictionaryGroupCommandResponse>>
    {
        public DataDictionaryGroup? group { get; set; }
    }
}
