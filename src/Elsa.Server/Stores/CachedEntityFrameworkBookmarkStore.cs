using AutoMapper;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using StackExchange.Redis;

namespace Elsa.Server.Stores
{
    public class CachedEntityFrameworkBookmarkStore : EntityFrameworkBookmarkStore
    {
        private IConnectionMultiplexer _cache;
        public CachedEntityFrameworkBookmarkStore(IElsaContextFactory dbContextFactory, IMapper mapper, IConnectionMultiplexer connectionMultiplexer, ILogger<EntityFrameworkBookmarkStore> logger) : base(dbContextFactory, mapper, logger)
        {
            _cache = connectionMultiplexer;
        }
    }
}
