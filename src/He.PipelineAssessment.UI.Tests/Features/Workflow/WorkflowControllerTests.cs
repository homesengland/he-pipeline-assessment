using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.UI.Features.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.StartWorkflow;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow
{
    public class WorkflowControllerTests
    {
        [Theory]
        [AutoMoqData]
        public void Index_ShouldReturn(
            WorkflowController sut)
        {
            //Arrange

            //Act
            var result = sut.Index();

            //Assert
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task StartWorkflow_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
            [Frozen] Mock<IMediator> mediator,
            StartWorkflowCommand command,
            Exception exception,
            WorkflowController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.StartWorkflow(command);

            //Assert
            mediator.Verify(x => x.Send(command, CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(command));

            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task StartWorkflow_ShouldRedirectToAction_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            StartWorkflowCommand command,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            WorkflowController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(loadWorkflowActivityRequest);

            //Act
            var result = await sut.StartWorkflow(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("LoadWorkflowActivity", redirectToActionResult.ActionName);


            var activityIdRouteValue = new KeyValuePair<string, object?>("ActivityId", loadWorkflowActivityRequest.ActivityId);
            var workflowInstanceIdRouteValue = new KeyValuePair<string, object?>("WorkflowInstanceId", loadWorkflowActivityRequest.WorkflowInstanceId);

            Assert.Contains(activityIdRouteValue, redirectToActionResult.RouteValues!);
            Assert.Contains(workflowInstanceIdRouteValue, redirectToActionResult.RouteValues!);

        }

        [Theory]
        [AutoMoqData]
        public async Task SaveAndContinue_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
            [Frozen] Mock<IMediator> mediator,
            SaveAndContinueCommand command,
            Exception exception,
            WorkflowController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.SaveAndContinue(command);

            //Assert
            mediator.Verify(x => x.Send(command, CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(command));

            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);

        }

        [Theory]
        [AutoMoqData]
        public async Task SaveAndContinue_ShouldRedirectToAction_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            SaveAndContinueCommand command,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            WorkflowController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(loadWorkflowActivityRequest);

            //Act
            var result = await sut.SaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("LoadWorkflowActivity", redirectToActionResult.ActionName);


            var activityIdRouteValue = new KeyValuePair<string, object?>("ActivityId", loadWorkflowActivityRequest.ActivityId);
            var workflowInstanceIdRouteValue = new KeyValuePair<string, object?>("WorkflowInstanceId", loadWorkflowActivityRequest.WorkflowInstanceId);

            Assert.Contains(activityIdRouteValue, redirectToActionResult.RouteValues!);
            Assert.Contains(workflowInstanceIdRouteValue, redirectToActionResult.RouteValues!);

        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
            [Frozen] Mock<IMediator> mediator,
            LoadWorkflowActivityRequest request,
            Exception exception,
            WorkflowController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(request, CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.LoadWorkflowActivity(request);

            //Assert
            mediator.Verify(x => x.Send(request, CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(request));

            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);

        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldRedirectToAction_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            LoadWorkflowActivityRequest request,
            SaveAndContinueCommand saveAndContinueCommand,
            WorkflowController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(request, CancellationToken.None)).ReturnsAsync(saveAndContinueCommand);

            //Act
            var result = await sut.LoadWorkflowActivity(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.Equal("SaveAndContinue", viewResult.ViewName);
            Assert.IsType<SaveAndContinueCommand>(viewResult.Model);

        }
    }
}
