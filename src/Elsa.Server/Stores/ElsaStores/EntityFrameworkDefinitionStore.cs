using AutoMapper;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.EntityFramework.Core;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.Specifications;
using Elsa.Serialization;
using Elsa.Server.Models;
using LinqKit;
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

        private WorkflowDefinition OnLoading(WorkflowDefinition entity, string? json)
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
            return entity;
        }

        public virtual async Task UnpublishAll(string definitionId, CancellationToken token)
        {
            ElsaContext dbContext = DbContextFactory.CreateDbContext();
            await dbContext.WorkflowDefinitions
                .Where(dbDefinition => dbDefinition.DefinitionId == definitionId)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(d => d.IsPublished, false)
                .SetProperty(d => d.IsLatest, false));
            await dbContext.SaveChangesAsync(token);
        }

        public virtual async Task Unpublish(WorkflowDefinition definition, CancellationToken token)
        {
            ElsaContext dbContext = DbContextFactory.CreateDbContext();
            await dbContext.WorkflowDefinitions
                .Where(dbDefinition => dbDefinition.Id == definition.Id)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(d => d.IsPublished, false));
            await dbContext.SaveChangesAsync(token);
        }

        public virtual async Task RemoveLatest(WorkflowDefinition definition, CancellationToken token)
        {
            ElsaContext dbContext = DbContextFactory.CreateDbContext();
            await dbContext.WorkflowDefinitions
                .Where(dbDefinition => dbDefinition.DefinitionId == definition.DefinitionId)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(d => d.IsLatest, false));
            await dbContext.SaveChangesAsync(token);
        }

        //public virtual async Task<WorkflowDefinition?> FindAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        //{
        //    string? json = string.Empty;
        //    var filter = MapSpecification(specification);
        //    var data = await DoQuery(async dbContext =>
        //    {
        //        var dbSet = dbContext.Set<WorkflowDefinition>();
        //        var entity = await dbSet.AsNoTracking().Include("Data").FirstOrDefaultAsync(filter, cancellationToken);
        //        if(entity != null)
        //        {
        //            json = (string?)dbContext.Entry(entity).Property("Data").CurrentValue;
        //        }
        //        return entity != null ? OnLoading(entity, json) : default;
        //    }, cancellationToken);
        //    return data;
        //}

        public override async Task<WorkflowDefinition?> FindAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        {
            var filter = MapSpecification(specification);
            var data = await DoQuery(async dbContext =>
            {
                var dbSet = dbContext.Set<WorkflowDefinition>();

                // Use a projection to get both the entity and its Data property
                var result = await dbSet.AsNoTracking()
                    .Where(filter)
                    .Select(e => new
                    {
                        Entity = e,
                        Data = EF.Property<string>(e, "Data")
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (result == null)
                    return null;

                // Process the entity with the data
                return OnLoading(result.Entity, result.Data);
            }, cancellationToken);
            return data;
        }

        //private async Task<WorkflowDefinition?> FindDefinitionAsync(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        //{
        //    var filter = MapSpecification(specification);
        //    var data = await DoQuery(async dbContext =>
        //    {
        //        var dbSet = dbContext.Set<WorkflowDefinition>().AsNoTracking();
        //        var entity = await dbSet.FirstOrDefaultAsync(filter, cancellationToken);
        //        return entity;
        //    }, cancellationToken);
        //    return data;
        //}

        //private async Task<string> GetData(ISpecification<WorkflowDefinition> specification, CancellationToken cancellationToken = default)
        //{
        //    var filter = MapSpecification(specification);
        //    var data = await DoQuery(async dbContext =>
        //    {
        //        var json = await dbContext.
        //        var dbSet = dbContext.Set<WorkflowDefinition>().AsNoTracking();
        //        var entity = await dbSet.FirstOrDefaultAsync(filter, cancellationToken);
        //        return entity != null ? dbContext.Entry(entity).Property("Data").CurrentValue as string : null;
        //    }, cancellationToken);
        //    return data ?? string.Empty;
        //}

        public async Task<List<WorkflowDefinitionIdentifiers>> FindWorkflowDefinitionIdentifiersAsync(string definitionId, VersionOptions? options, CancellationToken cancellationToken = default)
        {
            ElsaContext dbContext = DbContextFactory.CreateDbContext();
            if(options != null)
            {
                var identifiers = await dbContext.WorkflowDefinitions.Where(d => d.DefinitionId == definitionId).WithVersion(options.Value).Select(dbContext => new WorkflowDefinitionIdentifiers
                {
                    Id = dbContext.Id,
                    DefinitionId = dbContext.DefinitionId,
                    Version = dbContext.Version,
                    IsLatest = dbContext.IsLatest,
                    IsPublished = dbContext.IsPublished
                }).ToListAsync();
                return identifiers;
            }
            else
            {
                var identifiers = await dbContext.WorkflowDefinitions.Where(d => d.DefinitionId == definitionId).Select(dbContext => new WorkflowDefinitionIdentifiers
                {
                    Id = dbContext.Id,
                    DefinitionId = dbContext.DefinitionId,
                    Version = dbContext.Version,
                    IsLatest = dbContext.IsLatest,
                    IsPublished = dbContext.IsPublished
                }).ToListAsync(cancellationToken);
                return identifiers;
            }
        }
    }
    public static class CustomWorkflowDefinitionVersionOptionsExtensions
    {

        public static Expression<Func<WorkflowDefinitionIdentifiers, bool>> WithVersion(this Expression<Func<WorkflowDefinitionIdentifiers, bool>> predicate, VersionOptions? version = default)
        {
            var versionOption = version ?? VersionOptions.Latest;

            if (versionOption.IsDraft)
                return predicate.And(x => !x.IsPublished);
            if (versionOption.IsLatest)
                return predicate.And(x => x.IsLatest);
            if (versionOption.IsPublished)
                return predicate.And(x => x.IsPublished);
            if (versionOption.IsLatestOrPublished)
                return predicate.And(x => x.IsPublished || x.IsLatest);
            if (versionOption.Version > 0)
                return predicate.And(x => x.Version == versionOption.Version);

            return predicate;
        }
    }
}
