using DotLiquid;
using Elsa.CustomWorkflow.Sdk.GlobalVariableHelpers;
using Elsa.Models;
using Elsa.Server.Features.Admin.GlobalVariableHandler.ClearGlobalVariableCache;
using Elsa.Server.Models;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using StackExchange.Redis;
using System.Linq.Expressions;

namespace Elsa.Server.Features.Admin.GlobalVariableHandler.ClearGlobalVariableCache
{
    public class ClearGlobalVariableCacheCommandHandler : IRequestHandler<ClearGlobalVariableCacheCommand, OperationResult<bool>>
    {
        private IGlobalVariableIntellisenseAccessor _accessor;
        private ILogger<ClearGlobalVariableCacheCommandHandler> _logger;
        public ClearGlobalVariableCacheCommandHandler(IGlobalVariableIntellisenseAccessor accessor, ILogger<ClearGlobalVariableCacheCommandHandler> logger) 
        {
            _accessor = accessor;
            _logger = logger;
        }
        public async Task<OperationResult<bool>> Handle(ClearGlobalVariableCacheCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _accessor.ClearGlobalVariableCache(request.CacheKey, cancellationToken);
                if (result)
                {
                    return new OperationResult<bool>
                    {
                        Data = result
                    };
                }
                else
                {
                    return new OperationResult<bool>
                    {
                        Data = false,
                        ErrorMessages = new List<string>()
                        {
                            $"Unable to clear cache using Key: {request.CacheKey}"
                        }
                    };
                }
            }
            catch (Exception ex)
            {

                string logMessage = "An Exception was thrown whilst clearing global variable cache";
                string exceptionMessage = ex.Message;
                string? innerException = ex.InnerException?.Message;

                _logger.LogError(ex, logMessage);
                var returnValue = new OperationResult<bool>
                {
                    Data = false,
                    ErrorMessages = new List<string>
                    {
                        logMessage,
                        exceptionMessage,

                    }
                };
                if(innerException != null)
                {
                    returnValue.ErrorMessages.Add(innerException);
                }
                return returnValue;
            }
        }
    }
}
