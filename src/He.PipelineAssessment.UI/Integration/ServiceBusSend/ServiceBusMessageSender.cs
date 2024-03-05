using Azure.Core;
using Azure.Messaging.ServiceBus;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Integration.ServiceBusSend
{
    public class ServiceBusMessageSender : IServiceBusMessageSender
    {
        private readonly ServiceBusConfiguration _configuration;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private readonly IMediator _mediator;
        
        public ServiceBusMessageSender(IOptions<ServiceBusConfiguration> configuration, IMediator mediator)
        {
            _configuration = configuration.Value;
            this._client = new ServiceBusClient(_configuration.ConnectionString);
            this._sender = this._client.CreateSender(_configuration.QueueToSendMessages);
            this._mediator = mediator;
        }

        public async void SendMessage(string messageBody)
        {
            ServiceBusMessage message = new ServiceBusMessage(messageBody);
            await this._sender.SendMessageAsync(message);
        }

        public void SendMessage(AssessmentToolWorkflowInstance data)
        {
            GetAssessmentDTOCommand command = new GetAssessmentDTOCommand()
            {
                ProjectId = data.AssessmentId,
                WorkflowInstanceId = data.WorkflowInstanceId,
                AssessmentToolWorkflowInstanceId = data.Id
            };

            var assesmentStatusResult = _mediator.Send(command).Result;
            string jsonMessage = JsonConvert.SerializeObject(assesmentStatusResult.Assessment, Formatting.Indented);
            this.SendMessage(jsonMessage);
        }
    }
}
