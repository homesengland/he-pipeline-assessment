using AutoMapper;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using Elsa.Persistence.EntityFramework.Core;
using Elsa.Persistence.Specifications;
using Elsa.Persistence;
using Elsa.Serialization;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Elsa.Models;

namespace Elsa.Server.Stores.ElsaStores
{
        public class EntityFrameworkWorkflowDefinitionStore : ElsaContextEntityFrameworkStore<WorkflowDefinition>, IWorkflowDefinitionStore
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

                var json = (string)dbContext.Entry(entity).Property("Data").CurrentValue;
                data = JsonConvert.DeserializeAnonymousType(json, data, DefaultContentSerializer.CreateDefaultJsonSerializationSettings())!;

                entity.Activities = data.Activities;
                entity.Connections = data.Connections;
                entity.Variables = data.Variables;
                entity.ContextOptions = data.ContextOptions;
                entity.CustomAttributes = data.CustomAttributes;
                entity.Channel = data.Channel;
            }
        }
    }
