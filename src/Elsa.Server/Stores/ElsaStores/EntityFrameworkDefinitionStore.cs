using AutoMapper;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.EntityFramework.Core;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.Specifications;
using Elsa.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Elsa.Server.Stores.ElsaStores
{
        public class EntityFrameworkWorkflowDefinitionStore : ElsaContextEntityFrameworkStore<WorkflowDefinition>, ICustomWorkflowDefinitionStore
        {
            private readonly IContentSerializer _contentSerializer;

            public EntityFrameworkWorkflowDefinitionStore(IElsaContextFactory dbContextFactory, IMapper mapper, IContentSerializer contentSerializer) : base(dbContextFactory, mapper)
            {
                _contentSerializer = contentSerializer;
            }

            protected override Expression<Func<WorkflowDefinition, bool>> MapSpecification(ISpecification<WorkflowDefinition> specification) => AutoMapSpecification(specification);

            protected override void OnSaving(ElsaContext dbContext, WorkflowDefinition entity)
            {
                var data = new
                {
                    entity.Activities,
                    entity.Connections,
                    entity.Variables,
                    entity.ContextOptions,
                    entity.CustomAttributes,
                    entity.Channel
                };

                var json = _contentSerializer.Serialize(data);
                dbContext.Entry(entity).Property("Data").CurrentValue = json;
            }

        protected override void OnLoading(ElsaContext dbContext, WorkflowDefinition entity)
        {
            var data = new
            {
                entity.Activities,
                entity.Connections,
                entity.Variables,
                entity.ContextOptions,
                entity.CustomAttributes,
                entity.Channel
            };

            string? json = (string?)dbContext.Entry(entity).Property("Data").CurrentValue;
            if (json != null)
            {
                data = JsonConvert.DeserializeAnonymousType(json, data, DefaultContentSerializer.CreateDefaultJsonSerializationSettings())!;

                entity.Activities = data.Activities;
                entity.Connections = data.Connections;
                entity.Variables = data.Variables;
                entity.ContextOptions = data.ContextOptions;
                entity.CustomAttributes = data.CustomAttributes;
                entity.Channel = data.Channel;
            }
        }

        public async Task OnUnpublishDefinitions(string definitionId, CancellationToken token)
        {
            ElsaContext dbContext = DbContextFactory.CreateDbContext();
            await dbContext.WorkflowDefinitions
                .Where(dbDefinition => dbDefinition.DefinitionId == definitionId)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(d => d.IsPublished, false)
                .SetProperty(d => d.IsLatest, false));
            await dbContext.SaveChangesAsync(token);
        }
    }
    }
