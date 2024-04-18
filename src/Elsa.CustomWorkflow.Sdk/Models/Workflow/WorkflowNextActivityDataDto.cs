using FluentValidation.Results;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class WorkflowNextActivityDataDto
    {
        public WorkflowNextActivityData Data { get; set; } = null!;
        public bool IsValid { get; set; }
        public ValidationResult? ValidationMessages { get; set; }
    }

    public class WorkflowNextActivityData
    {
        public string WorkflowName { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string? NextWorkflowDefinitionIds { get; set; }
        public string FirstActivityId { get; set; } = null!;
        public string FirstActivityType { get; set; } = null!;
    }
}
