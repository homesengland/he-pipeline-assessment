using Elsa.CustomModels;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryItem
{
    public class UpdateDataDictionaryItemCommand : IRequest<OperationResult<UpdateDataDictionaryItemCommandResponse>>
    {
        public DataDictionary? Item { get; set; }
    }
}
