using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NodaTime;
using Rebus.Bus;
using Rebus.Exceptions;

namespace Elsa.Services.Messaging
{
    public class CommandSender : ICommandSender
    {
        private readonly IServiceBusFactory _serviceBusFactory;
        private readonly SemaphoreSlim _semaphore = new(1);
        private readonly ILogger<CommandSender> _logger;

        public CommandSender(IServiceBusFactory serviceBusFactory, ILogger<CommandSender> logger)
        {
            _serviceBusFactory = serviceBusFactory;
            _logger = logger;
        }

        public async Task SendAsync(object message, string? queueName = default, IDictionary<string, string>? headers = default, CancellationToken cancellationToken = default)
        {
            var bus = await GetBusAsync(message, queueName, cancellationToken);
            await _semaphore.WaitAsync(cancellationToken);
            
            // Attempt to prevent: Could not 'GetOrAdd' item with key 'new-azure-service-bus-transport' error.
            try
            {
                await bus.Send(message, headers);
            }
            catch (RebusApplicationException e)
            {
                _logger.LogWarning(e, "An error occurred while sending a message. Disposing and recreating the service bus instance and retrying.");
                await _serviceBusFactory.DisposeServiceBusAsync(bus, cancellationToken);
                bus = await GetBusAsync(message, queueName, cancellationToken);
                await bus.Send(message, headers);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DeferAsync(object message, Duration delay, string? queueName = default, IDictionary<string, string>? headers = default, CancellationToken cancellationToken = default)
        {
            var bus = await GetBusAsync(message, queueName, cancellationToken);
            await bus.Defer(delay.ToTimeSpan(), message, headers);
        }

        private Task<IBus> GetBusAsync(object message, string? queueName, CancellationToken cancellationToken) => _serviceBusFactory.GetServiceBusAsync(message.GetType(), queueName, cancellationToken);
    }
}