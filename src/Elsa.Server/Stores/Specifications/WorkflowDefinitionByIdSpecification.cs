using Elsa.Models;
using Elsa.Persistence.Specifications;
using LinqKit;
using System.Linq.Expressions;

namespace Elsa.Server.Stores.Specifications
{
    public class WorkflowDefinitionByIdSpecification : Specification<WorkflowDefinition>
    {
        public WorkflowDefinitionByIdSpecification(string id, string? tenantId = default)
        {
            Id = id;
            TenantId = tenantId;
        }

        public string Id { get; set; }
        public VersionOptions? VersionOptions { get; }
        public string? TenantId { get; set; }

        public override Expression<Func<WorkflowDefinition, bool>> ToExpression()
        {
            Expression<Func<WorkflowDefinition, bool>> predicate = x => x.Id == Id;

            if (!string.IsNullOrWhiteSpace(TenantId))
                predicate = predicate.And(x => x.TenantId == TenantId);

            return predicate;
        }
    }
}
