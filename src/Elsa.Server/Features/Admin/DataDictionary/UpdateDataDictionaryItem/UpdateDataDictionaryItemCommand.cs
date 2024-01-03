using Elsa.CustomModels;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionary.UpdateDataDictionaryItem
{
    public class UpdateDataDictionaryItemCommand : IRequest<OperationResult<UpdateDataDictionaryItemCommandResponse>>
    {
        public QuestionDataDictionary? item { get; set; }
    }
}
