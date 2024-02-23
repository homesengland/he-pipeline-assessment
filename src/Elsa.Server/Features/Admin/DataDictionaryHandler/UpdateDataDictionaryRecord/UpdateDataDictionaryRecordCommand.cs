using Elsa.CustomModels;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.UpdateDataDictionaryRecord
{
    public class UpdateDataDictionaryRecordCommand : IRequest<OperationResult<UpdateDataDictionaryRecordCommandResponse>>
    {
        public DataDictionary? Record { get; set; }
    }
}
