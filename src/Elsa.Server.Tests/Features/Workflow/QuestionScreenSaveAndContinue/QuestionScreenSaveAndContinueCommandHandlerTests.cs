using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Features.Workflow.Helpers;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.QuestionScreenSaveAndContinue;

public class QuestionScreenSaveAndContinueCommandHandlerTests
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
            List<QuestionScreenAnswer> currentAssessmentQuestions,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation nextAssessmentActivity,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            QuestionScreenSaveAndContinueCommandHandler sut)
    {
        //Arrange
        for (var i = 0; i < currentAssessmentQuestions.Count; i++)
        {
            var questionId = saveAndContinueCommand.Answers![i].Id;
            currentAssessmentQuestions[i].QuestionId = questionId;
        }

        var collectedWorkflow = new CollectedWorkflow(saveAndContinueCommand.WorkflowInstanceId, null, null);
        var opResult = new OperationResult<QuestionScreenSaveAndContinueResponse>
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
        workflowBlueprint.Activities.Add(activityBlueprint);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(currentAssessmentQuestions);

        questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                ActivityTypeConstants.QuestionScreen,
                saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
            .ReturnsAsync(new List<CollectedWorkflow>(){ collectedWorkflow });

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(nextAssessmentActivity);

        workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        workflowNextActivityProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
            .ReturnsAsync(activityBlueprint);

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Once);
        elsaCustomRepository.Verify(
            x => x.CreateCustomActivityNavigationAsync(nextAssessmentActivity, CancellationToken.None), Times.Never);

        Assert.Equal(saveAndContinueCommand.Answers![0].AnswerText, currentAssessmentQuestions[0].Answer);
        Assert.Equal(saveAndContinueCommand.Answers![1].AnswerText, currentAssessmentQuestions[1].Answer);
        Assert.Equal(saveAndContinueCommand.Answers![2].AnswerText, currentAssessmentQuestions[2].Answer);

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
            List<QuestionScreenAnswer> currentAssessmentQuestions,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation nextAssessmentActivity,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            string nextActivityType,
            QuestionScreenSaveAndContinueCommandHandler sut
        )
    {
        //Arrange
        var opResult = new OperationResult<QuestionScreenSaveAndContinueResponse>
        {
            Data = new QuestionScreenSaveAndContinueResponse
            {
                WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
                NextActivityId = workflowInstance.Output!.ActivityId
            },
            ErrorMessages = new List<string>(),
            ValidationMessages = null
        };
        var collectedWorkflow = new CollectedWorkflow(saveAndContinueCommand.WorkflowInstanceId, null, null);

        activityBlueprint.Id = workflowInstance.Output.ActivityId;
        activityBlueprint.Type = nextActivityType;
        workflowBlueprint.Activities.Add(activityBlueprint);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(currentAssessmentQuestions);

        questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                ActivityTypeConstants.QuestionScreen,
                saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
            .ReturnsAsync(new List<CollectedWorkflow>() { collectedWorkflow });

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync((CustomActivityNavigation?)null);

        workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        workflowNextActivityProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
            .ReturnsAsync(activityBlueprint);

        saveAndContinueHelper.Setup(x => x.CreateNextCustomActivityNavigation(saveAndContinueCommand.ActivityId,
                ActivityTypeConstants.QuestionScreen, activityBlueprint.Id, activityBlueprint.Type,
                workflowInstance))
            .Returns(nextAssessmentActivity);


        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Once);
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
            List<QuestionScreenAnswer> currentAssessmentQuestions,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation nextAssessmentActivity,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            List<QuestionScreenAnswer> nextAssessmentQuestions,
            QuestionScreenSaveAndContinueCommandHandler sut
        )
    {
        //Arrange
        var opResult = new OperationResult<QuestionScreenSaveAndContinueResponse>
        {
            Data = new QuestionScreenSaveAndContinueResponse
            {
                WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
                NextActivityId = workflowInstance.Output!.ActivityId
            },
            ErrorMessages = new List<string>(),
            ValidationMessages = null
        };
        var collectedWorkflow = new CollectedWorkflow(saveAndContinueCommand.WorkflowInstanceId, null, null);

        activityBlueprint.Id = workflowInstance.Output.ActivityId;
        activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
        workflowBlueprint.Activities.Add(activityBlueprint);

        nextAssessmentActivity.ActivityType = ActivityTypeConstants.QuestionScreen;


        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(currentAssessmentQuestions);

        questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                ActivityTypeConstants.QuestionScreen,
                saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
            .ReturnsAsync(new List<CollectedWorkflow>(){collectedWorkflow});

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync((CustomActivityNavigation?)null);

        workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        workflowNextActivityProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
            .ReturnsAsync(activityBlueprint);

        saveAndContinueHelper.Setup(x => x.CreateNextCustomActivityNavigation(saveAndContinueCommand.ActivityId,
                ActivityTypeConstants.QuestionScreen, activityBlueprint.Id, activityBlueprint.Type,
                workflowInstance))
            .Returns(nextAssessmentActivity);

        saveAndContinueHelper.Setup(x => x.CreateQuestionScreenAnswers(activityBlueprint.Id, workflowInstance))
            .Returns(nextAssessmentQuestions);

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Once);
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
    public async Task Handle_ShouldReturnErrors_WhenAssessmentQuestionsNotFoundInDatabase(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
        QuestionScreenSaveAndContinueCommandHandler sut)
    {
        //Arrange
        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(new List<QuestionScreenAnswer>());

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Never);
        elsaCustomRepository.Verify(
            x => x.CreateCustomActivityNavigationAsync(It.IsAny<CustomActivityNavigation>(), CancellationToken.None),
            Times.Never);
        Assert.Null(result.Data);
        Assert.Equal(
            $"Unable to find any questions for Question Screen activity Workflow Instance Id: {saveAndContinueCommand.WorkflowInstanceId} and Activity Id: {saveAndContinueCommand.ActivityId} in custom database",
            result.ErrorMessages.Single());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldReturnErrors_WhenADependencyThrows(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        Exception exception,
        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
        QuestionScreenSaveAndContinueCommandHandler sut)
    {
        //Arrange
        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .Throws(exception);

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Never);
        elsaCustomRepository.Verify(
            x => x.CreateCustomActivityNavigationAsync(It.IsAny<CustomActivityNavigation>(), CancellationToken.None),
            Times.Never);
        Assert.Null(result.Data);
        Assert.Equal(exception.Message, result.ErrorMessages.Single());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldNotUpdateCustomDatabase_WhenThereAreNoCollectedWorkflows(
        [Frozen] Mock<IQuestionInvoker> questionInvoker,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
        WorkflowBlueprint workflowBlueprint,
        ActivityBlueprint activityBlueprint,
        List<QuestionScreenAnswer> currentAssessmentQuestions,
        WorkflowInstance workflowInstance,
        CustomActivityNavigation nextAssessmentActivity,
        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
        List<QuestionScreenAnswer> nextAssessmentQuestions,
        QuestionScreenSaveAndContinueCommandHandler sut
    )
    {
        //Arrange
        activityBlueprint.Id = workflowInstance.Output!.ActivityId;
        activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
        workflowBlueprint.Activities.Add(activityBlueprint);

        nextAssessmentActivity.ActivityType = ActivityTypeConstants.QuestionScreen;


        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(currentAssessmentQuestions);

        questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                ActivityTypeConstants.QuestionScreen,
                saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
            .ReturnsAsync(new List<CollectedWorkflow>());

        workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Never);
        elsaCustomRepository.Verify(
            x => x.CreateCustomActivityNavigationAsync(nextAssessmentActivity, CancellationToken.None), Times.Never);
        elsaCustomRepository.Verify(
            x => x.CreateQuestionScreenAnswersAsync(nextAssessmentQuestions, CancellationToken.None), Times.Never);
        Assert.Null(result.Data);
        Assert.Equal(
            $"Unable to save answers. Workflow status is: {workflowInstance.WorkflowStatus}",
            result.ErrorMessages.Single());
        Assert.Null(result.ValidationMessages);
    }

}