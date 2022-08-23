using AutoFixture.Xunit2;
using Elsa.Server.Features.Workflow;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow
{
    public class WorkflowControllerTests
    {



        [Theory]
        [AutoData]
        public async Task WorkflowController_StartWorkflow_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            StartWorkflowCommand command,
            StartWorkflowResponse response,
            Mock<IMediator> mediatorMock)
        {
            var startWorkflowOperationResult = new OperationResult<StartWorkflowResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = new List<string>(),
                Data = response
            };
            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(startWorkflowOperationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.StartWorkflow(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var okResultValueData = (OperationResult<StartWorkflowResponse>)okResult.Value;

            Assert.Equal(response.NextActivityId, okResultValueData.Data.NextActivityId);
            Assert.Equal(response.WorkflowInstanceId, okResultValueData.Data.WorkflowInstanceId);

        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_StartWorkflow_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            StartWorkflowCommand command,
            OperationResult<StartWorkflowResponse> operationResult,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.StartWorkflow(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value;

            Assert.Equal(string.Join(',', operationResult.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_StartWorkflow_ShouldReturn500_WhenCommandHandlerThrowsException(
            StartWorkflowCommand command,
            Exception exception,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ThrowsAsync(exception);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.StartWorkflow(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<Exception>(objectResult.Value);

            var exceptionResult = (Exception)objectResult.Value;

            Assert.Equal(exception.Message, exceptionResult.Message);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_StartWorkflow_ShouldReturn500_WhenCommandHandlerReturnsNull(
            StartWorkflowCommand command,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync((OperationResult<StartWorkflowResponse>)null);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.StartWorkflow(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<NullReferenceException>(objectResult.Value);
        }
    }
}
