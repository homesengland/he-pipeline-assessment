using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Integration.ServiceBusSend
{
    public interface IServiceBusMessageSender
    {
        void SendMessage(string message);

        void SendMessage(AssessmentToolWorkflowInstance data);
    }
}
