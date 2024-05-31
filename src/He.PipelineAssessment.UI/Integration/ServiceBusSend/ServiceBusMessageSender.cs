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
        private readonly List<ServiceBusSender> _senders;
        private readonly IMediator _mediator;
        private readonly ILogger<ServiceBusMessageSender> _logger;

        public ServiceBusMessageSender(IOptions<ServiceBusConfiguration> configuration, IMediator mediator, ILogger<ServiceBusMessageSender> logger)
        {
            _configuration = configuration.Value;
            _senders = new List<ServiceBusSender>();

            if (_configuration.UseDefaultAzureCredential)
            {
                this._client = new ServiceBusClient(_configuration.ConnectionString, new DefaultAzureCredential());
            }
            else
            {
                this._client = new ServiceBusClient(_configuration.ConnectionString);
            }

            if (_configuration.QueueToSendMessages != null)
            {
                var queuesToSendMessage = _configuration.QueueToSendMessages.Split(",");

                if (queuesToSendMessage != null && queuesToSendMessage.Length > 0)
                {
                    foreach (var queue in queuesToSendMessage)
                    {
                        this._senders.Add(this._client.CreateSender(queue));
                    }
                }
            }

            this._mediator = mediator;
            this._logger = logger;
        }

        public async void SendMessage(string messageBody)
        {
            if (_configuration.SendMessages)
            {
                ServiceBusMessage message = new ServiceBusMessage(messageBody);
               if(_senders !=  null && _senders.Count() > 0)
                {
                    foreach (var sender in _senders)
                    {
                        await sender.SendMessageAsync(message);
                    }
                }
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
                ProjectId = data.Assessment.SpId,
                WorkflowInstanceId = data.WorkflowInstanceId,
                AssessmentToolWorkflowInstanceId = data.Id
            };

            var assesmentStatusResult = _mediator.Send(command).Result;
            string jsonMessage = JsonConvert.SerializeObject(assesmentStatusResult.Assessment, Formatting.Indented);
            this.SendMessage(jsonMessage);
        }
    }
}
