using AutoMapper;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using StackExchange.Redis;

namespace Elsa.Server.Stores
{
    public class CachedEntityFrameworkTriggerStore : EntityFrameworkTriggerStore
    {
        private IConnectionMultiplexer _cache;
        public CachedEntityFrameworkTriggerStore(IElsaContextFactory dbContextFactory, IMapper mapper, IConnectionMultiplexer connectionMultiplexer) : base(dbContextFactory, mapper)
        {
            _cache = connectionMultiplexer;
        }
    }
}
