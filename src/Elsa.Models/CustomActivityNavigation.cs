namespace Elsa.CustomModels
{
    public class CustomActivityNavigation : AuditableEntity
    {
        public int Id { get; set; }
        public string ActivityId { get; set; } = null!;
        public string CorrelationId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
        public string PreviousActivityType { get; set; } = null!;

    }
}
