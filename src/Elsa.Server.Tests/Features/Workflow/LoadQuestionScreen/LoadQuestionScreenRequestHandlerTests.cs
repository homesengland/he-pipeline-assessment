﻿using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Common;
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
using Question = Elsa.CustomModels.Question;

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
        List<Question> assessmentQuestions,
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

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
        List<Question> assessmentQuestions,
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

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
            List<Question> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answers = new List<Answer> { new() { AnswerText = "Choice1" } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.CheckboxQuestion;
        elsaAssessmentQuestions.Questions[0].Checkbox.Choices = new List<CheckboxRecord>
        {
            new("A", "Choice1", false,false),
            new("B", "Choice2", false, false),
            new("C", "Choice3", false,false)
        };



        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
        Assert.Equal(1, result.Data.QuestionScreenAnswers[0].Checkbox.SelectedChoices.First());
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
            List<Question> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        var myChoice = "Choice1";
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answers = new List<Answer> { new() { AnswerText = "Choice1" } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.RadioQuestion;
        elsaAssessmentQuestions.Questions[0].Radio.Choices = new List<RadioRecord>
        {
            new("A", "Choice1",false),
            new("B", "Choice2",false),
            new("C", "Choice3",false)
        };



        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
        Assert.Equal(1, result.Data.QuestionScreenAnswers[0].Radio.SelectedAnswer);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithPotScoreChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<Question> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        var myChoice = "Choice1";
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answers = new List<Answer> { new() { AnswerText = myChoice } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.PotScoreRadioQuestion;
        elsaAssessmentQuestions.Questions[0].PotScoreRadio.Choices = new List<PotScoreRadioRecord>
        {
            new("A", "Choice1","1",false),
            new("B", "Choice2","1", false),
            new("C", "Choice3","1", false)
        };



        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
        Assert.Equal(1, result.Data.QuestionScreenAnswers[0].Radio.SelectedAnswer);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithPotScoreChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedAndNoErrorsEncountered(
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<Question> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        var myChoice = "Choice1";
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answers = new List<Answer> { new() { AnswerText = "" } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.PotScoreRadioQuestion;
        elsaAssessmentQuestions.Questions[0].PotScoreRadio.Choices = new List<PotScoreRadioRecord>
        {
            new("A", "Choice1","1",true),
            new("B", "Choice2","1", false),
            new("C", "Choice3","1", false)
        };

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
        Assert.Equal(1, result.Data.QuestionScreenAnswers[0].Radio.SelectedAnswer);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithRadioChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedAndNoErrorsEncountered(
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<Question> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        var myChoice = "Choice1";
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answers = new List<Answer> { new() { AnswerText = "" } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.RadioQuestion;
        elsaAssessmentQuestions.Questions[0].Radio.Choices = new List<RadioRecord>
        {
            new("A", "Choice1",true),
            new("B", "Choice2",false),
            new("C", "Choice3",false)
        };

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
        Assert.Equal(1, result.Data.QuestionScreenAnswers[0].Radio.SelectedAnswer);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithCheckboxChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedAndNoErrorsEncountered(
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<Question> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answers = new List<Answer> { new() { AnswerText = "" } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.CheckboxQuestion;
        elsaAssessmentQuestions.Questions[0].Checkbox.Choices = new List<CheckboxRecord>
        {
            new("A", "Choice1",false,true),
            new("B", "Choice2",false,true),
            new("C", "Choice3",false,false)
        };

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
        Assert.Equal(1, result.Data.QuestionScreenAnswers[0].Checkbox.SelectedChoices[0]);
        Assert.Equal(2, result.Data.QuestionScreenAnswers[0].Checkbox.SelectedChoices[1]);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithInformationObject_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<Question> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            TextModel textModel,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.Information;
        elsaAssessmentQuestions.Questions[0].Text = textModel;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
        var textRecord = textModel.TextRecords.First();
        var actualText = result.Data.QuestionScreenAnswers[0].Information.InformationTextList.First();
        Assert.Equal(textRecord.Text, actualText.Text);
        Assert.Equal(textRecord.IsHyperlink, actualText.IsHyperlink);
        Assert.Equal(textRecord.IsGuidance, actualText.IsGuidance);
        Assert.Equal(textRecord.IsParagraph, actualText.IsParagraph);
        Assert.Equal(textRecord.Url, actualText.Url);
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
            List<Question> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answers = null;
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

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
        Assert.Equal("PrepopulatedAnswer", result.Data.QuestionScreenAnswers[0].Answers.FirstOrDefault().AnswerText);
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
            List<Question> assessmentQuestions,
            AssessmentQuestions elsaAssessmentQuestions,
            LoadQuestionScreenRequestHandler sut)
    {
        //Arrange
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answers = new List<Answer> { new() { AnswerText = "DatabaseAnswer" } };
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

        elsaCustomRepository.Setup(x => x.GetQuestions(loadWorkflowActivityRequest.ActivityId,
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
        Assert.Equal("DatabaseAnswer", result.Data.QuestionScreenAnswers[0].Answers.FirstOrDefault().AnswerText);
        Assert.False(result.Data.QuestionScreenAnswers[0].IsReadOnly);
    }


}