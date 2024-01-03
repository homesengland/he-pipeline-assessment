using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionary.CreateDataDictionaryItem
{
    public class CreateDataDictionaryItemCommand : IRequest<OperationResult<CreateDataDictionaryItemCommandResponse>>
    {
        public String? Name {get; set;}
        public String? LegacyName { get; set; }
        public int DataDictionaryGroupId { get; set; }
    }
}
