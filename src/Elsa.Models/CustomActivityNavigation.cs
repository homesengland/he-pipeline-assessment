namespace Elsa.CustomModels
{
    public class CustomActivityNavigation
    {
        public int Id { get; set; }
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
        public string PreviousActivityType { get; set; } = null!;
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
    }
}
