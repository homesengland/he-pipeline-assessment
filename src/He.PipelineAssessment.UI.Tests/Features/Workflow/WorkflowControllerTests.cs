using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk;
using FluentValidation;
using FluentValidation.Results;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.LoadCheckYourAnswersScreen;
using He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
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
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
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
            var activityTypeRouteValue = new KeyValuePair<string, object?>("ActivityType", loadWorkflowActivityRequest.ActivityType);

            Assert.Contains(activityIdRouteValue, redirectToActionResult.RouteValues!);
            Assert.Contains(workflowInstanceIdRouteValue, redirectToActionResult.RouteValues!);
            Assert.Contains(activityTypeRouteValue, redirectToActionResult.RouteValues!);
        }

        [Theory]
        [AutoMoqData]
        public async Task QuestionScreenSaveAndContinue_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
            [Frozen] Mock<IMediator> mediator,
            QuestionScreenSaveAndContinueCommand command,
            Exception exception,
            WorkflowController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.QuestionScreenSaveAndContinue(command);

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
        public async Task QuestionScreenSaveAndContinue_ShouldRedirectToAction_GivenValidationIssuesAreFound(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IValidator<QuestionScreenSaveAndContinueCommand>> validator,
            QuestionScreenSaveAndContinueCommand command,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            ValidationResult validationResult,
            WorkflowController sut)
        {
            //Arrange


            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(saveAndContinueCommandResponse);
            validator.Setup(x => x.Validate(It.IsAny<QuestionScreenSaveAndContinueCommand>()))
                .Returns(validationResult);
            //Act
            var result = await sut.QuestionScreenSaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var redirectToActionResult = (ViewResult)result;
            Assert.Equal("MultiSaveAndContinue", redirectToActionResult.ViewName);

        }

        [Theory]
        [AutoMoqData]
        public async Task QuestionScreenSaveAndContinue_ShouldRedirectToAction_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            QuestionScreenSaveAndContinueCommand command,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            WorkflowController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(saveAndContinueCommandResponse);

            //Act
            var result = await sut.QuestionScreenSaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("LoadWorkflowActivity", redirectToActionResult.ActionName);


            var activityIdRouteValue = new KeyValuePair<string, object?>("ActivityId", saveAndContinueCommandResponse.ActivityId);
            var workflowInstanceIdRouteValue = new KeyValuePair<string, object?>("WorkflowInstanceId", saveAndContinueCommandResponse.WorkflowInstanceId);
            var activityTypeRouteValue = new KeyValuePair<string, object?>("ActivityType", saveAndContinueCommandResponse.ActivityType);

            Assert.Contains(activityIdRouteValue, redirectToActionResult.RouteValues!);
            Assert.Contains(workflowInstanceIdRouteValue, redirectToActionResult.RouteValues!);
            Assert.Contains(activityTypeRouteValue, redirectToActionResult.RouteValues!);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaughtForCheckYourAnswers(
            [Frozen] Mock<IMediator> mediator,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            Exception exception,
            WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.ActivityType = ActivityTypeConstants.CheckYourAnswersScreen;

            mediator.Setup(x =>
                x.Send(It.IsAny<LoadCheckYourAnswersScreenRequest>(), CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.LoadWorkflowActivity(saveAndContinueCommandResponse);

            //Assert
            mediator.Verify(x => x.Send(It.Is<LoadCheckYourAnswersScreenRequest>(y =>
                y.ActivityId == saveAndContinueCommandResponse.ActivityId && y.WorkflowInstanceId ==
                saveAndContinueCommandResponse.WorkflowInstanceId), CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(It.Is<LoadCheckYourAnswersScreenRequest>(x =>
                x.ActivityId == saveAndContinueCommandResponse.ActivityId && x.WorkflowInstanceId ==
                saveAndContinueCommandResponse.WorkflowInstanceId)));

            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaughtForQuestionScreen(
            [Frozen] Mock<IMediator> mediator,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            Exception exception,
            WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.ActivityType = ActivityTypeConstants.QuestionScreen;

            mediator.Setup(x =>
                x.Send(It.IsAny<LoadQuestionScreenRequest>(), CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.LoadWorkflowActivity(saveAndContinueCommandResponse);

            //Assert
            mediator.Verify(x => x.Send(It.Is<LoadQuestionScreenRequest>(y =>
                y.ActivityId == saveAndContinueCommandResponse.ActivityId && y.WorkflowInstanceId ==
                saveAndContinueCommandResponse.WorkflowInstanceId), CancellationToken.None), Times.Once);
            await Assert.ThrowsAsync<Exception>(() => mediator.Object.Send(It.Is<LoadQuestionScreenRequest>(x =>
                x.ActivityId == saveAndContinueCommandResponse.ActivityId && x.WorkflowInstanceId ==
                saveAndContinueCommandResponse.WorkflowInstanceId)));

            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldRedirectToCheckYourAnswersView_GivenCheckYourAnswersScreenAndNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.ActivityType = ActivityTypeConstants.CheckYourAnswersScreen;

            mediator.Setup(x =>
                    x.Send(
                        It.Is<LoadCheckYourAnswersScreenRequest>(y =>
                            y.ActivityId == saveAndContinueCommandResponse.ActivityId && y.WorkflowInstanceId ==
                            saveAndContinueCommandResponse.WorkflowInstanceId), CancellationToken.None))
                .ReturnsAsync(saveAndContinueCommand);

            //Act
            var result = await sut.LoadWorkflowActivity(saveAndContinueCommandResponse);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.Equal("CheckYourAnswers", viewResult.ViewName);
            Assert.IsType<QuestionScreenSaveAndContinueCommand>(viewResult.Model); // this will change

        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldRedirectToConfirmationView_GivenConfirmationScreenAndNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            LoadConfirmationScreenResponse response,
            WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.ActivityType = ActivityTypeConstants.ConfirmationScreen;

            mediator.Setup(x =>
                    x.Send(
                        It.Is<LoadConfirmationScreenRequest>(y =>
                            y.ActivityId == saveAndContinueCommandResponse.ActivityId && y.WorkflowInstanceId ==
                            saveAndContinueCommandResponse.WorkflowInstanceId), CancellationToken.None))
                .ReturnsAsync(response);

            //Act
            var result = await sut.LoadWorkflowActivity(saveAndContinueCommandResponse);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.Equal("Confirmation", viewResult.ViewName);
            Assert.IsType<LoadConfirmationScreenResponse>(viewResult.Model);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldRedirectToMultiSaveAndContinueView_GivenQuestionScreenAndNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.ActivityType = ActivityTypeConstants.QuestionScreen;

            mediator.Setup(x =>
                    x.Send(
                        It.Is<LoadQuestionScreenRequest>(y =>
                            y.ActivityId == saveAndContinueCommandResponse.ActivityId && y.WorkflowInstanceId ==
                            saveAndContinueCommandResponse.WorkflowInstanceId), CancellationToken.None))
                .ReturnsAsync(saveAndContinueCommand);

            //Act
            var result = await sut.LoadWorkflowActivity(saveAndContinueCommandResponse);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.Equal("MultiSaveAndContinue", viewResult.ViewName);
            Assert.IsType<QuestionScreenSaveAndContinueCommand>(viewResult.Model);

        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldThrowApplicationException_GivenUnknownActivityType(
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.ActivityType = "UnknownType";

            //Act
            var result = await sut.LoadWorkflowActivity(saveAndContinueCommandResponse);

            //Assert
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("Error", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            var exceptionRouteValue = new KeyValuePair<string, object?>("message", "Attempted to load unsupported activity type: UnknownType");

            Assert.Contains(exceptionRouteValue, redirectToActionResult.RouteValues!);
        }

        [Theory]
        [AutoMoqData]
        public async Task CheckYourAnswerScreenSaveAndContinue_ShouldRedirectToErrorPage_GivenInnerExceptionIsCaught(
          [Frozen] Mock<IMediator> mediator,
          CheckYourAnswersSaveAndContinueCommand command,
          Exception exception,
          WorkflowController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).Throws(exception);

            //Act
            var result = await sut.CheckYourAnswerScreenSaveAndContinue(command);

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
        public async Task CheckYourAnswerScreenSaveAndContinue_ShouldRedirectToAction_GivenNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            CheckYourAnswersSaveAndContinueCommand command,
            CheckYourAnswersSaveAndContinueCommandResponse response,
            WorkflowController sut)
        {
            //Arrange
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            //Act
            var result = await sut.CheckYourAnswerScreenSaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectToActionResult = (RedirectToActionResult)result;

            Assert.Equal("LoadWorkflowActivity", redirectToActionResult.ActionName);

            var activityIdRouteValue = new KeyValuePair<string, object?>("ActivityId", response.ActivityId);
            var activityTypeRouteValue = new KeyValuePair<string, object?>("ActivityType", response.ActivityType);
            var workflowInstanceIdRouteValue = new KeyValuePair<string, object?>("WorkflowInstanceId", response.WorkflowInstanceId);

            Assert.Contains(activityIdRouteValue, redirectToActionResult.RouteValues!);
            Assert.Contains(activityTypeRouteValue, redirectToActionResult.RouteValues!);
            Assert.Contains(workflowInstanceIdRouteValue, redirectToActionResult.RouteValues!);
        }
    }
}
