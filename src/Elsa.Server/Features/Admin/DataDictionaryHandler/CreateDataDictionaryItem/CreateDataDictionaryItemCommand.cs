using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryItem
{
    public class CreateDataDictionaryItemCommand : IRequest<OperationResult<CreateDataDictionaryItemCommandResponse>>
    {
        public string? Name {get; set;}
        public string? LegacyName { get; set; }
        public int DataDictionaryGroupId { get; set; }
    }
}
