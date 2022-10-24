namespace Elsa.CustomModels
{
    public class AssessmentQuestion
    {
        public int Id { get; set; }
        public string? CorrelationId { get; set; }
        public int? Version { get; set; }

        public string ActivityId { get; set; }
        public string? ActivityName { get; set; }
        public string ActivityType { get; set; } = null!;

        public string? WorkflowName { get; set; }
        public string WorkflowDefinitionId { get; set; }
        public string WorkflowInstanceId { get; set; }

        public string? Question { get; set; }
        public string? Answer { get; set; }
        public string? Comments { get; set; }
        public string PreviousActivityId { get; set; } = null!;

        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }

        public void SetAnswer(string? answer, DateTime lastModifiedDateTime)
        {
            this.Answer = answer;
            this.LastModifiedDateTime = lastModifiedDateTime;
        }
    }
}
