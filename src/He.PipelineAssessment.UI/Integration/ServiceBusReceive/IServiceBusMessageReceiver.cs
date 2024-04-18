namespace He.PipelineAssessment.UI.Integration.ServiceBus
{
    public interface IServiceBusMessageReceiver
    {
        Task RegisterMessageHandler();
        Task UnRegisterMessageHandler();
    }
}
