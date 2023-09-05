using AutoMapper;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using Elsa.Serialization;
using StackExchange.Redis;

namespace Elsa.Server.Stores
{
    public class CachedEntityFrameworkWorkflowDefinitionStore : EntityFrameworkWorkflowDefinitionStore
    {
        private IConnectionMultiplexer _cache;
        public CachedEntityFrameworkWorkflowDefinitionStore(IElsaContextFactory dbContextFactory, IMapper mapper, IContentSerializer contentSerializer, IConnectionMultiplexer connectionMultiplexer) : base(dbContextFactory, mapper, contentSerializer)
        {
            _cache = connectionMultiplexer;
        }
    }
}
