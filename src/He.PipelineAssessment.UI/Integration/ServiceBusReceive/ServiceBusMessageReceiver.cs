using Azure.Identity;
using Azure.Messaging.ServiceBus;
using He.PipelineAssessment.UI.Integration.Project;
using MediatR;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Integration.ServiceBus
{
    public class ServiceBusMessageReceiver : IServiceBusMessageReceiver, IDisposable
    {
        private ServiceBusConfiguration _configuration;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        private IMediator _mediator;

        public ServiceBusMessageReceiver(IOptions<ServiceBusConfiguration> configuration, IMediator mediator)
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

            this._processor = _client.CreateProcessor(_configuration.QueueToReceiveMessages, new ServiceBusProcessorOptions());
            this._mediator = mediator;
        }
        public Task RegisterMessageHandler()
        {
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
            _processor.StartProcessingAsync();
            return Task.CompletedTask;
        }

        public Task UnRegisterMessageHandler()
        {
            _processor.StopProcessingAsync();
            _processor.ProcessMessageAsync -= MessageHandler;
            _processor.ProcessErrorAsync -= ErrorHandler;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (disposing)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
                await _client.DisposeAsync().ConfigureAwait(false);
            }
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string messageBody = args.Message.Body.ToString();
            ProjectDTO record = JsonConvert.DeserializeObject<ProjectDTO>(messageBody)!;
            var command = new UpsertProjectCommand(record);
            var id = await _mediator.Send(command);
            await args.CompleteMessageAsync(args.Message);
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
