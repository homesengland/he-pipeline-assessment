﻿using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.Workflow.LoadQuestionScreen;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;
using QuestionActivityData = Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen.QuestionActivityData;

namespace Elsa.Server.Tests.Features.Workflow.LoadWorkflowActivity;

public class LoadQuestionScreenRequestHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenActivityIdCannotBeFoundInActivityDictionary(
        [Frozen] Mock<IQuestionInvoker> questionInvoker,
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<ILoadQuestionScreenJsonHelper> loadQuestionScreenJsonHelper,
        LoadQuestionScreenRequest loadQuestionScreenRequest,
        List<CollectedWorkflow> collectedWorkflows,
        WorkflowInstance workflowInstance,
        CustomActivityNavigation customActivityNavigation,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        questionInvoker
            .Setup(x => x.FindWorkflowsAsync(loadQuestionScreenRequest.ActivityId,
                customActivityNavigation.ActivityType, loadQuestionScreenRequest.WorkflowInstanceId,
                CancellationToken.None))
            .ReturnsAsync(collectedWorkflows);

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadQuestionScreenRequest.ActivityId,
                loadQuestionScreenRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        //Act
        var result = await sut.Handle(loadQuestionScreenRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.MultiQuestionActivityData);
        Assert.Equal(
            $"Cannot find activity Id {loadQuestionScreenRequest.ActivityId} in the workflow activity data dictionary",
            result.ErrorMessages.Single());
        workflowInstanceStore.Verify(
            x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
        elsaCustomRepository.Verify(
            x => x.GetCustomActivityNavigation(loadQuestionScreenRequest.ActivityId,
                loadQuestionScreenRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
        loadQuestionScreenJsonHelper.Verify(
            x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(
                It.IsAny<IDictionary<string, object?>>()), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenAssessmentQuestionDoesNotExist(
        [Frozen] Mock<IQuestionInvoker> questionInvoker,
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<ILoadQuestionScreenJsonHelper> loadQuestionScreenJsonHelper,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        List<CollectedWorkflow> collectedWorkflows,
        WorkflowInstance workflowInstance,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        questionInvoker
            .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                It.IsAny<string>(), loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(collectedWorkflows);

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync((CustomActivityNavigation?)null);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.MultiQuestionActivityData);
        Assert.Equal(
            $"Unable to find workflow instance with Id: {loadWorkflowActivityRequest.WorkflowInstanceId} and Activity Id: {loadWorkflowActivityRequest.ActivityId} in Pipeline Assessment database",
            result.ErrorMessages.Single());
        workflowInstanceStore.Verify(
            x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Never);
        loadQuestionScreenJsonHelper.Verify(
            x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(
                It.IsAny<IDictionary<string, object?>>()), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenCollectedWorkflowInstanceIsNull(
        [Frozen] Mock<IQuestionInvoker> questionInvoker,
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<ILoadQuestionScreenJsonHelper> loadQuestionScreenJsonHelper,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        CustomActivityNavigation customActivityNavigation,
        List<CollectedWorkflow> collectedWorkflows,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        questionInvoker
            .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                customActivityNavigation.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
                CancellationToken.None))
            .ReturnsAsync(collectedWorkflows);

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync((WorkflowInstance?)null);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.MultiQuestionActivityData);
        Assert.Equal(
            $"Unable to find workflow instance with Id: {loadWorkflowActivityRequest.WorkflowInstanceId} in Elsa database",
            result.ErrorMessages.Single());
        workflowInstanceStore.Verify(
            x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
        loadQuestionScreenJsonHelper.Verify(
            x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(
                It.IsAny<IDictionary<string, object?>>()), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenNoWorkflowsAreCollected(
        [Frozen] Mock<IQuestionInvoker> questionInvoker,
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<ILoadQuestionScreenJsonHelper> loadQuestionScreenJsonHelper,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        CustomActivityNavigation customActivityNavigation,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        questionInvoker
            .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                customActivityNavigation.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
                CancellationToken.None))
            .ReturnsAsync(new List<CollectedWorkflow>());

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.MultiQuestionActivityData);
        Assert.Equal(
            $"Unable to progress workflow instance Id {loadWorkflowActivityRequest.WorkflowInstanceId}. No collected workflows",
            result.ErrorMessages.Single());
        workflowInstanceStore.Verify(
            x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Never);
        loadQuestionScreenJsonHelper.Verify(
            x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(
                It.IsAny<IDictionary<string, object?>>()), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenExceptionIsThrown(
        [Frozen] Mock<IQuestionInvoker> questionInvoker,
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<ILoadQuestionScreenJsonHelper> loadQuestionScreenJsonHelper,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        CustomActivityNavigation customActivityNavigation,
        Exception exception,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        questionInvoker
            .Setup(x => x.FindWorkflowsAsync(It.IsAny<string>(), customActivityNavigation.ActivityType,
                It.IsAny<string>(), CancellationToken.None))
            .Throws(exception);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.MultiQuestionActivityData);
        Assert.Equal(exception.Message, result.ErrorMessages.Single());
        workflowInstanceStore.Verify(
            x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Never);
        loadQuestionScreenJsonHelper.Verify(
            x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(
                It.IsAny<IDictionary<string, object?>>()), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsErrors_GivenActiovityDataReturnsNullForAcitivtyId(
        [Frozen] Mock<IQuestionInvoker> questionInvoker,
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<ILoadQuestionScreenJsonHelper> loadQuestionScreenJsonHelper,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        List<CollectedWorkflow> collectedWorkflows,
        WorkflowInstance workflowInstance,
        CustomActivityNavigation customActivityNavigation,
        QuestionActivityData questionActivityData,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        questionInvoker
            .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                customActivityNavigation.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
                CancellationToken.None))
            .ReturnsAsync(collectedWorkflows);

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        loadQuestionScreenJsonHelper
            .Setup(x =>
                x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(
                    It.IsAny<IDictionary<string, object?>>()))
            .Returns(questionActivityData);

        IDictionary<string, object?>? customDictionary = null;
        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, customDictionary!);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.MultiQuestionActivityData);
        Assert.Equal($"Activity data is null for Activity Id: {loadWorkflowActivityRequest.ActivityId}",
            result.ErrorMessages.Single());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnErrors_GivenActivityIsQuestionScreenAndAssessmentQuestionsAreNull(
        [Frozen] Mock<IQuestionInvoker> questionInvoker,
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<ILoadQuestionScreenJsonHelper> loadQuestionScreenJsonHelper,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        List<CollectedWorkflow> collectedWorkflows,
        WorkflowInstance workflowInstance,
        CustomActivityNavigation customActivityNavigation,
        List<QuestionScreenAnswer> assessmentQuestions,
        QuestionActivityData questionActivityData,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;

        questionInvoker
            .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                customActivityNavigation.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
                CancellationToken.None))
            .ReturnsAsync(collectedWorkflows);

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        loadQuestionScreenJsonHelper
            .Setup(x =>
                x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(
                    It.IsAny<IDictionary<string, object?>>()))
            .Returns(questionActivityData);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        AssessmentQuestions? elsaAssessmentQuestions = null;
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.MultiQuestionActivityData);
        Assert.Equal("Failed to map activity data to MultiQuestionActivityData", result.ErrorMessages.Single());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnMultiQuestionActivityData_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
        [Frozen] Mock<IQuestionInvoker> questionInvoker,
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<ILoadQuestionScreenJsonHelper> loadQuestionScreenJsonHelper,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        List<CollectedWorkflow> collectedWorkflows,
        WorkflowInstance workflowInstance,
        CustomActivityNavigation customActivityNavigation,
        List<QuestionScreenAnswer> assessmentQuestions,
        QuestionActivityData questionActivityData,
        AssessmentQuestions elsaAssessmentQuestions,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;

        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        questionInvoker
            .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                customActivityNavigation.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
                CancellationToken.None))
            .ReturnsAsync(collectedWorkflows);

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        loadQuestionScreenJsonHelper
            .Setup(x =>
                x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(
                    It.IsAny<IDictionary<string, object?>>()))
            .Returns(questionActivityData);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.MultiQuestionActivityData);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.MultiQuestionActivityData.Count());
        Assert.Empty(result.ErrorMessages);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithMultiChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<ILoadQuestionScreenJsonHelper> loadQuestionScreenJsonHelper,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<QuestionScreenAnswer> assessmentQuestions,
            QuestionActivityData questionActivityData,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        var myChoice = @"[""Choice1""]";
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answer = myChoice;
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.CheckboxQuestion;
        elsaAssessmentQuestions.Questions[0].Checkbox.Choices = new List<CheckboxRecord>
        {
            new("Choice1", false),
            new("Choice2", false),
            new("Choice3", false)
        };

        questionInvoker
            .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                customActivityNavigation.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
                CancellationToken.None))
            .ReturnsAsync(collectedWorkflows);

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        loadQuestionScreenJsonHelper
            .Setup(x =>
                x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(
                    It.IsAny<IDictionary<string, object?>>()))
            .Returns(questionActivityData);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.MultiQuestionActivityData);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.MultiQuestionActivityData.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal("Choice1", result.Data.MultiQuestionActivityData[0].Checkbox.SelectedChoices.First());
        Assert.Single(result.Data.MultiQuestionActivityData[0].Checkbox.SelectedChoices);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithSingleChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<ILoadQuestionScreenJsonHelper> loadQuestionScreenJsonHelper,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<QuestionScreenAnswer> assessmentQuestions,
            QuestionActivityData questionActivityData,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        var myChoice = "Choice1";
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answer = "Choice1";
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.RadioQuestion;
        elsaAssessmentQuestions.Questions[0].Radio.Choices = new List<RadioRecord>
        {
            new("Choice1"),
            new("Choice2"),
            new("Choice3")
        };

        questionInvoker
            .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                customActivityNavigation.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
                CancellationToken.None))
            .ReturnsAsync(collectedWorkflows);

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        loadQuestionScreenJsonHelper
            .Setup(x =>
                x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(
                    It.IsAny<IDictionary<string, object?>>()))
            .Returns(questionActivityData);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.MultiQuestionActivityData);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.MultiQuestionActivityData.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(result.Data.MultiQuestionActivityData[0].Radio.SelectedAnswer, myChoice);
    }
}