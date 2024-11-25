using AutoMapper;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using StackExchange.Redis;

namespace Elsa.Server.Stores
{
    public class CachedEntityFrameworkWorkflowExecutionLogRecordStore : EntityFrameworkWorkflowExecutionLogRecordStore
    {
        private IConnectionMultiplexer _cache;
        public CachedEntityFrameworkWorkflowExecutionLogRecordStore(IElsaContextFactory dbContextFactory, IMapper mapper, IConnectionMultiplexer connectionMultiplexer, ILogger<EntityFrameworkWorkflowExecutionLogRecordStore> logger) : base(dbContextFactory, mapper, logger)
        {
            _cache = connectionMultiplexer;
        }
    }
}
