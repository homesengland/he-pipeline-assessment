using Elsa.CustomModels;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.CreateDataDictionaryRecord
{
    public class CreateDataDictionaryRecordCommand : IRequest<OperationResult<CreateDataDictionaryRecordCommandResponse>>
    {
        public DataDictionary? DictionaryRecord { get; set; } 
    }
}
