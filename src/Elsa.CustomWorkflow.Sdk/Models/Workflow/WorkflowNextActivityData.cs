namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class WorkflowNextActivityDataDto
    {
        public WorkflowNextActivityData Data { get; set; } = null!;
        public bool IsValid { get; set; }
        public IList<string> ValidationMessages { get; set; } = new List<string>();
    }

    public class WorkflowNextActivityData
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
    }
}
