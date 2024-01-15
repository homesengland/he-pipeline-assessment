using AutoFixture;
using AutoFixture.Xunit2;
using Elsa.Server.Features.Workflow;
using Elsa.Server.Features.Workflow.ArchiveQuestions;
using Elsa.Server.Features.Workflow.CheckYourAnswersSaveAndContinue;
using Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen;
using Elsa.Server.Features.Workflow.LoadConfirmationScreen;
using Elsa.Server.Features.Workflow.LoadQuestionScreen;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Features.Workflow.ReturnToActivity;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
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
           Mock<IMediator> mediatorMock)
        {

            var response = SampleResponse<LoadQuestionScreenResponse>();
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
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            OperationResult<LoadQuestionScreenResponse> operationResult = new OperationResult<LoadQuestionScreenResponse>();
            operationResult.ErrorMessages = new List<string> { "StandardErrorMessage" };


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
           Mock<IMediator> mediatorMock)
        {
            var response = SampleResponse<LoadCheckYourAnswersScreenResponse>();
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
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            OperationResult<LoadCheckYourAnswersScreenResponse> operationResult = new OperationResult<LoadCheckYourAnswersScreenResponse>();
            operationResult.ErrorMessages = new List<string> { "StandardErrorMessage" };
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
           Mock<IMediator> mediatorMock)
        {
            var response = SampleResponse<LoadConfirmationScreenResponse>();
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
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            OperationResult<LoadConfirmationScreenResponse> operationResult = new OperationResult<LoadConfirmationScreenResponse>();
            operationResult.ErrorMessages = new List<string> { "StandardErrorMessage" };
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
            mediatorMock.Setup(x => x.Send(It.IsAny<LoadConfirmationScreenRequest>(), CancellationToken.None)).ReturnsAsync((OperationResult<LoadConfirmationScreenResponse>)null!);

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

        [Theory]
        [AutoData]
        public async Task WorkflowController_CheckYourAnswersSaveAndContinue_ShouldReturnOK_WhenCommandHandlerIsSuccessful(

           CheckYourAnswersSaveAndContinueCommand request,
           Mock<IMediator> mediatorMock)
        {
            var response = SampleResponse<CheckYourAnswersSaveAndContinueResponse>();
            var startWorkflowOperationResult = new OperationResult<CheckYourAnswersSaveAndContinueResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<CheckYourAnswersSaveAndContinueCommand>(), CancellationToken.None)).ReturnsAsync(startWorkflowOperationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.CheckYourAnswersSaveAndContinue(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var okResultValueData = (OperationResult<CheckYourAnswersSaveAndContinueResponse>)okResult.Value!;

            Assert.Equal(response.ActivityType, okResultValueData.Data!.ActivityType);
            Assert.Equal(response.WorkflowInstanceId, okResultValueData.Data.WorkflowInstanceId);

        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_CheckYourAnswersSaveAndContinue_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            CheckYourAnswersSaveAndContinueCommand request,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            OperationResult<CheckYourAnswersSaveAndContinueResponse> operationResult = new OperationResult<CheckYourAnswersSaveAndContinueResponse>();
            operationResult.ErrorMessages = new List<string> { "StandardErrorMessage" };


            mediatorMock.Setup(x => x.Send(It.IsAny<CheckYourAnswersSaveAndContinueCommand>(), CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.CheckYourAnswersSaveAndContinue(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', operationResult.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_CheckYourAnswersSaveAndContinue_ShouldReturn500_WhenCommandHandlerThrowsException(
            CheckYourAnswersSaveAndContinueCommand request,
            Exception exception,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<CheckYourAnswersSaveAndContinueCommand>(), CancellationToken.None)).ThrowsAsync(exception);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.CheckYourAnswersSaveAndContinue(request);

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
        public async Task WorkflowController_CheckYourAnswersSaveAndContinue_ShouldReturn500_WhenCommandHandlerReturnsNull(
            CheckYourAnswersSaveAndContinueCommand request,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<CheckYourAnswersSaveAndContinueCommand>(), CancellationToken.None)).ReturnsAsync((OperationResult<CheckYourAnswersSaveAndContinueResponse>)null!);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.CheckYourAnswersSaveAndContinue(request);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<NullReferenceException>(objectResult.Value);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_ArchiveQuestions_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
            ArchiveQuestionsCommand command,
            Mock<IMediator> mediatorMock)
        {
            var operationResult = new OperationResult<ArchiveQuestionsCommandResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null
            };
            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.ArchiveQuestions(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_ArchiveQuestions_ShouldReturnBadRequest_WhenCommandHandlerReturnsErrors(
            ArchiveQuestionsCommand command,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            var operationResult = new OperationResult<ArchiveQuestionsCommandResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null
            };
            operationResult.ErrorMessages = new List<string> { "StandardErrorMessage" };


            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.ArchiveQuestions(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', operationResult.ErrorMessages), badResultValueData);

        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_ArchiveQuestions_ShouldReturn500_WhenCommandHandlerThrowsException(
            ArchiveQuestionsCommand command,
            Exception exception,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ThrowsAsync(exception);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.ArchiveQuestions(command);

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
        public async Task WorkflowController_ArchiveQuestions_ShouldReturn500_WhenCommandHandlerReturnsNull(
            ArchiveQuestionsCommand command,
            Mock<IMediator> mediatorMock)
        {

            //Arrange
            mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync((OperationResult<ArchiveQuestionsCommandResponse>)null!);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.ArchiveQuestions(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<NullReferenceException>(objectResult.Value);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_ReturnToActivity_ShouldReturnOK_WhenCommandHandlerIsSuccessful(
        ReturnToActivityCommand command,
        ReturnToActivityResponse response,
        Mock<IMediator> mediatorMock)
        {
            var returnToActivityOperationResult = new OperationResult<ReturnToActivityResponse>
            {
                ErrorMessages = new List<string>(),
                ValidationMessages = null,
                Data = response
            };
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<ReturnToActivityCommand>(), CancellationToken.None)).ReturnsAsync(returnToActivityOperationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.ReturnToActivity(command.WorkflowInstanceId, command.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var okResultValueData = (OperationResult<ReturnToActivityResponse>)okResult.Value!;

            Assert.Equal(response.ActivityId, okResultValueData.Data!.ActivityId);
            Assert.Equal(response.ActivityType, okResultValueData.Data.ActivityType);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_ReturnToActivity_ShouldReturnBadRequest_WhenCommandHandlerIsNotSuccessful(
        ReturnToActivityCommand command,
        OperationResult<ReturnToActivityResponse> operationResult,
        Mock<IMediator> mediatorMock)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<ReturnToActivityCommand>(), CancellationToken.None)).ReturnsAsync(operationResult);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.ReturnToActivity(command!.WorkflowInstanceId, command!.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            var badResult = (BadRequestObjectResult)result;
            var badResultValueData = (string)badResult.Value!;

            Assert.Equal(string.Join(',', operationResult.ErrorMessages), badResultValueData);
        }

        [Theory]
        [AutoData]
        public async Task WorkflowController_ReturnToActivity_ShouldReturn500_WhenCommandHandlerThrowsException(
        ReturnToActivityCommand command,
        Exception exception,
        Mock<IMediator> mediatorMock)
        {
            //Arrange
            mediatorMock.Setup(x => x.Send(It.IsAny<ReturnToActivityCommand>(), CancellationToken.None)).ThrowsAsync(exception);

            WorkflowController controller = new WorkflowController(mediatorMock.Object);

            //Act
            var result = await controller.ReturnToActivity(command!.WorkflowInstanceId, command!.ActivityId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);

            var objectResult = (ObjectResult)result;

            Assert.Equal(500, objectResult.StatusCode);
            Assert.IsType<Exception>(objectResult.Value);

            var exceptionResult = (Exception)objectResult.Value!;

            Assert.Equal(exception.Message, exceptionResult.Message);
        }


        private T SampleResponse<T>()
        {
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => fixture.Behaviors.Remove(x));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var response = fixture.Create<T>();
            return response;
        }

    }
}
