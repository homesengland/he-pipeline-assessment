using Azure.Identity;
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
        private readonly ILogger<ServiceBusMessageSender> _logger;

        public ServiceBusMessageSender(IOptions<ServiceBusConfiguration> configuration, IMediator mediator, ILogger<ServiceBusMessageSender> logger)
        {
            _configuration = configuration.Value;

            if (_configuration.UseDefaultAzureCredential)
            {
                this._client = new ServiceBusClient(_configuration.ConnectionString, new DefaultAzureCredential());
            }
            else
            {
                this._client = new ServiceBusClient(_configuration.ConnectionString);
            }

            this._sender = this._client.CreateSender(_configuration.QueueToSendMessages);
            this._mediator = mediator;
            this._logger = logger;
        }

        public async void SendMessage(string messageBody)
        {
            if (_configuration.SendMessages)
            {
                ServiceBusMessage message = new ServiceBusMessage(messageBody);
                await this._sender.SendMessageAsync(message);
            }
            else
            {
                _logger.LogInformation("A message was not sent to Service Bus due to configuration");
            }
        }

        public void SendMessage(AssessmentToolWorkflowInstance data)
        {
            GetAssessmentDTOCommand command = new GetAssessmentDTOCommand()
            {
                ProjectId = data.Assessment == null ?  0 : data.Assessment.Id,
                WorkflowInstanceId = data.WorkflowInstanceId,
                AssessmentToolWorkflowInstanceId = data.Id
            };

            var assesmentStatusResult = _mediator.Send(command).Result;
            string jsonMessage = JsonConvert.SerializeObject(assesmentStatusResult.Assessment, Formatting.Indented);
            this.SendMessage(jsonMessage);
        }
    }
}
