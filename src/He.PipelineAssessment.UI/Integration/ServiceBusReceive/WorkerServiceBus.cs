using Auth0.ManagementApi.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Integration.ServiceBus
{
    public class WorkerServiceBus : IHostedService
    {
        private IServiceScopeFactory _serviceScopeFactory { get; set; }
        private IServiceScope _serviceScope { get; set; }
        private readonly ILogger<WorkerServiceBus> _logger;
        private readonly IServiceBusMessageReceiver _serviceBusConsumer;

        public WorkerServiceBus(
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _serviceScope = serviceScopeFactory.CreateScope();
            //ServiceBusClient
            _serviceBusConsumer = _serviceScope.ServiceProvider.GetRequiredService<IServiceBusMessageReceiver>();
            _logger = _serviceScope.ServiceProvider.GetRequiredService<ILogger<WorkerServiceBus>>(); ;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Starting the service bus queue consumer and the subscription");
            await _serviceBusConsumer.RegisterMessageHandler().ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Stopping the service bus queue consumer and the subscription");
            await _serviceBusConsumer.UnRegisterMessageHandler().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceScope.Dispose();
            }
        }
    }
}
