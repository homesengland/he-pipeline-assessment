using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.Workflow.LoadQuestionScreen;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.LoadQuestionScreen;

public class LoadQuestionScreenRequestHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenActivityIdCannotBeFoundInActivityDictionary(
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadQuestionScreenRequest loadQuestionScreenRequest,
        WorkflowInstance workflowInstance,
        CustomActivityNavigation customActivityNavigation,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;



        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadQuestionScreenRequest.ActivityId,
                loadQuestionScreenRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        //Act
        var result = await sut.Handle(loadQuestionScreenRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.QuestionScreenAnswers);
        Assert.Equal(
            $"Cannot find activity Id {loadQuestionScreenRequest.ActivityId} in the workflow activity data dictionary",
            result.ErrorMessages.Single());
        workflowInstanceStore.Verify(
            x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
        elsaCustomRepository.Verify(
            x => x.GetCustomActivityNavigation(loadQuestionScreenRequest.ActivityId,
                loadQuestionScreenRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenCustomActivityNavigationDoesNotExist(
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        WorkflowInstance workflowInstance,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange


        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync((CustomActivityNavigation?)null);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.QuestionScreenAnswers);
        Assert.Equal(
            $"Unable to find activity navigation with Workflow Id: {loadWorkflowActivityRequest.WorkflowInstanceId} and Activity Id: {loadWorkflowActivityRequest.ActivityId} in Elsa Custom database",
            result.ErrorMessages.Single());
        workflowInstanceStore.Verify(
            x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenCollectedWorkflowInstanceIsNull(
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        CustomActivityNavigation customActivityNavigation,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;



        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync((WorkflowInstance?)null);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.QuestionScreenAnswers);
        Assert.Equal(
            $"Unable to find workflow instance with Id: {loadWorkflowActivityRequest.WorkflowInstanceId} in Elsa database",
            result.ErrorMessages.Single());
        workflowInstanceStore.Verify(
            x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
    }


    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenExceptionIsThrown(
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        CustomActivityNavigation customActivityNavigation,
        Exception exception,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .Throws(exception);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.QuestionScreenAnswers);
        Assert.Equal(exception.Message, result.ErrorMessages.Single());
        workflowInstanceStore.Verify(
            x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnErrors_GivenActivityIsQuestionScreenAndAssessmentQuestionsAreNull(
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        WorkflowInstance workflowInstance,
        CustomActivityNavigation customActivityNavigation,
        List<QuestionScreenAnswer> assessmentQuestions,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;



        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        AssessmentQuestions? elsaAssessmentQuestions = null;
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.QuestionScreenAnswers);
        Assert.Equal("Failed to map activity data to QuestionScreenAnswers", result.ErrorMessages.Single());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnErrors_GivenActivityTypeIsNotQuestionScreen(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        CustomActivityNavigation customActivityNavigation,
        LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.CheckYourAnswersScreen;

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.QuestionScreenAnswers);
        Assert.Equal($"Attempted to load question screen with {customActivityNavigation.ActivityType} activity type", result.ErrorMessages.Single());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnMultiQuestionActivityData_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
        [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadQuestionScreenRequest loadWorkflowActivityRequest,
        WorkflowInstance workflowInstance,
        CustomActivityNavigation customActivityNavigation,
        List<QuestionScreenAnswer> assessmentQuestions,
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



        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.QuestionScreenAnswers);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.QuestionScreenAnswers.Count());
        Assert.Empty(result.ErrorMessages);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithMultiChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<QuestionScreenAnswer> assessmentQuestions,
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
            new("A", "Choice1", false),
            new("B", "Choice2", false),
            new("C", "Choice3", false)
        };



        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.QuestionScreenAnswers);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.QuestionScreenAnswers.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal("Choice1", result.Data.QuestionScreenAnswers[0].Checkbox.SelectedChoices.First());
        Assert.Single(result.Data.QuestionScreenAnswers[0].Checkbox.SelectedChoices);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithSingleChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<QuestionScreenAnswer> assessmentQuestions,
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
            new("A", "Choice1"),
            new("B", "Choice2"),
            new("C", "Choice3")
        };



        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.QuestionScreenAnswers);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.QuestionScreenAnswers.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(result.Data.QuestionScreenAnswers[0].Radio.SelectedAnswer, myChoice);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithPrepopulatedAnswers_GivenOnlyPrepopulatedAnswerAreProvided(
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<QuestionScreenAnswer> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answer = null;
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].Answer = "PrepopulatedAnswer";
        elsaAssessmentQuestions.Questions[0].IsReadOnly = true;
        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.TextAreaQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.QuestionScreenAnswers);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.QuestionScreenAnswers.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal("PrepopulatedAnswer", result.Data.QuestionScreenAnswers[0].Answer);
        Assert.True(result.Data.QuestionScreenAnswers[0].IsReadOnly);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithDatabaseAnswers_GivenBothDatabaseAndPrepopulatedAnswerAreProvided(
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<QuestionScreenAnswer> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answer = "DatabaseAnswer";
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].Answer = "PrepopulatedAnswer";
        elsaAssessmentQuestions.Questions[0].IsReadOnly = false;
        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.TextAreaQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.QuestionScreenAnswers);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.QuestionScreenAnswers.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal("DatabaseAnswer", result.Data.QuestionScreenAnswers[0].Answer);
        Assert.False(result.Data.QuestionScreenAnswers[0].IsReadOnly);
    }
}