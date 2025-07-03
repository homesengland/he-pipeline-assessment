using Elsa.CustomWorkflow.Sdk.DataDictionaryHelpers;
using Elsa.Models;
using Elsa.Server.Models;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using StackExchange.Redis;
using System.Linq.Expressions;

namespace Elsa.Server.Features.Admin.DataDictionaryHandler.ClearDictionaryCache
{
    public class ClearDictionaryCacheCommandHandler : IRequestHandler<ClearDictionaryCacheCommand, OperationResult<bool>>
    {
        private IDataDictionaryIntellisenseAccessor _accessor;
        private ILogger<ClearDictionaryCacheCommandHandler> _logger;
        public ClearDictionaryCacheCommandHandler(IDataDictionaryIntellisenseAccessor accessor, ILogger<ClearDictionaryCacheCommandHandler> logger) 
        {
            _accessor = accessor;
            _logger = logger;
        }
        public async Task<OperationResult<bool>> Handle(ClearDictionaryCacheCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _accessor.ClearDictionaryCache(request.CacheKey, cancellationToken);
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

                string logMessage = "An Exception was thrown whilst clearing data dictionary cache";
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
