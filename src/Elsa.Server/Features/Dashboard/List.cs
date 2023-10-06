using AutoMapper;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications;
using Elsa.Server.Api.Endpoints.WorkflowDefinitions;
using Elsa.Server.Api.Helpers;
using Elsa.Server.Api.Models;
using Elsa.Server.Api.Swagger.Examples;
using Elsa.Services;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Linq.Expressions;

namespace Elsa.Server.Features.Dashboard
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{apiVersion:apiVersion}/custom-workflow-definitions")]
    [Produces("application/json")]
    public class CustomList : Controller
    {
        private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
        private readonly IMapper _mapper;
        private readonly ITenantAccessor _tenantAccessor;

        public CustomList(IWorkflowDefinitionStore workflowDefinitionStore, IMapper mapper, ITenantAccessor tenantAccessor)
        {
            _workflowDefinitionStore = workflowDefinitionStore;
            _mapper = mapper;
            _tenantAccessor = tenantAccessor;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<WorkflowDefinitionSummaryModel>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(WorkflowDefinitionPagedListExample))]
        [SwaggerOperation(
            Summary = "Returns a list of workflow definitions.",
            Description = "Returns paginated a list of workflow definition summaries. When no version options are specified, the latest versions are returned.",
            OperationId = "WorkflowDefinitions.List",
            Tags = new[] { "WorkflowDefinitions" })
        ]
        public async Task<ActionResult<PagedList<WorkflowDefinitionSummaryModel>>> Handle(
            [FromQuery] string? ids,
            [FromQuery] string? searchTerm = default,
            int? page = default,
            int? pageSize = default,
            VersionOptions? version = default,
            CancellationToken cancellationToken = default)
        {
            var tenantId = await _tenantAccessor.GetTenantIdAsync(cancellationToken);
            version ??= VersionOptions.Latest;
            var paging = page == null || pageSize == null ? default : Paging.Page(page.Value, pageSize.Value);
            var specification = GetSpecification(ids, version.Value, tenantId, searchTerm);

            var totalCount = await _workflowDefinitionStore.CountAsync(specification, cancellationToken);

            var items = await _workflowDefinitionStore.FindManyAsync(specification, new OrderBy<WorkflowDefinition>(x => x.Name!, SortDirection.Ascending), paging, cancellationToken);
            var summaries = _mapper.Map<IList<WorkflowDefinitionSummaryModel>>(items);
            var pagedList = new PagedList<WorkflowDefinitionSummaryModel>(summaries, page, pageSize, totalCount);

            return Json(pagedList, SerializationHelper.GetSettingsForWorkflowDefinition());
        }

        private WorkflowDefinitionListSpecification GetSpecification(string? ids, VersionOptions version,
            string? tenantId, string? searchTerm) => new(ids, version, tenantId, searchTerm);

    }

    public class WorkflowDefinitionListSpecification : Specification<WorkflowDefinition>
    {
        public WorkflowDefinitionListSpecification(string? ids, VersionOptions version, string? tenantId,
            string? searchTerm)
        {
            WorkflowDefinitionIds = ids;
            VersionOptions = version;
            TenantId = tenantId;
            SearchTerm = searchTerm;
        }
        public string? WorkflowDefinitionIds { get; set; }
        public VersionOptions? VersionOptions { get; }
        public string? TenantId { get; set; }
        public string? SearchTerm { get; set; }

        public override Expression<Func<WorkflowDefinition, bool>> ToExpression()
        {
            Expression<Func<WorkflowDefinition, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(WorkflowDefinitionIds))
            {
                var splitIds = WorkflowDefinitionIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
                predicate = x => splitIds.Contains(x.DefinitionId);
            }

            if (!string.IsNullOrWhiteSpace(TenantId))
                predicate = predicate.And(x => x.TenantId == TenantId);

            if (VersionOptions != null)
                predicate = predicate.WithVersion(VersionOptions.Value);

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                predicate = predicate.And(x => x.Name!.Contains(SearchTerm) ||
                                               x.DisplayName!.Contains(SearchTerm) ||
                                               x.Description!.Contains(SearchTerm));
            }

            return predicate;
        }
    }
}
