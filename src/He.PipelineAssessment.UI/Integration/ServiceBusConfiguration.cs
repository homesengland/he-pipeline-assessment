namespace He.PipelineAssessment.UI.Integration
{
    public class ServiceBusConfiguration
    {
        public string? ConnectionString { get; set; }
        public string? QueueToReceiveMessages { get; set; }
        public string? QueueToSendMessages { get; set; }
        public bool UseDefaultAzureCredential { get; set; }
        public bool ReceiveMessages { get; set; }
        public bool SendMessages { get; set; }
    }
}