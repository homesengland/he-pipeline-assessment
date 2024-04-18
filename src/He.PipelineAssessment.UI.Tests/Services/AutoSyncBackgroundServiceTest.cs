using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using He.PipelineAssessment.UI.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Services
{
    public class AutoSyncBackgroundServiceTest
    {
        [Fact]
        public async Task HandleEvent_Task()
        {
            var mediatorMock = new Mock<IMediator>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var loggerMock = new Mock<ILogger<AutoSyncBackgroundService>>();

            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            var scopeMock = new Mock<IServiceScope>();

            scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);

            scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);

            serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactoryMock.Object);

            serviceProviderMock.Setup(x => x.GetService(typeof(IMediator))).Returns(mediatorMock.Object);

            var backgroundService = new AutoSyncBackgroundService(loggerMock.Object,serviceProviderMock.Object, httpContextAccessorMock.Object);

            await backgroundService.PublicMethodAsync();

            mediatorMock.Verify(m => m.Send(It.IsAny<SyncCommand>(),CancellationToken.None), Times.Once);
        }
    }
}
