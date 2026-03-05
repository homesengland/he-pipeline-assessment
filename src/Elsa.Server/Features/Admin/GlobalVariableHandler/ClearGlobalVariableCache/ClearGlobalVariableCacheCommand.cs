using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Admin.GlobalVariableHandler.ClearGlobalVariableCache
{
    public class ClearGlobalVariableCacheCommand : IRequest<OperationResult<bool>>
    {
        public string CacheKey = "GlobalVariable";
    }
}
