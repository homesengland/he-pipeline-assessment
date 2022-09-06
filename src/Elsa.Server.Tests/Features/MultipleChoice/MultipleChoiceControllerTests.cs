using AutoFixture.Xunit2;
using Elsa.Server.Features.Workflow;
using Elsa.Server.Features.Workflow.SaveAndContinue;
using Elsa.Server.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.MultipleChoice
{
    public class MultipleChoiceQuestionControllerTests
    {
        [Theory]
        [AutoData]
        public async Task MultipleChoiceQuestionController_SaveAndContinue_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
           SaveAndContinueCommand command,
           SaveAndContinueResponse response,
           Mock<IMediator> mediatorMock)
        {
            var startWorkflowOperationResult = new OperationResult<SaveAndContinueResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = new List<string>(),
                Data = response
            };
            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(startWorkflowOperationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.SaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var okResultValueData = (OperationResult<SaveAndContinueResponse>)okResult.Value!;

            Assert.Equal(response.NextActivityId, okResultValueData.Data!.NextActivityId);
            Assert.Equal(response.WorkflowInstanceId, okResultValueData.Data.WorkflowInstanceId);

        }

        [Theory]
        [AutoData]
        public async Task MultipleChoiceQuestionController_SaveAndContinue_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            SaveAndContinueCommand command,
            OperationResult<SaveAndContinueResponse> operationResult,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.SaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', operationResult.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task MultipleChoiceQuestionController_SaveAndContinue_ShouldReturn500_WhenCommandHandlerThrowsException(
            SaveAndContinueCommand command,
            Exception exception,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ThrowsAsync(exception);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.SaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<Exception>(objectResult.Value);

            var exceptionResult = (Exception)objectResult.Value!;

            Assert.Equal(exception.Message, exceptionResult.Message);
        }

        [Theory]
        [AutoData]
        public async Task MultipleChoiceQuestionController_SaveAndContinue_ShouldReturn500_WhenCommandHandlerReturnsNull(
            SaveAndContinueCommand command,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync((OperationResult<SaveAndContinueResponse>)null!);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.SaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<NullReferenceException>(objectResult.Value);
        }
    }
}
