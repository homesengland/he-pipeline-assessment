namespace Elsa.CustomModels
{
    public class QuestionWorkflowInstance
    {
        public int Id { get; set; }
        public string WorkflowInstanceId { get; set; } = null!;
        public string WorkflowDefinitionId { get; set; } = null!;
        public string WorkflowName { get; set; } = null!;
        public string CorrelationId { get; set; } = null!;
        public string? Result { get; set; }
        public string? Score { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
