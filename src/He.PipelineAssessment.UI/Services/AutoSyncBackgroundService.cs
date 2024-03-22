
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using MediatR;
using System.Security.Claims;

namespace He.PipelineAssessment.UI.Services
{
    public class AutoSyncBackgroundService : BackgroundService
    {
        private readonly ILogger<AutoSyncBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _contextAccessor;

        public AutoSyncBackgroundService(ILogger<AutoSyncBackgroundService> logger, IServiceProvider serviceProvider, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _contextAccessor = contextAccessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var userName = "bg_task";
                var httpContext = new DefaultHttpContext();
                httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, userName) }));
                _contextAccessor.HttpContext = httpContext;
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
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Auto Sync Back Ground Service returned : {ex.Message}");
            }
        }
    }
}
