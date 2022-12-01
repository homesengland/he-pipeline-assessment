using AutoFixture.Xunit2;
using Elsa.Server.Features.Workflow;
using Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen;
using Elsa.Server.Features.Workflow.LoadConfirmationScreen;
using Elsa.Server.Features.Workflow.LoadQuestionScreen;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
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
                ValidationMessages = null,
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
            var okResultValueData = (OperationResult<StartWorkflowResponse>)okResult.Value!;

            Assert.Equal(response.NextActivityId, okResultValueData.Data!.NextActivityId);
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
            var badResultValueData = (string)badResult.Value!;

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

            var exceptionResult = (Exception)objectResult.Value!;

            Assert.Equal(exception.Message, exceptionResult.Message);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_StartWorkflow_ShouldReturn500_WhenCommandHandlerReturnsNull(
            StartWorkflowCommand command,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync((OperationResult<StartWorkflowResponse>)null!);

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

        [Theory]
        [AutoData]
        public async Task WorkflowController_LoadQuestionScreen_ShouldReturnOK_WhenCommandHandlerIsSuccessful(

           LoadQuestionScreenRequest request,
           LoadQuestionScreenResponse response,
           Mock<IMediator> mediatorMock)
        {
            var startWorkflowOperationResult = new OperationResult<LoadQuestionScreenResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadQuestionScreenRequest>(), CancellationToken.None)).ReturnsAsync(startWorkflowOperationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadQuestionScreen(request.WorkflowInstanceId, request.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var okResultValueData = (OperationResult<LoadQuestionScreenResponse>)okResult.Value!;

            Assert.Equal(response.ActivityId, okResultValueData.Data!.ActivityId);
            Assert.Equal(response.WorkflowInstanceId, okResultValueData.Data.WorkflowInstanceId);

        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_LoadQuestionScreen_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            LoadQuestionScreenRequest request,
            OperationResult<LoadQuestionScreenResponse> operationResult,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadQuestionScreenRequest>(), CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadQuestionScreen(request.WorkflowInstanceId, request.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', operationResult.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_LoadQuestionScreen_ShouldReturn500_WhenCommandHandlerThrowsException(
            LoadQuestionScreenRequest request,
            Exception exception,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadQuestionScreenRequest>(), CancellationToken.None)).ThrowsAsync(exception);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadQuestionScreen(request.WorkflowInstanceId, request.ActivityId);

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
        public async Task WorkflowController_LoadQuestionScreen_ShouldReturn500_WhenCommandHandlerReturnsNull(
            LoadQuestionScreenRequest request,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadQuestionScreenRequest>(), CancellationToken.None)).ReturnsAsync((OperationResult<LoadQuestionScreenResponse>)null!);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadQuestionScreen(request.WorkflowInstanceId, request.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<NullReferenceException>(objectResult.Value);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_LoadCheckYourAnswersScreen_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
           LoadCheckYourAnswersScreenRequest request,
           LoadCheckYourAnswersScreenResponse response,
           Mock<IMediator> mediatorMock)
        {
            var loadCheckYourAnswersResponse = new OperationResult<LoadCheckYourAnswersScreenResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadCheckYourAnswersScreenRequest>(), CancellationToken.None)).ReturnsAsync(loadCheckYourAnswersResponse);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadCheckYourAnswersScreen(request.WorkflowInstanceId, request.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var okResultValueData = (OperationResult<LoadCheckYourAnswersScreenResponse>)okResult.Value!;

            Assert.Equal(response.ActivityId, okResultValueData.Data!.ActivityId);
            Assert.Equal(response.WorkflowInstanceId, okResultValueData.Data.WorkflowInstanceId);

        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_LoadCheckYourAnswersScreen_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            LoadCheckYourAnswersScreenRequest request,
            OperationResult<LoadCheckYourAnswersScreenResponse> operationResult,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadCheckYourAnswersScreenRequest>(), CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadCheckYourAnswersScreen(request.WorkflowInstanceId, request.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', operationResult.ErrorMessages), badResultValueData);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_LoadCheckYourAnswersScreen_ShouldReturn500_WhenCommandHandlerThrowsException(
            LoadCheckYourAnswersScreenRequest request,
            Exception exception,
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadCheckYourAnswersScreenRequest>(), CancellationToken.None)).ThrowsAsync(exception);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadCheckYourAnswersScreen(request.WorkflowInstanceId, request.ActivityId);

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
        public async Task WorkflowController_LoadCheckYourAnswersScreen_ShouldReturn500_WhenCommandHandlerReturnsNull(
            LoadCheckYourAnswersScreenRequest request,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadCheckYourAnswersScreenRequest>(), CancellationToken.None)).ReturnsAsync((OperationResult<LoadCheckYourAnswersScreenResponse>)null!);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadCheckYourAnswersScreen(request.WorkflowInstanceId, request.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<NullReferenceException>(objectResult.Value);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_LoadConfirmationScreen_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
           LoadConfirmationScreenRequest request,
           LoadConfirmationScreenResponse response,
           Mock<IMediator> mediatorMock)
        {
            var operationResult = new OperationResult<LoadConfirmationScreenResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadConfirmationScreenRequest>(), CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadConfirmationScreen(request.WorkflowInstanceId, request.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var okResultValueData = (OperationResult<LoadConfirmationScreenResponse>)okResult.Value!;

            Assert.Equal(response.ActivityId, okResultValueData.Data!.ActivityId);
            Assert.Equal(response.WorkflowInstanceId, okResultValueData.Data.WorkflowInstanceId);

        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_LoadConfirmationScreen_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            LoadConfirmationScreenRequest request,
            OperationResult<LoadConfirmationScreenResponse> operationResult,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadConfirmationScreenRequest>(), CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadConfirmationScreen(request.WorkflowInstanceId, request.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', operationResult.ErrorMessages), badResultValueData);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_LoadConfirmationScreen_ShouldReturn500_WhenCommandHandlerThrowsException(
            LoadConfirmationScreenRequest request,
            Exception exception,
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadConfirmationScreenRequest>(), CancellationToken.None)).ThrowsAsync(exception);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadConfirmationScreen(request.WorkflowInstanceId, request.ActivityId);

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
        public async Task WorkflowController_LoadConfirmationScreen_ShouldReturn500_WhenCommandHandlerReturnsNull(
            LoadConfirmationScreenRequest request,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadConfirmationScreenRequest>(), CancellationToken.None)).ReturnsAsync((OperationResult<LoadCheckYourAnswersScreenResponse>)null!);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.LoadConfirmationScreen(request.WorkflowInstanceId, request.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<NullReferenceException>(objectResult.Value);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_MutliSaveAndContinue_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            QuestionScreenSaveAndContinueCommand command,
            QuestionScreenSaveAndContinueResponse response,
            Mock<IMediator> mediatorMock)
        {

            var startWorkflowOperationResult = new OperationResult<QuestionScreenSaveAndContinueResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<QuestionScreenSaveAndContinueCommand>(), CancellationToken.None)).ReturnsAsync(startWorkflowOperationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.QuestionScreenSaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var okResultValueData = (OperationResult<QuestionScreenSaveAndContinueResponse>)okResult.Value!;

            Assert.Equal(response.NextActivityId, okResultValueData.Data!.NextActivityId);
            Assert.Equal(response.WorkflowInstanceId, okResultValueData.Data.WorkflowInstanceId);

        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_MultiSaveAndContinue_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            QuestionScreenSaveAndContinueCommand command,
            OperationResult<QuestionScreenSaveAndContinueResponse> operationResult,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<QuestionScreenSaveAndContinueCommand>(), CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.QuestionScreenSaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', operationResult.ErrorMessages), badResultValueData);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_MultiSaveAndContinue_ShouldReturn500_WhenCommandHandlerThrowsException(
            QuestionScreenSaveAndContinueCommand command,
            Exception exception,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<QuestionScreenSaveAndContinueCommand>(), CancellationToken.None)).ThrowsAsync(exception);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.QuestionScreenSaveAndContinue(command);

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
        public async Task WorkflowController_MultiSaveAndContinue_ShouldReturn500_WhenCommandHandlerReturnsNull(
            QuestionScreenSaveAndContinueCommand command,
            Mock<IMediator> mediatorMock)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<QuestionScreenSaveAndContinueCommand>(), CancellationToken.None)).ReturnsAsync((OperationResult<QuestionScreenSaveAndContinueResponse>)null!);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.QuestionScreenSaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<NullReferenceException>(objectResult.Value);
        }

    }
}
