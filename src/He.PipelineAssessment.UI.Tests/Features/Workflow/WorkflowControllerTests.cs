using AutoFixture.Xunit2;
using Azure.Core;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using FluentValidation;
using FluentValidation.Results;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Exceptions;
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
using Xunit.Sdk;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow
{
    public class WorkflowControllerTests
    {
       

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
        public async Task QuestionScreenSaveAndContinue_ShouldRedirectToAction_GivenValidationIssuesAreFound(
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<IElsaServerHttpClient> http,
            QuestionScreenSaveAndContinueCommand command,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            WorkflowNextActivityDataDto httpResponse,
            ValidationResult validationResult,
            WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.IsValid = false;
            saveAndContinueCommandResponse.ValidationMessages = validationResult;
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(saveAndContinueCommandResponse);
            
            //Act
            var result = await sut.QuestionScreenSaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var redirectToActionResult = (ViewResult)result;
            Assert.Equal("SaveAndContinue", redirectToActionResult.ViewName);

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
            saveAndContinueCommandResponse.IsAuthorised = true;
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
        public async Task QuestionScreenSaveAndContinue_ShouldRedirectToAction_GivenIncorrectBusinessArea(
           [Frozen] Mock<IMediator> mediator,
           QuestionScreenSaveAndContinueCommand command,
           QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
           WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.IsAuthorised = false;
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(saveAndContinueCommandResponse);

            //Act
            var result = await sut.QuestionScreenSaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("AccessDenied", redirectToActionResult.ActionName);
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
            saveAndContinueCommand.IsAuthorised = true;
            saveAndContinueCommand.IsReadOnly = false;
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
        public async Task LoadWorkflowActivity_ShouldRedirectToLoadReadOnlyWorkflowActivity_GivenCheckYourAnswersScreenAndIncorrectCorrectBusinessArea(
            [Frozen] Mock<IMediator> mediator,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.ActivityType = ActivityTypeConstants.CheckYourAnswersScreen;
            saveAndContinueCommand.IsAuthorised = false;

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
            Assert.IsType<RedirectToActionResult>(result);
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("LoadReadOnlyWorkflowActivity", redirectToActionResult.ActionName);
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
        public async Task LoadWorkflowActivity_ShouldRedirectToLoadReadOnlyWorkflowActivity_GivenIncorrectBusinessArea(
           [Frozen] Mock<IMediator> mediator,
           QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
           QuestionScreenSaveAndContinueCommand response,
           WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.ActivityType = ActivityTypeConstants.QuestionScreen;
            response.IsAuthorised = false;

            mediator.Setup(x =>
                    x.Send(
                        It.Is<LoadQuestionScreenRequest>(y =>
                            y.ActivityId == saveAndContinueCommandResponse.ActivityId && y.WorkflowInstanceId ==
                            saveAndContinueCommandResponse.WorkflowInstanceId), CancellationToken.None))
                .ReturnsAsync(response);

            //Act
            var result = await sut.LoadWorkflowActivity(saveAndContinueCommandResponse);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("LoadReadOnlyWorkflowActivity", redirectToActionResult.ActionName);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldRedirectToSaveAndContinueView_GivenQuestionScreenAndNoExceptionsThrow(
            [Frozen] Mock<IMediator> mediator,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommand.IsAuthorised = true;
            saveAndContinueCommand.IsReadOnly = false;
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
            Assert.Equal("SaveAndContinue", viewResult.ViewName);
            Assert.IsType<QuestionScreenSaveAndContinueCommand>(viewResult.Model);

        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldRedirectToLoadReadOnlyWorkflowActivity_GivenQuestionScreenAndIsNotCorrectBusinessArea(
          [Frozen] Mock<IMediator> mediator,
          QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
          QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
          WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.ActivityType = ActivityTypeConstants.QuestionScreen;
            saveAndContinueCommand.IsValid = false;
            saveAndContinueCommand.IsReadOnly = true;


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
            Assert.IsType<RedirectToActionResult>(result);
            var redirectToActionResult = (RedirectToActionResult)result;
            Assert.Equal("LoadReadOnlyWorkflowActivity", redirectToActionResult.ActionName);
        }


        [Theory]
        [AutoMoqData]
        public async Task LoadReadOnlyWorkflowActivity_ShouldRedirectToCheckYourAnswersView_GivenConfirmationScreenAndNoExceptionsThrow(
          [Frozen] Mock<IMediator> mediator,
          QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
          QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
          WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommandResponse.ActivityType = ActivityTypeConstants.ConfirmationScreen;

            mediator.Setup(x =>
                    x.Send(
                        It.Is<LoadCheckYourAnswersScreenRequest>(y =>
                            y.ActivityId == saveAndContinueCommandResponse.ActivityId && y.WorkflowInstanceId ==
                            saveAndContinueCommandResponse.WorkflowInstanceId), CancellationToken.None))
                .ReturnsAsync(saveAndContinueCommand);

            //Act
            var result = await sut.LoadReadOnlyWorkflowActivity(saveAndContinueCommandResponse);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.Equal("CheckYourAnswersReadOnly", viewResult.ViewName);
            Assert.IsType<QuestionScreenSaveAndContinueCommand>(viewResult.Model);
        }

      


        [Theory]
        [AutoMoqData]
        public async Task LoadReadOnlyWorkflowActivity_ShouldRedirectToCheckYourAnswersView_GivenCheckYourAnswersScreenAndNoExceptionsThrow(
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
            var result = await sut.LoadReadOnlyWorkflowActivity(saveAndContinueCommandResponse);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.Equal("CheckYourAnswersReadOnly", viewResult.ViewName);
            Assert.IsType<QuestionScreenSaveAndContinueCommand>(viewResult.Model); // this will change
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ShouldRedirectToLoadWorkflowActivity_GivenDataScourceActivityType(
            [Frozen] Mock<IMediator> mediator,
            QuestionScreenSaveAndContinueCommandResponse saveAndContinueCommandResponse,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            WorkflowController sut)
        {
            //Arrange
            saveAndContinueCommand.IsAuthorised = true;
            saveAndContinueCommand.IsReadOnly = false;
            saveAndContinueCommandResponse.ActivityType = ActivityTypeConstants.SinglePipelineDataSource;

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
            Assert.IsType<RedirectToActionResult>(result);

            var viewResult = (RedirectToActionResult)result;
            Assert.Equal("LoadWorkflowActivity", viewResult.ActionName);

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
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.LoadWorkflowActivity(saveAndContinueCommandResponse));

            //Assert
            Assert.Equal("Attempted to load unsupported activity type: UnknownType",ex.Message);

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
            response.IsAuthorised = true;
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            //AcT
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

        [Theory]
        [AutoMoqData]
        public async Task CheckYourAnswerScreenSaveAndContinue_ShouldRedirectToAction_GivenIncorrectBusinessArea(
           [Frozen] Mock<IMediator> mediator,
           CheckYourAnswersSaveAndContinueCommand command,
           CheckYourAnswersSaveAndContinueCommandResponse response,
           WorkflowController sut)
        {
            //Arrange
            response.IsAuthorised = false;
            mediator.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(response);

            //AcT
            var result = await sut.CheckYourAnswerScreenSaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);

            var redirectToActionResult = (RedirectToActionResult)result;

            Assert.Equal("AccessDenied", redirectToActionResult.ActionName);

        }
    }
}
