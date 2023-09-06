using AutoMapper;
using Elsa.Models;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using Elsa.Persistence.EntityFramework.Core;

namespace Elsa.Server.Stores.ElsaStores
{
    public abstract class ElsaContextEntityFrameworkStore<T> : EntityFrameworkStore<T, ElsaContext> where T : class, IEntity
    {
        protected ElsaContextEntityFrameworkStore(IContextFactory<ElsaContext> dbContextFactory, IMapper mapper) : base(dbContextFactory, mapper)
        {
        }
    }
}
