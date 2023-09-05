using AutoMapper;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using Elsa.Serialization;
using StackExchange.Redis;

namespace Elsa.Server.Stores
{
    public class CachedEntityFrameworkWorkflowInstanceStore : EntityFrameworkWorkflowInstanceStore
    {
        private IConnectionMultiplexer _cache;
        public CachedEntityFrameworkWorkflowInstanceStore(IElsaContextFactory dbContextFactory, IMapper mapper, IContentSerializer contentSerializer, IConnectionMultiplexer connectionMultiplexer, ILogger<EntityFrameworkWorkflowInstanceStore> logger) : base(dbContextFactory, mapper, contentSerializer, logger)
        {
            _cache = connectionMultiplexer;
        }
    }
}
