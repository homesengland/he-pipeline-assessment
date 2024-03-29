﻿using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Features.Workflow.CheckYourAnswersSaveAndContinue;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Helpers;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
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
                [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
                [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
                [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
                [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
                WorkflowBlueprint workflowBlueprint,
                ActivityBlueprint activityBlueprint,
                WorkflowInstance workflowInstance,
                CustomActivityNavigation nextAssessmentActivity,
                CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
                QuestionWorkflowInstance questionWorkflowInstance,
                CheckYourAnswersSaveAndContinueCommandHandler sut)
        {
            //Arrange

            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint
            };
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

            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(nextAssessmentActivity);

            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance(workflowInstance.Id, CancellationToken.None))
    .ReturnsAsync(questionWorkflowInstance);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowNextActivityProvider.Setup(x => x.GetNextActivity(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, null, ActivityTypeConstants.CheckYourAnswersScreen,
                    CancellationToken.None))
                .ReturnsAsync(workflowNextActivityModel);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.CreateCustomActivityNavigationAsync(nextAssessmentActivity, CancellationToken.None),
                Times.Never);
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId, nextAssessmentActivity,
                    workflowNextActivityModel.NextActivity, workflowInstance, questionWorkflowInstance.WorkflowName, CancellationToken.None), Times.Once);

            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndInsertsNewQuestionIfDoesNotExist(
                [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
                [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
                [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
                [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
                WorkflowBlueprint workflowBlueprint,
                ActivityBlueprint activityBlueprint,
                WorkflowInstance workflowInstance,
                CustomActivityNavigation nextAssessmentActivity,
                CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
                QuestionWorkflowInstance questionWorkflowInstance,
                CheckYourAnswersSaveAndContinueCommandHandler sut
            )
        {
            //Arrange
            nextAssessmentActivity.ActivityType = ActivityTypeConstants.QuestionScreen;
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint
            };
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

            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((CustomActivityNavigation?)null);


            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance(workflowInstance.Id, CancellationToken.None))
    .ReturnsAsync(questionWorkflowInstance);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowNextActivityProvider.Setup(x => x.GetNextActivity(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, null, ActivityTypeConstants.CheckYourAnswersScreen,
                    CancellationToken.None))
                .ReturnsAsync(workflowNextActivityModel);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId, null,
                    workflowNextActivityModel.NextActivity, workflowInstance, questionWorkflowInstance.WorkflowName, CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Equal(activityBlueprint.Type, result.Data.ActivityType);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndOnlyInsertsNavigationRecordForNonQuestionScreenType(
                [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
                [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
                [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
                [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
                WorkflowBlueprint workflowBlueprint,
                ActivityBlueprint activityBlueprint,
                WorkflowInstance workflowInstance,
                CustomActivityNavigation nextAssessmentActivity,
                CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
                QuestionWorkflowInstance questionWorkflowInstance,
                CheckYourAnswersSaveAndContinueCommandHandler sut
            )
        {
            //Arrange
            nextAssessmentActivity.ActivityType = "Not.QuestionScreen";
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint
            };
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

            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((CustomActivityNavigation?)null);

            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance(workflowInstance.Id, CancellationToken.None))
.ReturnsAsync(questionWorkflowInstance);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowNextActivityProvider.Setup(x => x.GetNextActivity(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, null, ActivityTypeConstants.CheckYourAnswersScreen,
                    CancellationToken.None))
                .ReturnsAsync(workflowNextActivityModel);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId, null,
                    workflowNextActivityModel.NextActivity, workflowInstance, questionWorkflowInstance.WorkflowName, CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Equal(activityBlueprint.Type, result.Data.ActivityType);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndInsertsNewMultiScreenQuestionIfDoesNotExist(
                [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
                [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
                [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
                [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
                WorkflowBlueprint workflowBlueprint,
                ActivityBlueprint activityBlueprint,
                WorkflowInstance workflowInstance,
                CustomActivityNavigation nextAssessmentActivity,
                CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
                QuestionWorkflowInstance questionWorkflowInstance,
                CheckYourAnswersSaveAndContinueCommandHandler sut
            )
        {
            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint
            };
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

            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((CustomActivityNavigation?)null);

            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance(workflowInstance.Id, CancellationToken.None))
.ReturnsAsync(questionWorkflowInstance);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowNextActivityProvider.Setup(x => x.GetNextActivity(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, null, ActivityTypeConstants.CheckYourAnswersScreen,
                    CancellationToken.None))
                .ReturnsAsync(workflowNextActivityModel);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId, null,
                    workflowNextActivityModel.NextActivity, workflowInstance, questionWorkflowInstance.WorkflowName, CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenADependencyThrows(
            [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
            Exception exception,
            CheckYourAnswersSaveAndContinueCommand command,
            CheckYourAnswersSaveAndContinueCommandHandler sut)
        {
            //Arrange
            workflowNextActivityProvider.Setup(x => x.GetNextActivity(command.ActivityId, command.WorkflowInstanceId,
                    null, ActivityTypeConstants.CheckYourAnswersScreen, CancellationToken.None))
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
            Handle_ShouldCallModelHelpersWithCorrectWorkflowInstance(
                [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
                [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
                [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
                [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
                WorkflowBlueprint workflowBlueprint,
                ActivityBlueprint activityBlueprint,
                WorkflowInstance workflowInstance,
                CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
                CustomActivityNavigation customActivityNavigation,
                QuestionWorkflowInstance questionWorkflowInstance,
                CheckYourAnswersSaveAndContinueCommandHandler sut)
        {
            //Arrange
            customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint,
            };

            activityBlueprint.Id = workflowInstance.Output!.ActivityId;
            workflowBlueprint.Activities.Add(activityBlueprint);

            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((CustomActivityNavigation?)null);
            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance(workflowInstance.Id, CancellationToken.None))
                .ReturnsAsync(questionWorkflowInstance);


            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowNextActivityProvider.Setup(x => x.GetNextActivity(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, null, ActivityTypeConstants.CheckYourAnswersScreen,
                    CancellationToken.None))
                .ReturnsAsync(workflowNextActivityModel);

            //Act
            await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId, null,
                    workflowNextActivityModel.NextActivity, workflowInstance, questionWorkflowInstance.WorkflowName, CancellationToken.None), Times.Once);
        }

    }
}
