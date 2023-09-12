using AutoMapper;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using Elsa.Server.Api.Endpoints.WorkflowDefinitions;
using Elsa.Server.Api.Helpers;
using Elsa.Services;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Expressions;

namespace Elsa.Server.Features.Dashboard
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{apiVersion:apiVersion}/workflow-definitions/{definitionId}/CustomHistory")]
    [Produces("application/json")]
    public class CustomHistory : Controller
    {
        private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
        private readonly IMapper _mapper;
        private readonly ITenantAccessor _tenantAccessor;

        public CustomHistory(IWorkflowDefinitionStore workflowDefinitionStore, IMapper mapper, ITenantAccessor tenantAccessor)
        {
            _workflowDefinitionStore = workflowDefinitionStore;
            _mapper = mapper;
            _tenantAccessor = tenantAccessor;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<WorkflowDefinitionVersionModel>))]
        [SwaggerOperation(
            Summary = "Returns a list of workflow definition versions.",
            Description = "Returns a list of workflow definition versions.",
            OperationId = "WorkflowDefinitions.CustomHistory",
            Tags = new[] { "WorkflowDefinitions" })
        ]
        public async Task<IActionResult> Handle(string definitionId, CancellationToken cancellationToken = default)
        {
            var tenantId = await _tenantAccessor.GetTenantIdAsync(cancellationToken);
            var specification = GetSpecification(definitionId, tenantId);
            var definitions = await _workflowDefinitionStore.FindManyAsync(specification, new OrderBy<WorkflowDefinition>(x => x.Version, SortDirection.Descending), cancellationToken: cancellationToken);
            var versionModels = _mapper.Map<IList<WorkflowDefinitionVersionModel>>(definitions);

            var model = new
            {
                Items = versionModels
            };

            return Json(model, SerializationHelper.GetSettingsForWorkflowDefinition());
        }

        private VersionHistorySpecification GetSpecification(string definitionId, string? tenantId) => new VersionHistorySpecification(definitionId, tenantId, VersionOptions.All);


    }

    public class VersionHistorySpecification : Specification<WorkflowDefinition>
    {
        public VersionHistorySpecification(string workflowDefinitionId, string? tenantId, VersionOptions? options)
        {
            WorkflowDefinitionId = workflowDefinitionId;
            TenantId = tenantId;
            VersionOptions = options;
        }
        public string WorkflowDefinitionId { get; set; }
        public VersionOptions? VersionOptions { get; }
        public string? TenantId { get; set; }

        public override Expression<Func<WorkflowDefinition, bool>> ToExpression()
        {
            Expression<Func<WorkflowDefinition, bool>> predicate = x => x.DefinitionId == WorkflowDefinitionId;

            if (!string.IsNullOrWhiteSpace(TenantId))
                predicate = predicate.And(x => x.TenantId == TenantId);

            if (VersionOptions != null)
                predicate = predicate.WithVersion(VersionOptions.Value);

            return predicate;
        }
    }

    public class CustomWorkflowDefinition : Entity
    {
        public string? DisplayName { get; set; }
        public int Version { get; set; }
        public Instant CreatedAt { get; set; }
    }
}