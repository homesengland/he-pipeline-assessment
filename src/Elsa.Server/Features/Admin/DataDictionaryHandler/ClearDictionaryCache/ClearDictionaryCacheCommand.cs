using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache
{
    public class ClearDictionaryCacheCommand : IRequest<OperationResult<bool>>
    {
        public string CacheKey = "DataDictionary";
    }
}
