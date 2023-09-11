﻿using AutoMapper;
using Elsa.Persistence.EntityFramework.Core.Services;
using Elsa.Persistence.EntityFramework.Core.Stores;
using Elsa.Persistence.EntityFramework.Core;
using Elsa.Persistence.Specifications;
using Elsa.Persistence;
using Elsa.Serialization;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Elsa.Models;
using Elsa.Persistence.EntityFramework.Core.Extensions;

namespace Elsa.Server.Stores.ElsaStores
{
    public class EntityFrameworkWorkflowInstanceStore : ElsaContextEntityFrameworkStore<WorkflowInstance>, IWorkflowInstanceStore
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<EntityFrameworkWorkflowInstanceStore> _logger;

        public EntityFrameworkWorkflowInstanceStore(
            IElsaContextFactory dbContextFactory,
            IMapper mapper,
            IContentSerializer contentSerializer,
            ILogger<EntityFrameworkWorkflowInstanceStore> logger) : base(dbContextFactory, mapper)
        {
            _contentSerializer = contentSerializer;
            _logger = logger;
        }

        public override async Task DeleteAsync(WorkflowInstance entity, CancellationToken cancellationToken = default)
        {
            var workflowInstanceId = entity.Id;

            await DoWork(async dbContext =>
            {
                await dbContext.Set<WorkflowExecutionLogRecord>().AsQueryable().Where(x => x.WorkflowInstanceId == workflowInstanceId).BatchDeleteWithWorkAroundAsync(dbContext, cancellationToken);
                await dbContext.Set<Bookmark>().AsQueryable().Where(x => x.WorkflowInstanceId == workflowInstanceId).BatchDeleteWithWorkAroundAsync(dbContext, cancellationToken);
                await dbContext.Set<WorkflowInstance>().AsQueryable().Where(x => x.Id == workflowInstanceId).BatchDeleteWithWorkAroundAsync(dbContext, cancellationToken);
            }, cancellationToken);
        }

        public override async Task<int> DeleteManyAsync(ISpecification<WorkflowInstance> specification, CancellationToken cancellationToken = default)
        {
            var workflowInstanceIds = (await FindManyAsync<string>(specification, (wf) => wf.Id, cancellationToken: cancellationToken)).ToList();
            await DeleteManyByIdsAsync(workflowInstanceIds, cancellationToken);
            return workflowInstanceIds.Count;
        }

        public async Task DeleteManyByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            var idList = ids.ToList();

            if (!idList.Any())
                return;

            await DoWork(async dbContext =>
            {
                await dbContext.Set<WorkflowExecutionLogRecord>().AsQueryable().Where(x => idList.Contains(x.WorkflowInstanceId)).BatchDeleteWithWorkAroundAsync(dbContext, cancellationToken);
                await dbContext.Set<Bookmark>().AsQueryable().Where(x => idList.Contains(x.WorkflowInstanceId)).BatchDeleteWithWorkAroundAsync(dbContext, cancellationToken);
                await dbContext.Set<WorkflowInstance>().AsQueryable().Where(x => idList.Contains(x.Id)).BatchDeleteWithWorkAroundAsync(dbContext, cancellationToken);
            }, cancellationToken);
        }

        protected override Expression<Func<WorkflowInstance, bool>> MapSpecification(ISpecification<WorkflowInstance> specification)
        {
            return AutoMapSpecification(specification);
        }

        protected override void OnSaving(ElsaContext dbContext, WorkflowInstance entity)
        {
            var data = new
            {
                entity.Input,
                entity.Output,
                entity.Variables,
                entity.ActivityData,
                entity.Metadata,
                entity.BlockingActivities,
                entity.ScheduledActivities,
                entity.Scopes,
                entity.Faults,
                entity.CurrentActivity
            };

            var json = _contentSerializer.Serialize(data);

            dbContext.Entry(entity).Property("Data").CurrentValue = json;
        }

        protected override void OnLoading(ElsaContext dbContext, WorkflowInstance entity)
        {
            var data = new
            {
                entity.Input,
                entity.Output,
                entity.Variables,
                entity.ActivityData,
                entity.Metadata,
                entity.BlockingActivities,
                entity.ScheduledActivities,
                entity.Scopes,
                entity.Faults,
                entity.CurrentActivity
            };

            var json = (string?)dbContext.Entry(entity).Property("Data").CurrentValue;

            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    data = JsonConvert.DeserializeAnonymousType(json, data, DefaultContentSerializer.CreateDefaultJsonSerializationSettings())!;
                }
                catch (JsonSerializationException e)
                {
                    _logger.LogWarning(e, "Failed to deserialize workflow instance JSON data");
                }
            }

            entity.Input = data.Input;
            entity.Output = data.Output;
            entity.Variables = data.Variables;
            entity.ActivityData = data.ActivityData;
            entity.Metadata = data.Metadata;
            entity.BlockingActivities = data.BlockingActivities;
            entity.ScheduledActivities = data.ScheduledActivities;
            entity.Scopes = data.Scopes;
            entity.Faults = data.Faults;
            entity.CurrentActivity = data.CurrentActivity;
        }
    }
}
