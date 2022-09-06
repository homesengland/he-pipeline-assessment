using AutoFixture.Xunit2;
using Elsa.Server.Features.MultipleChoice;
using Elsa.Server.Features.MultipleChoice.SaveAndContinue;
using Elsa.Server.Features.Shared.SaveAndContinue;
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
           MultipleChoiceSaveAndContinueCommand command,
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

            SaveAndContinueController controller = new MultipleChoiceQuestionController(mediatorMock.Object);

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
            MultipleChoiceSaveAndContinueCommand command,
            OperationResult<SaveAndContinueResponse> operationResult,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(operationResult);

            SaveAndContinueController controller = new MultipleChoiceQuestionController(mediatorMock.Object);

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
            MultipleChoiceSaveAndContinueCommand command,
            Exception exception,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ThrowsAsync(exception);

            SaveAndContinueController controller = new MultipleChoiceQuestionController(mediatorMock.Object);

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
            MultipleChoiceSaveAndContinueCommand command,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync((OperationResult<SaveAndContinueResponse>)null!);

            SaveAndContinueController controller = new MultipleChoiceQuestionController(mediatorMock.Object);

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
