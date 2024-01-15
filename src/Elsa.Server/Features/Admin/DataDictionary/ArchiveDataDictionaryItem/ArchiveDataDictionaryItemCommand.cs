using Elsa.CustomModels;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionary.ArchiveDataDictionaryItem
{
    public class ArchiveDataDictionaryItemCommand : IRequest<OperationResult<ArchiveDataDictionaryItemCommandResponse>>
    {
        public int Id { get; set; }
       
    }
}
