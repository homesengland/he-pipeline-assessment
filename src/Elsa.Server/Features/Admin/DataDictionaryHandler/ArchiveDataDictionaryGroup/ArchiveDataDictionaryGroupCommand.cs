using Elsa.CustomModels;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryGroup
{
    public class ArchiveDataDictionaryGroupCommand : IRequest<OperationResult<ArchiveDataDictionaryGroupCommandResponse>>
    {
        public int Id { get; set; }
        public bool IsArchived { get; set; }
       
    }
}
