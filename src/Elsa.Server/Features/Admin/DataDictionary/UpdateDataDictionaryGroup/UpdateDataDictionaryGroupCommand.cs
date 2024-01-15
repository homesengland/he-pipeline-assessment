using Elsa.CustomModels;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionary.UpdateDataDictionaryGroup
{
    public class UpdateDataDictionaryGroupCommand : IRequest<OperationResult<UpdateDataDictionaryGroupCommandResponse>>
    {
        public QuestionDataDictionaryGroup? group { get; set; }
    }
}
