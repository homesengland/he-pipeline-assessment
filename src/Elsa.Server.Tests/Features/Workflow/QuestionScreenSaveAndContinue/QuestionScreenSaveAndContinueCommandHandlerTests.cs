using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
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
        Handle_ShouldReturnSuccessfulOperationResultAndCallAllDependencies_WhenSuccessful(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
            [Frozen] Mock<IDeleteChangedWorkflowPathService> deleteChangedWorkflowPathService,
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

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(nextAssessmentActivity);

        workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        workflowNextActivityProvider.Setup(x => x.GetNextActivity(saveAndContinueCommand.ActivityId, saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, ActivityTypeConstants.QuestionScreen, CancellationToken.None))
            .ReturnsAsync(activityBlueprint);

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Once);

        nextActivityNavigationService.Verify(
            x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId,
                nextAssessmentActivity, activityBlueprint, workflowInstance, CancellationToken.None), Times.Once);

        deleteChangedWorkflowPathService.Verify(
            x => x.DeleteChangedWorkflowPath(saveAndContinueCommand.WorkflowInstanceId, saveAndContinueCommand.ActivityId,
                activityBlueprint, workflowInstance, CancellationToken.None), Times.Once);

        workflowInstanceProvider.Verify(
            x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Once);

        Assert.Equal(saveAndContinueCommand.Answers![0].AnswerText, currentAssessmentQuestions[0].Answer);
        Assert.Equal(saveAndContinueCommand.Answers![1].AnswerText, currentAssessmentQuestions[1].Answer);
        Assert.Equal(saveAndContinueCommand.Answers![2].AnswerText, currentAssessmentQuestions[2].Answer);

        Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
        Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
        Assert.Equal(opResult.Data.ActivityType, result.Data.ActivityType);
        Assert.Empty(result.ErrorMessages);
        Assert.Null(result.ValidationMessages);
    }

    //[Theory]
    //[AutoMoqData]
    //public async Task
    //    Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndInsertsNewActivityNavigationIfDoesNotExist(
    //        [Frozen] Mock<IQuestionInvoker> questionInvoker,
    //        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
    //        [Frozen] Mock<IWorkflowRegistryProvider> workflowNextActivityProvider,
    //        [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
    //        [Frozen] Mock<IElsaCustomModelHelper> saveAndContinueHelper,
    //        [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
    //        WorkflowBlueprint workflowBlueprint,
    //        ActivityBlueprint activityBlueprint,
    //        List<QuestionScreenAnswer> currentAssessmentQuestions,
    //        WorkflowInstance workflowInstance,
    //        CustomActivityNavigation nextAssessmentActivity,
    //        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
    //        string nextActivityType,
    //        DateTime date,
    //        QuestionScreenSaveAndContinueCommandHandler sut
    //    )
    //{
    //    //Arrange
    //    var opResult = new OperationResult<QuestionScreenSaveAndContinueResponse>
    //    {
    //        Data = new QuestionScreenSaveAndContinueResponse
    //        {
    //            WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
    //            NextActivityId = workflowInstance.Output!.ActivityId
    //        },
    //        ErrorMessages = new List<string>(),
    //        ValidationMessages = null
    //    };
    //    var collectedWorkflow = new CollectedWorkflow(saveAndContinueCommand.WorkflowInstanceId, null, null);

    //    activityBlueprint.Id = workflowInstance.Output.ActivityId;
    //    activityBlueprint.Type = nextActivityType;
    //    workflowBlueprint.Activities.Add(activityBlueprint);

    //    elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
    //            saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
    //        .ReturnsAsync(currentAssessmentQuestions);

    //    questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
    //            ActivityTypeConstants.QuestionScreen,
    //            saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
    //        .ReturnsAsync(new List<CollectedWorkflow>() { collectedWorkflow });

    //    elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
    //            saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
    //        .ReturnsAsync((CustomActivityNavigation?)null);

    //    workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
    //        .ReturnsAsync(workflowInstance);

    //    workflowNextActivityProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
    //        .ReturnsAsync(activityBlueprint);

    //    saveAndContinueHelper.Setup(x => x.CreateNextCustomActivityNavigation(saveAndContinueCommand.ActivityId,
    //            ActivityTypeConstants.QuestionScreen, activityBlueprint.Id, activityBlueprint.Type,
    //            workflowInstance))
    //        .Returns(nextAssessmentActivity);
    //    dateTimeProvider.Setup(x => x.UtcNow()).Returns(date);


    //    //Act
    //    var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

    //    //Assert
    //    elsaCustomRepository.Verify(
    //        x => x.SaveChanges(CancellationToken.None), Times.Once);
    //    elsaCustomRepository.Verify(
    //        x => x.CreateCustomActivityNavigationAsync(nextAssessmentActivity, CancellationToken.None), Times.Once);
    //    elsaCustomRepository.Verify(
    //        x => x.UpdateCustomActivityNavigation(nextAssessmentActivity, CancellationToken.None), Times.Never);
    //    workflowInstanceProvider.Verify(
    //        x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Exactly(2));
    //    Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
    //    Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
    //    Assert.Empty(result.ErrorMessages);
    //    Assert.Null(result.ValidationMessages);
    //}


    //[Theory]
    //[AutoMoqData]
    //public async Task
    //    Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndInsertsNewMultiScreenQuestionIfDoesNotExist(
    //        [Frozen] Mock<IQuestionInvoker> questionInvoker,
    //        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
    //        [Frozen] Mock<IWorkflowRegistryProvider> workflowNextActivityProvider,
    //        [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
    //        [Frozen] Mock<IElsaCustomModelHelper> saveAndContinueHelper,
    //        WorkflowBlueprint workflowBlueprint,
    //        ActivityBlueprint activityBlueprint,
    //        List<QuestionScreenAnswer> currentAssessmentQuestions,
    //        WorkflowInstance workflowInstance,
    //        CustomActivityNavigation nextAssessmentActivity,
    //        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
    //        List<QuestionScreenAnswer> nextAssessmentQuestions,
    //        QuestionScreenSaveAndContinueCommandHandler sut
    //    )
    //{
    //    //Arrange
    //    var opResult = new OperationResult<QuestionScreenSaveAndContinueResponse>
    //    {
    //        Data = new QuestionScreenSaveAndContinueResponse
    //        {
    //            WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
    //            NextActivityId = workflowInstance.Output!.ActivityId
    //        },
    //        ErrorMessages = new List<string>(),
    //        ValidationMessages = null
    //    };
    //    var collectedWorkflow = new CollectedWorkflow(saveAndContinueCommand.WorkflowInstanceId, null, null);

    //    activityBlueprint.Id = workflowInstance.Output.ActivityId;
    //    activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
    //    workflowBlueprint.Activities.Add(activityBlueprint);

    //    nextAssessmentActivity.ActivityType = ActivityTypeConstants.QuestionScreen;


    //    elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
    //            saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
    //        .ReturnsAsync(currentAssessmentQuestions);

    //    questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
    //            ActivityTypeConstants.QuestionScreen,
    //            saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
    //        .ReturnsAsync(new List<CollectedWorkflow>() { collectedWorkflow });

    //    elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
    //            saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
    //        .ReturnsAsync((CustomActivityNavigation?)null);

    //    workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
    //        .ReturnsAsync(workflowInstance);

    //    workflowNextActivityProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
    //        .ReturnsAsync(activityBlueprint);

    //    saveAndContinueHelper.Setup(x => x.CreateNextCustomActivityNavigation(saveAndContinueCommand.ActivityId,
    //            ActivityTypeConstants.QuestionScreen, activityBlueprint.Id, activityBlueprint.Type,
    //            workflowInstance))
    //        .Returns(nextAssessmentActivity);

    //    saveAndContinueHelper.Setup(x => x.CreateQuestionScreenAnswers(activityBlueprint.Id, workflowInstance))
    //        .Returns(nextAssessmentQuestions);

    //    //Act
    //    var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

    //    //Assert
    //    elsaCustomRepository.Verify(
    //        x => x.SaveChanges(CancellationToken.None), Times.Once);
    //    elsaCustomRepository.Verify(
    //        x => x.CreateCustomActivityNavigationAsync(nextAssessmentActivity, CancellationToken.None), Times.Once);
    //    elsaCustomRepository.Verify(
    //        x => x.CreateQuestionScreenAnswersAsync(nextAssessmentQuestions, CancellationToken.None), Times.Once);
    //    workflowInstanceProvider.Verify(
    //        x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Exactly(2));
    //    Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
    //    Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
    //    Assert.Empty(result.ErrorMessages);
    //    Assert.Null(result.ValidationMessages);
    //}


    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldReturnErrors_WhenAssessmentQuestionsNotFoundInDatabase(
        [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
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
        workflowInstanceProvider.Verify(
            x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Never);
        Assert.Null(result.Data);
        Assert.Equal(
            $"Unable to find any questions for Question Screen activity Workflow Instance Id: {saveAndContinueCommand.WorkflowInstanceId} and Activity Id: {saveAndContinueCommand.ActivityId} in custom database",
            result.ErrorMessages.Single());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldReturnErrors_WhenADependencyThrows(
        [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
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
        workflowInstanceProvider.Verify(
            x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Never);
        Assert.Null(result.Data);
        Assert.Equal(exception.Message, result.ErrorMessages.Single());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldNotCallDependencies_GivenWorkflowStatusIsFinished(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
        [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
        [Frozen] Mock<IDeleteChangedWorkflowPathService> deleteChangedWorkflowPathService,
        WorkflowBlueprint workflowBlueprint,
        ActivityBlueprint activityBlueprint,
        List<QuestionScreenAnswer> currentAssessmentQuestions,
        WorkflowInstance workflowInstance,
        CustomActivityNavigation nextAssessmentActivity,
        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
        QuestionScreenSaveAndContinueCommandHandler sut
    )
    {
        //Arrange
        activityBlueprint.Id = workflowInstance.Output!.ActivityId;
        activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
        workflowBlueprint.Activities.Add(activityBlueprint);
        workflowInstance.WorkflowStatus = WorkflowStatus.Finished;

        nextAssessmentActivity.ActivityType = ActivityTypeConstants.QuestionScreen;

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(currentAssessmentQuestions);

        workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Never);

        nextActivityNavigationService.Verify(
            x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId,
                nextAssessmentActivity, activityBlueprint, workflowInstance, CancellationToken.None), Times.Never());

        deleteChangedWorkflowPathService.Verify(
            x => x.DeleteChangedWorkflowPath(saveAndContinueCommand.WorkflowInstanceId, saveAndContinueCommand.ActivityId,
                activityBlueprint, workflowInstance, CancellationToken.None), Times.Never());
        workflowInstanceProvider.Verify(
            x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Once);

        Assert.Null(result.Data);
        Assert.Equal(
            $"Unable to save answers. Workflow status is: {workflowInstance.WorkflowStatus}",
            result.ErrorMessages.Single());
        Assert.Null(result.ValidationMessages);
    }

    //[Theory]
    //[AutoMoqData]
    //public async Task Handle_ShouldAlwaysUpdateCustomDatabase_WhenWorkflowStatusIsNotFinished(
    //    [Frozen] Mock<IQuestionInvoker> questionInvoker,
    //    [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
    //    [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
    //    WorkflowBlueprint workflowBlueprint,
    //    ActivityBlueprint activityBlueprint,
    //    List<QuestionScreenAnswer> currentAssessmentQuestions,
    //    WorkflowInstance workflowInstance,
    //    CustomActivityNavigation nextAssessmentActivity,
    //    QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
    //    QuestionScreenSaveAndContinueCommandHandler sut
    //)
    //{
    //    //Arrange
    //    activityBlueprint.Id = workflowInstance.Output!.ActivityId;
    //    activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
    //    workflowBlueprint.Activities.Add(activityBlueprint);
    //    workflowInstance.WorkflowStatus = WorkflowStatus.Suspended;

    //    nextAssessmentActivity.ActivityType = ActivityTypeConstants.QuestionScreen;

    //    elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
    //            saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
    //        .ReturnsAsync(currentAssessmentQuestions);

    //    questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
    //            ActivityTypeConstants.QuestionScreen,
    //            saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
    //        .ReturnsAsync(new List<CollectedWorkflow>());

    //    workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
    //        .ReturnsAsync(workflowInstance);

    //    //Act
    //    await sut.Handle(saveAndContinueCommand, CancellationToken.None);

    //    //Assert
    //    elsaCustomRepository.Verify(
    //        x => x.SaveChanges(CancellationToken.None), Times.Once);
    //    workflowInstanceProvider.Verify(
    //        x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Exactly(2));

    //}

    //[Theory]
    //[AutoMoqData]
    //public async Task Handle_ShouldThrowException_WhenWorkflowStatusIsNotFinishedAndThereAreNoCollectedWorkflows(
    //    [Frozen] Mock<IQuestionInvoker> questionInvoker,
    //    [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
    //    [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
    //    WorkflowBlueprint workflowBlueprint,
    //    ActivityBlueprint activityBlueprint,
    //    List<QuestionScreenAnswer> currentAssessmentQuestions,
    //    WorkflowInstance workflowInstance,
    //    CustomActivityNavigation nextAssessmentActivity,
    //    QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
    //    List<QuestionScreenAnswer> nextAssessmentQuestions,
    //    QuestionScreenSaveAndContinueCommandHandler sut
    //)
    //{
    //    //Arrange
    //    activityBlueprint.Id = workflowInstance.Output!.ActivityId;
    //    activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
    //    workflowBlueprint.Activities.Add(activityBlueprint);
    //    workflowInstance.WorkflowStatus = WorkflowStatus.Suspended;

    //    nextAssessmentActivity.ActivityType = ActivityTypeConstants.QuestionScreen;

    //    elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
    //            saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
    //        .ReturnsAsync(currentAssessmentQuestions);

    //    questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
    //            ActivityTypeConstants.QuestionScreen,
    //            saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
    //        .ReturnsAsync(new List<CollectedWorkflow>());

    //    workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
    //        .ReturnsAsync(workflowInstance);

    //    //Act
    //    var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

    //    //Assert
    //    elsaCustomRepository.Verify(
    //        x => x.SaveChanges(CancellationToken.None), Times.Once);
    //    elsaCustomRepository.Verify(
    //        x => x.CreateCustomActivityNavigationAsync(nextAssessmentActivity, CancellationToken.None), Times.Never);
    //    elsaCustomRepository.Verify(
    //        x => x.CreateQuestionScreenAnswersAsync(nextAssessmentQuestions, CancellationToken.None), Times.Never);
    //    workflowInstanceProvider.Verify(
    //        x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Exactly(2));
    //    Assert.Null(result.Data);
    //    Assert.Equal(
    //        $"Unable to progress workflow. Workflow status is: {workflowInstance.WorkflowStatus}",
    //        result.ErrorMessages.Single());
    //    Assert.Null(result.ValidationMessages);
    //}

    //[Theory]
    //[AutoMoqData]
    //public async Task
    //    Handle_ShouldDeleteCustomActivityNavigationAndQuestionAnswers_WhenWorkflowPathChanged(
    //        [Frozen] Mock<IQuestionInvoker> questionInvoker,
    //        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
    //        [Frozen] Mock<IWorkflowPathProvider> workflowPathProvider,
    //        [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
    //        [Frozen] Mock<IWorkflowRegistryProvider> workflowNextActivityProvider,
    //        ActivityBlueprint activityBlueprint,
    //        List<QuestionScreenAnswer> currentAssessmentQuestions,
    //        WorkflowInstance workflowInstance,
    //        CustomActivityNavigation changedPathAssessmentActivity,
    //        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
    //        List<string> previousPathActivities,
    //        QuestionScreenSaveAndContinueCommandHandler sut)
    //{
    //    //Arrange
    //    var collectedWorkflow = new CollectedWorkflow(saveAndContinueCommand.WorkflowInstanceId, null, null);

    //    elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
    //            saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
    //        .ReturnsAsync(currentAssessmentQuestions);

    //    questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
    //            ActivityTypeConstants.QuestionScreen,
    //            saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
    //        .ReturnsAsync(new List<CollectedWorkflow>() { collectedWorkflow });

    //    workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
    //        .ReturnsAsync(workflowInstance);

    //    workflowNextActivityProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
    //        .ReturnsAsync(activityBlueprint);

    //    workflowPathProvider
    //        .Setup(x => x.GetChangedPathCustomNavigation(saveAndContinueCommand.WorkflowInstanceId,
    //            saveAndContinueCommand.ActivityId, activityBlueprint.Id, CancellationToken.None))
    //        .ReturnsAsync(changedPathAssessmentActivity);

    //    workflowPathProvider
    //        .Setup(x => x.GetPreviousPathActivities(workflowInstance.DefinitionId,
    //            changedPathAssessmentActivity.ActivityId, CancellationToken.None))
    //        .ReturnsAsync(previousPathActivities);

    //    //Act
    //    await sut.Handle(saveAndContinueCommand, CancellationToken.None);

    //    //Assert
    //    elsaCustomRepository.Verify(
    //        x => x.DeleteCustomNavigation(changedPathAssessmentActivity, CancellationToken.None), Times.Once);

    //    elsaCustomRepository.Verify(
    //        x => x.DeleteQuestionScreenAnswers(changedPathAssessmentActivity.WorkflowInstanceId, previousPathActivities, CancellationToken.None), Times.Once);
    //}
}