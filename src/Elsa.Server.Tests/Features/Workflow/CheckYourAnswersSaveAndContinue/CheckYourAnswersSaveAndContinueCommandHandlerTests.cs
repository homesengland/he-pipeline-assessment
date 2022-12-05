using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Features.Workflow.CheckYourAnswersSaveAndContinue;
using Elsa.Server.Features.Workflow.Helpers;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task
            Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndDoesNotInsertNewQuestionIfAlreadyExists(
                [Frozen] Mock<IQuestionInvoker> questionInvoker,
                [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
                [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
                [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
                WorkflowBlueprint workflowBlueprint,
                ActivityBlueprint activityBlueprint,
                List<CollectedWorkflow> collectedWorkflows,
                WorkflowInstance workflowInstance,
                CustomActivityNavigation nextAssessmentActivity,
                CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
                CheckYourAnswersSaveAndContinueCommandHandler sut)
        {
            //Arrange
            var opResult = new OperationResult<CheckYourAnswersSaveAndContinueResponse>()
            {
                Data = new CheckYourAnswersSaveAndContinueResponse
                {
                    WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
                    NextActivityId = workflowInstance.Output!.ActivityId
                },
                ErrorMessages = new List<string>(),
                ValidationMessages = null
            };

            activityBlueprint.Id = workflowInstance.Output.ActivityId;
            workflowBlueprint.Activities.Add(activityBlueprint);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.CheckYourAnswersScreen,
                    saveAndContinueCommand.WorkflowInstanceId, null, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(nextAssessmentActivity);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(workflowInstance);

            workflowNextActivityProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(activityBlueprint);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.CreateCustomActivityNavigationAsync(nextAssessmentActivity, CancellationToken.None), Times.Never);


            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task
          Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndInsertsNewQuestionIfDoesNotExist(
              [Frozen] Mock<IQuestionInvoker> questionInvoker,
              [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
              [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
              [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
              [Frozen] Mock<ISaveAndContinueHelper> saveAndContinueHelper,
              WorkflowBlueprint workflowBlueprint,
              ActivityBlueprint activityBlueprint,
              List<CollectedWorkflow> collectedWorkflows,
              WorkflowInstance workflowInstance,
              CustomActivityNavigation nextAssessmentActivity,
              string nextActivityType,
              CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
              CheckYourAnswersSaveAndContinueCommandHandler sut
          )
        {
            //Arrange
            var opResult = new OperationResult<CheckYourAnswersSaveAndContinueResponse>()
            {
                Data = new CheckYourAnswersSaveAndContinueResponse
                {
                    WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
                    NextActivityId = workflowInstance.Output!.ActivityId
                },
                ErrorMessages = new List<string>(),
                ValidationMessages = null
            };

            activityBlueprint.Id = workflowInstance.Output.ActivityId;
            activityBlueprint.Type = nextActivityType;
            workflowBlueprint.Activities.Add(activityBlueprint);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.CheckYourAnswersScreen,
                    saveAndContinueCommand.WorkflowInstanceId, null, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((CustomActivityNavigation?)null);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(workflowInstance);

            workflowNextActivityProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(activityBlueprint);

            saveAndContinueHelper.Setup(x => x.CreateNextCustomActivityNavigation(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.CheckYourAnswersScreen, activityBlueprint.Id, activityBlueprint.Type,
                    workflowInstance))
                .Returns(nextAssessmentActivity);


            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert

            elsaCustomRepository.Verify(
                x => x.CreateCustomActivityNavigationAsync(nextAssessmentActivity, CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task
        Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndInsertsNewMultiScreenQuestionIfDoesNotExist(
              [Frozen] Mock<IQuestionInvoker> questionInvoker,
              [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
              [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
              [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
              [Frozen] Mock<ISaveAndContinueHelper> saveAndContinueHelper,
              WorkflowBlueprint workflowBlueprint,
              ActivityBlueprint activityBlueprint,
              List<CollectedWorkflow> collectedWorkflows,
              WorkflowInstance workflowInstance,
              CustomActivityNavigation nextAssessmentActivity,
              List<QuestionScreenAnswer> nextAssessmentQuestions,
              CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
              CheckYourAnswersSaveAndContinueCommandHandler sut
          )
        {
            //Arrange
            var opResult = new OperationResult<QuestionScreenSaveAndContinueResponse>()
            {
                Data = new QuestionScreenSaveAndContinueResponse
                {
                    WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
                    NextActivityId = workflowInstance.Output!.ActivityId
                },
                ErrorMessages = new List<string>(),
                ValidationMessages = null
            };

            activityBlueprint.Id = workflowInstance.Output.ActivityId;
            activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
            workflowBlueprint.Activities.Add(activityBlueprint);

            nextAssessmentActivity.ActivityType = ActivityTypeConstants.QuestionScreen;


            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.CheckYourAnswersScreen,
                    saveAndContinueCommand.WorkflowInstanceId, null, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((CustomActivityNavigation?)null);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(workflowInstance);

            workflowNextActivityProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(activityBlueprint);

            saveAndContinueHelper.Setup(x => x.CreateNextCustomActivityNavigation(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.CheckYourAnswersScreen, activityBlueprint.Id, activityBlueprint.Type,
                    workflowInstance))
                .Returns(nextAssessmentActivity);

            saveAndContinueHelper.Setup(x => x.CreateQuestionScreenAnswers(activityBlueprint.Id, workflowInstance)).Returns(nextAssessmentQuestions);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.CreateCustomActivityNavigationAsync(nextAssessmentActivity, CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(
                x => x.CreateQuestionScreenAnswersAsync(nextAssessmentQuestions, CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenADependencyThrows(
            [Frozen] Mock<IQuestionInvoker> invoker,
            Exception exception,
            CheckYourAnswersSaveAndContinueCommand command,
            CheckYourAnswersSaveAndContinueCommandHandler sut)
        {
            //Arrange
            invoker.Setup(x => x.ExecuteWorkflowsAsync(command.ActivityId, ActivityTypeConstants.CheckYourAnswersScreen, command.WorkflowInstanceId, null, CancellationToken.None))
                .Throws(exception);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert

            Assert.Null(result.Data);
            Assert.Equal(exception.Message, result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task
        Handle_ShouldReturnError_WhenNoCollectedWorkflows(
              [Frozen] Mock<IQuestionInvoker> questionInvoker,
              [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
              [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
              WorkflowBlueprint workflowBlueprint,
              ActivityBlueprint activityBlueprint,
              WorkflowInstance workflowInstance,
              CustomActivityNavigation nextAssessmentActivity,
              List<QuestionScreenAnswer> nextAssessmentQuestions,
              CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
              CheckYourAnswersSaveAndContinueCommandHandler sut
          )
        {
            //Arrange
            activityBlueprint.Id = workflowInstance.Output!.ActivityId;
            activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
            workflowBlueprint.Activities.Add(activityBlueprint);

            nextAssessmentActivity.ActivityType = ActivityTypeConstants.QuestionScreen;


            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.CheckYourAnswersScreen,
                    saveAndContinueCommand.WorkflowInstanceId, null, CancellationToken.None))
                .ReturnsAsync(new List<CollectedWorkflow>());

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(workflowInstance);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.CreateCustomActivityNavigationAsync(nextAssessmentActivity, CancellationToken.None), Times.Never);
            elsaCustomRepository.Verify(
                x => x.CreateQuestionScreenAnswersAsync(nextAssessmentQuestions, CancellationToken.None), Times.Never);
            Assert.Null(result.Data);
            Assert.Equal(
                $"Unable to progress. Workflow status is: {workflowInstance.WorkflowStatus}",
                result.ErrorMessages.Single());
            Assert.Null(result.ValidationMessages);
        }
    }
}
