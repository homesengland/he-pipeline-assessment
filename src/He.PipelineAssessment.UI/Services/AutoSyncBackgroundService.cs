
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using MediatR;

namespace He.PipelineAssessment.UI.Services
{
    public class AutoSyncBackgroundService : BackgroundService
    {
        private readonly ILogger<AutoSyncBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public AutoSyncBackgroundService(ILogger<AutoSyncBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var scopedServiceProvider = scope.ServiceProvider;
                        var scopedService = scopedServiceProvider.GetService<IMediator>();
                        if (scopedService != null)
                        {
                            await scopedService.Send(new SyncCommand());
                        }
                        else
                        {
                            _logger.LogInformation($"Failed to Load the Mediator Service in AutoSync Background Service");
                        }
                    }
                    await Task.Delay(TimeSpan.FromMinutes(5));
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Auto Sync Back Ground Service returned : {ex.Message}");
            }
        }
    }
}
