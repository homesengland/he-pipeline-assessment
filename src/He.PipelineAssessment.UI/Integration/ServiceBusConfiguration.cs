namespace He.PipelineAssessment.UI.Integration
{
    public class ServiceBusConfiguration
    {
        public string ConnectionString { get; set; }
        public string QueueToReceiveMessages { get; set; }
        public string QueueToSendMessages { get; set; }
    }
}
