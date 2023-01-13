using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.ConfirmationScreen
{
    public class ConfirmationScreenTests
    {
        //[Theory]
        //[AutoMoqData]
        //public async Task OnExecute_ReturnsSuspendResult(
        //    [Frozen] Mock<IServiceProvider> serviceProvider,
        //    [Frozen] Mock<IServiceScope> serviceScope,
        //    [Frozen] Mock<IServiceScopeFactory> serviceScopeFactory,
        //    [Frozen] Mock<IMediator> mediator,
        //    JsonSerializer jsonSerializer,
        //    WorkflowBlueprint workflowBlueprint,
        //    WorkflowInstance workflowInstance,
        //    CustomActivities.Activities.ConfirmationScreen.ConfirmationScreen sut)
        //{
        //    //Arrange
        //    serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

        //    serviceScopeFactory
        //        .Setup(x => x.CreateScope())
        //        .Returns(serviceScope.Object);

        //    serviceProvider
        //        .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
        //        .Returns(serviceScopeFactory.Object);

        //    serviceProvider.Setup(x => x.GetService(typeof(JsonSerializer))).Returns(jsonSerializer);
        //    serviceProvider.Setup(x => x.GetService(typeof(IMediator))).Returns(mediator.Object);

        //    WorkflowExecutionContext workflowExecutionContext = new WorkflowExecutionContext(serviceProvider.Object, workflowBlueprint, workflowInstance, null);

        //    var context = new ActivityExecutionContext(default!, workflowExecutionContext, default!, null, default, default);

        //    //Act
        //    var result = await sut.ExecuteAsync(context);

        //    //Assert
        //    Assert.NotNull(result);
        //    var outcomeResult = (SuspendResult)result;
        //    Assert.IsType<SuspendResult>(outcomeResult);
        //}

        [Theory]
        [AutoMoqData]
        public async Task OnResume_ReturnsDoneResult(
            CustomActivities.Activities.ConfirmationScreen.ConfirmationScreen sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, null, default, default);

            //Act
            var result = await sut.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);
            var outcomeResult = (OutcomeResult)result;
            Assert.IsType<OutcomeResult>(outcomeResult);
        }
    }
}
