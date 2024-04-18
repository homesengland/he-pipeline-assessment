using Elsa.CustomModels;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.ArchiveDataDictionaryRecord
{
    public class ArchiveDataDictionaryRecordCommand : IRequest<OperationResult<ArchiveDataDictionaryRecordCommandResponse>>
    {
        public int Id { get; set; }
        public bool IsArchived { get; set; }
       
    }
}
