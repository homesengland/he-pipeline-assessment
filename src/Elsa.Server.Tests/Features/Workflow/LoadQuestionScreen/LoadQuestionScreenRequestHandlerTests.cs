using AutoFixture.Xunit2;
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
using System.Text.Json;

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
        Assert.Null(result.Data!.Questions);
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
        Assert.Null(result.Data!.Questions);
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
        Assert.Null(result.Data!.Questions);
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
        Assert.Null(result.Data!.Questions);
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

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        AssessmentQuestions? elsaAssessmentQuestions = null;
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.Questions);
        Assert.Equal("Failed to map activity data to Questions", result.ErrorMessages.Single());
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
        Assert.Null(result.Data!.Questions);
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

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithMultiChoiceOptionsCorrectlySelectedFromDatabase_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
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
        assessmentQuestions[0].Answers = new List<Answer> {
            new() { AnswerText = "", Choice = new QuestionChoice { Id = 1, Identifier = "A" } } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers = false!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.CheckboxQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(1, result.Data.Questions[0].Checkbox.SelectedChoices.First());
        Assert.Single(result.Data.Questions[0].Checkbox.SelectedChoices);
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
        assessmentQuestions[0].Answers = new List<Answer> {
            new() { AnswerText = myChoice, Choice = new QuestionChoice { Id = 1, Identifier = "A" } } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers = false!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.RadioQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(1, result.Data.Questions[0].Radio.SelectedAnswer);
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
        assessmentQuestions[0].Answers = new List<Answer> {
            new() { AnswerText = myChoice, Choice = new QuestionChoice { Id = 1, Identifier = "A" } } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers = false!;
        }

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.PotScoreRadioQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(1, result.Data.Questions[0].Radio.SelectedAnswer);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithPotScoreChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedAndNoDatabaseAnswerExistsAndNoErrorsEncountered(
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
        assessmentQuestions[0].Answers = new List<Answer>();
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].PotScoreRadio.Choices = new List<PotScoreRadioRecord>()
        {
            new PotScoreRadioRecord("A", "Answer A","High", true),
            new PotScoreRadioRecord("B", "Answer B","Medium", false),
        };
        }

        assessmentQuestions[0].Choices = new List<QuestionChoice>()
        {
            new QuestionChoice { Id = 1, Answer = "Choice1", IsPrePopulated = false, Identifier = "A"},
            new QuestionChoice { Id = 2, Answer = "Choice2", IsPrePopulated = false, Identifier = "B" }
        };

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.PotScoreRadioQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(1, result.Data.Questions[0].Radio.SelectedAnswer);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithPotScoreChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedDatabaseAnswerExistsAndReevaluatePrepoluatedSelectedAndNoErrorsEncountered(
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
        assessmentQuestions[0].Answers = new List<Answer>(){
            new() { AnswerText = "", Choice = new QuestionChoice { Id = 1, Identifier = "A" } } }; ;
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers= true;
            elsaAssessmentQuestions.Questions[i].PotScoreRadio.Choices = new List<PotScoreRadioRecord>()
        {
            new PotScoreRadioRecord("A", "Answer A","High", false),
            new PotScoreRadioRecord("B", "Answer B","Medium", true),
        };
        }

        assessmentQuestions[0].Choices = new List<QuestionChoice>()
        {
            new QuestionChoice { Id = 1, Answer = "Choice1", IsPrePopulated = false, Identifier = "A"},
            new QuestionChoice { Id = 2, Answer = "Choice2", IsPrePopulated = false, Identifier = "B" }
        };

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.PotScoreRadioQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(2, result.Data.Questions[0].Radio.SelectedAnswer);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithWeighedRadioChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedAndNoDatabaseAnswerExistsAndNoErrorsEncountered(
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
        assessmentQuestions[0].Answers = new List<Answer>();
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].WeightedRadio.Choices = new List<WeightedRadioRecord>()
        {
            new WeightedRadioRecord("A", "Answer A",new decimal(1.0), true),
            new WeightedRadioRecord("B", "Answer B",new decimal(1.0), false),
        };
        }

        assessmentQuestions[0].Choices = new List<QuestionChoice>()
        {
            new QuestionChoice { Id = 1, Answer = "Choice1", IsPrePopulated = false, Identifier = "A"},
            new QuestionChoice { Id = 2, Answer = "Choice2", IsPrePopulated = false, Identifier = "B" }
        };

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.WeightedRadioQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(1, result.Data.Questions[0].Radio.SelectedAnswer);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithWeighedRadioChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedDatabaseAnswerExistsAndReevaluatePrepoluatedSelectedAndNoErrorsEncountered(
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
        assessmentQuestions[0].Answers = new List<Answer>(){
            new() { AnswerText = "", Choice = new QuestionChoice { Id = 1, Identifier = "A" } } }; ;
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers = true;
            elsaAssessmentQuestions.Questions[i].WeightedRadio.Choices = new List<WeightedRadioRecord>()
        {
            new WeightedRadioRecord("A", "Answer A",new decimal(1.0), false),
            new WeightedRadioRecord("B", "Answer B",new decimal(1.0), true),
        };
        }

        assessmentQuestions[0].Choices = new List<QuestionChoice>()
        {
            new QuestionChoice { Id = 1, Answer = "Choice1", IsPrePopulated = false, Identifier = "A"},
            new QuestionChoice { Id = 2, Answer = "Choice2", IsPrePopulated = false, Identifier = "B" }
        };

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.WeightedRadioQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(2, result.Data.Questions[0].Radio.SelectedAnswer);
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
        customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
        assessmentQuestions[0].Answers = new List<Answer>();
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].Radio.Choices = new List<RadioRecord>()
        {
            new RadioRecord("A", "Answer A", true),
            new RadioRecord("B", "Answer B",false),
        };
        }

        assessmentQuestions[0].Choices = new List<QuestionChoice>()
        {
            new QuestionChoice { Id = 1, Answer = "Choice1", IsPrePopulated = false , Identifier="A"},
            new QuestionChoice { Id = 2, Answer = "Choice2", IsPrePopulated = false , Identifier="B"}
        };

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.RadioQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(1, result.Data.Questions[0].Radio.SelectedAnswer);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithRadioChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedDatabaseAnswerExistsAndReevaluatePrepopulatedSelectedAndNoErrorsEncountered(
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
        assessmentQuestions[0].Answers =  new List<Answer> {
            new() { AnswerText = "", Choice = new QuestionChoice { Id = 1, Identifier = "A" } } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers= true;
            elsaAssessmentQuestions.Questions[i].Radio.Choices = new List<RadioRecord>()
        {
            new RadioRecord("A", "Answer A", false),
            new RadioRecord("B", "Answer B",true),
        };
        }

        assessmentQuestions[0].Choices = new List<QuestionChoice>()
        {
            new QuestionChoice { Id = 1, Answer = "Choice1", IsPrePopulated = false , Identifier = "A"},
            new QuestionChoice { Id = 2, Answer = "Choice2", IsPrePopulated = false, Identifier = "B" }
        };

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.RadioQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(2, result.Data.Questions[0].Radio.SelectedAnswer);
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
        assessmentQuestions[0].Answers = new List<Answer>();
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].Checkbox.Choices = new List<CheckboxRecord>()
        {
            new CheckboxRecord("A", "Answer A", false,true),
            new CheckboxRecord("B", "Answer B",false,true),
            new CheckboxRecord("C", "Answer C",false,false),
        };
        }

        assessmentQuestions[0].Choices = new List<QuestionChoice>()
        {
            new QuestionChoice { Id = 1, Answer = "Choice1", IsPrePopulated = false , Identifier= "A"},
            new QuestionChoice { Id = 2, Answer = "Choice2", IsPrePopulated = false , Identifier="B"},
            new QuestionChoice { Id = 3, Answer = "Choice3", IsPrePopulated = false , Identifier="C"}
        };

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.CheckboxQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count, result.Data!.Questions.Count);
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(2, result.Data.Questions[0].Checkbox.SelectedChoices.Count);
        Assert.Equal(1, result.Data.Questions[0].Checkbox.SelectedChoices[0]);
        Assert.Equal(2, result.Data.Questions[0].Checkbox.SelectedChoices[1]);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithCheckboxChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedDatabaseAnswerExistsandReevaluatePrepopulatedSelectedAndNoErrorsEncountered(
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
        assessmentQuestions[0].Answers = new List<Answer> {
            new() { AnswerText = "", Choice = new QuestionChoice { Id = 1, Identifier = "A" } } };
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers = true;
            elsaAssessmentQuestions.Questions[i].Checkbox.Choices = new List<CheckboxRecord>()
        {
            new CheckboxRecord("A", "Answer A", false,true),
            new CheckboxRecord("B", "Answer B",false,true),
            new CheckboxRecord("C", "Answer C",false,false),
        };
        }

        assessmentQuestions[0].Choices = new List<QuestionChoice>()
        {
            new QuestionChoice { Id = 1, Answer = "Choice1", IsPrePopulated = false, Identifier="A" },
            new QuestionChoice { Id = 2, Answer = "Choice2", IsPrePopulated = false, Identifier="B" },
            new QuestionChoice { Id = 3, Answer = "Choice3", IsPrePopulated = false, Identifier="C" }
        };

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.CheckboxQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count, result.Data!.Questions.Count);
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(2, result.Data.Questions[0].Checkbox.SelectedChoices.Count);
        Assert.Equal(1, result.Data.Questions[0].Checkbox.SelectedChoices[0]);
        Assert.Equal(2, result.Data.Questions[0].Checkbox.SelectedChoices[1]);
    }

    [Theory]
    [AutoMoqData]
    public async Task
       Handle_ReturnMultiQuestionActivityDataWithWeighedCheckboxChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedNoDatabaseAnswerExistsAndNoErrorsEncountered(
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
        assessmentQuestions[0].Answers = new List<Answer>(){
            new() { AnswerText = "", Choice = new QuestionChoice { Id = 1, Identifier = "A" } } }; 
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
        }

        var elsaGroups = new Dictionary<string, WeightedCheckboxGroup>();
        elsaGroups.Add("1", new WeightedCheckboxGroup()
        {
            GroupIdentifier = "A",
            Choices = new List<WeightedCheckboxRecord>{
                new WeightedCheckboxRecord("A", "Choice 1", false, new decimal(1.0), true, false),
                new WeightedCheckboxRecord("B", "Choice 2", false, new decimal(1.0), false, false),
            }

        });
        elsaGroups.Add("2", new WeightedCheckboxGroup()
        {
            GroupIdentifier = "B",
            Choices = new List<WeightedCheckboxRecord>{
                new WeightedCheckboxRecord("A", "Choice 1", false, new decimal(1.0), true, false),
                new WeightedCheckboxRecord("B", "Choice 2", false, new decimal(1.0), false, false),
            }

        });
        elsaAssessmentQuestions.Questions[0].WeightedCheckbox.Groups = elsaGroups;

        assessmentQuestions[0].Choices = new List<QuestionChoice>()
        {
            new QuestionChoice { Id = 1, Answer = "Choice1", IsPrePopulated = false , Identifier= "A", QuestionChoiceGroup = new QuestionChoiceGroup{GroupIdentifier= "A", Id=1} , QuestionChoiceGroupId =1},
            new QuestionChoice { Id = 2, Answer = "Choice2", IsPrePopulated = false , Identifier="B", QuestionChoiceGroup = new QuestionChoiceGroup{GroupIdentifier= "A", Id=1}, QuestionChoiceGroupId =1},
            new QuestionChoice { Id = 3, Answer = "Choice1", IsPrePopulated = false , Identifier= "A", QuestionChoiceGroup = new QuestionChoiceGroup{GroupIdentifier= "B", Id=2} , QuestionChoiceGroupId =2},
            new QuestionChoice { Id = 4, Answer = "Choice2", IsPrePopulated = false , Identifier="B", QuestionChoiceGroup = new QuestionChoiceGroup{GroupIdentifier= "B", Id=2}, QuestionChoiceGroupId =2},
        };

        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count, result.Data!.Questions.Count);
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(2, result.Data.Questions[0].Checkbox.SelectedChoices.Count);
        Assert.Equal(1, result.Data.Questions[0].Checkbox.SelectedChoices[0]);
        Assert.Equal(3, result.Data.Questions[0].Checkbox.SelectedChoices[1]);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ReturnMultiQuestionActivityDataWithWeighedCheckboxChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenPrepopulatedIsSelectedDatabaseAnswerExistsandReevaluatePrepopulatedSelectedAndNoErrorsEncountered(
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
        assessmentQuestions[0].Answers = new List<Answer>(){
            new() { AnswerText = "", Choice = new QuestionChoice { Id = 1, Identifier = "A" } } }; 
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers = true;
        }

        var elsaGroups = new Dictionary<string, WeightedCheckboxGroup>();
        elsaGroups.Add("1", new WeightedCheckboxGroup()
        {
            GroupIdentifier = "A",
            Choices = new List<WeightedCheckboxRecord>{
                new WeightedCheckboxRecord("A", "Choice 1", false, new decimal(1.0), true, false),
                new WeightedCheckboxRecord("B", "Choice 2", false, new decimal(1.0), false, false),
            }

        });
        elsaGroups.Add("2", new WeightedCheckboxGroup()
        {
            GroupIdentifier = "B",
            Choices = new List<WeightedCheckboxRecord>{
                new WeightedCheckboxRecord("A", "Choice 1", false, new decimal(1.0), true, false),
                new WeightedCheckboxRecord("B", "Choice 2", false, new decimal(1.0), false, false),
            }

        });
        elsaAssessmentQuestions.Questions[0].WeightedCheckbox.Groups = elsaGroups;

        assessmentQuestions[0].Choices = new List<QuestionChoice>()
        {
            new QuestionChoice { Id = 1, Answer = "Choice1", IsPrePopulated = false , Identifier= "A", QuestionChoiceGroup = new QuestionChoiceGroup{GroupIdentifier= "A", Id=1} , QuestionChoiceGroupId =1},
            new QuestionChoice { Id = 2, Answer = "Choice2", IsPrePopulated = false , Identifier="B", QuestionChoiceGroup = new QuestionChoiceGroup{GroupIdentifier= "A", Id=1}, QuestionChoiceGroupId =1},
            new QuestionChoice { Id = 3, Answer = "Choice1", IsPrePopulated = false , Identifier= "A", QuestionChoiceGroup = new QuestionChoiceGroup{GroupIdentifier= "B", Id=2} , QuestionChoiceGroupId =2},
            new QuestionChoice { Id = 4, Answer = "Choice2", IsPrePopulated = false , Identifier="B", QuestionChoiceGroup = new QuestionChoiceGroup{GroupIdentifier= "B", Id=2}, QuestionChoiceGroupId =2},
        };
        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.WeightedCheckboxQuestion;

        workflowInstanceStore.Setup(x =>
                x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count, result.Data!.Questions.Count);
        Assert.Empty(result.ErrorMessages);
        Assert.Equal(2, result.Data.Questions[0].Checkbox.SelectedChoices.Count);
        Assert.Equal(1, result.Data.Questions[0].Checkbox.SelectedChoices[0]);
        Assert.Equal(3, result.Data.Questions[0].Checkbox.SelectedChoices[1]);
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

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        var textRecord = textModel.TextRecords.First();
        var actualText = result.Data.Questions[0].Information.InformationTextList.First();
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
        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            assessmentQuestions[i].Answers = new List<Answer>();
        }

        elsaAssessmentQuestions.Questions[0].Answer = "PrepopulatedAnswer";
        elsaAssessmentQuestions.Questions[0].IsReadOnly = true;
        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.TextAreaQuestion;

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        workflowInstanceStore.Setup(x =>
        x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
    .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal("PrepopulatedAnswer", result.Data.Questions[0].Answers.FirstOrDefault()!.AnswerText);
        Assert.True(result.Data.Questions[0].IsReadOnly);
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
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers = false!;
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

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal("DatabaseAnswer", result.Data.Questions[0].Answers.FirstOrDefault()?.AnswerText);
        Assert.False(result.Data.Questions[0].IsReadOnly);
    }

    [Theory]
    [AutoMoqData]
    public async Task
       Handle_ReturnMultiQuestionActivityDataWithDatabaseAnswers_GivenBothDatabaseAndPrepopulatedAnswerAreProvidedAndReevaluatePrepopulatedSelected(
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
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers = true!;
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

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal("PrepopulatedAnswer", result.Data.Questions[0].Answers.FirstOrDefault()?.AnswerText);
        Assert.False(result.Data.Questions[0].IsReadOnly);
    }

    [Theory]
    [AutoMoqData]
    public async Task
    Handle_ReturnMultiQuestionActivityDataWithDataTableAnswers_GivenOnlyPrepopulatedTableAnswerAreProvided(
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
            assessmentQuestions[i].Answers = new List<Answer>();
        }

        var dataTableAnswer = new DataTable()
        {

            InputType = "Integer",
            Inputs = new List<TableInput>(){
            new TableInput("1", "TableInput1", false, false, "Prepopulated Answer 1"),
            new TableInput("2", "TableInput1", false, false, "Prepopulated Answer 2"),
        }
        };

        elsaAssessmentQuestions.Questions[0].Answer = JsonSerializer.Serialize(dataTableAnswer);
        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.DataTable;

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        workflowInstanceStore.Setup(x =>
        x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
    .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);
        var answerResult = JsonSerializer.Deserialize<DataTable>(result.Data.Questions[0].Answers.FirstOrDefault().AnswerText);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal("Prepopulated Answer 1", answerResult.Inputs[0].Input);
        Assert.Equal("Prepopulated Answer 2", answerResult.Inputs[1].Input);
    }

    [Theory]
    [AutoMoqData]
    public async Task
    Handle_ReturnMultiQuestionActivityDataWithDataTableAnswers_GivenPrepopulatedAndDatabaseTableAnswerAreProvided(
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

        var databaseDataTableAnswer = new DataTable()
        {

            InputType = "Integer",
            Inputs = new List<TableInput>(){
            new TableInput("1", "TableInput1", false, false, "Database Answer 1"),
            new TableInput("2", "TableInput1", false, false, "Database Answer 2"),
        }
        };

        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            assessmentQuestions[i].Answers = new List<Answer> { new() { AnswerText = JsonSerializer.Serialize(databaseDataTableAnswer) } };
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers = false;
        }

        var dataTableAnswer = new DataTable()
        {

            InputType = "Integer",
            Inputs = new List<TableInput>(){
            new TableInput("1", "TableInput1", false, false, "Prepopulated Answer 1"),
            new TableInput("2", "TableInput1", false, false, "Prepopulated Answer 2"),
        }
        };

        elsaAssessmentQuestions.Questions[0].Answer = JsonSerializer.Serialize(dataTableAnswer);
        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.DataTable;

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        workflowInstanceStore.Setup(x =>
        x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
    .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);
        var answerResult = JsonSerializer.Deserialize<DataTable>(result.Data.Questions[0].Answers.FirstOrDefault().AnswerText);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal("Database Answer 1", answerResult.Inputs[0].Input);
        Assert.Equal("Database Answer 2", answerResult.Inputs[1].Input);
    }

    [Theory]
    [AutoMoqData]
    public async Task
Handle_ReturnMultiQuestionActivityDataWithDataTableAnswers_GivenPrepopulatedAndDatabaseTableAnswerAreProvidedAndReevelatedPrepopulatedSelected(
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

        var databaseDataTableAnswer = new DataTable()
        {

            InputType = "Integer",
            Inputs = new List<TableInput>(){
            new TableInput("1", "TableInput1", false, false, "Database Answer 1"),
            new TableInput("2", "TableInput1", false, false, "Database Answer 2"),
        }
        };

        for (var i = 0; i < assessmentQuestions.Count; i++)
        {
            var questionId = assessmentQuestions[i].QuestionId;
            elsaAssessmentQuestions.Questions[i].Id = questionId!;
            assessmentQuestions[i].Answers = new List<Answer> { new() { AnswerText = JsonSerializer.Serialize(databaseDataTableAnswer) } };
            elsaAssessmentQuestions.Questions[i].ReevaluatePrePopulatedAnswers = true;
        }

        var dataTableAnswer = new DataTable()
        {

            InputType = "Integer",
            Inputs = new List<TableInput>(){
            new TableInput("1", "TableInput1", false, false, "Prepopulated Answer 1"),
            new TableInput("2", "TableInput1", false, false, "Prepopulated Answer 2"),
        }
        };

        elsaAssessmentQuestions.Questions[0].Answer = JsonSerializer.Serialize(dataTableAnswer);
        elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.DataTable;

        var assessmentQuestionsDictionary = new Dictionary<string, object?>();
        assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

        workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

        workflowInstanceStore.Setup(x =>
        x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
    .ReturnsAsync(workflowInstance);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetActivityQuestions(loadWorkflowActivityRequest.ActivityId,
                loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

        //Act
        var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);
        var answerResult = JsonSerializer.Deserialize<DataTable>(result.Data.Questions[0].Answers.FirstOrDefault().AnswerText);

        //Assert
        Assert.NotNull(result.Data!.Questions);
        Assert.Equal(assessmentQuestions.Count(), result.Data!.Questions.Count());
        Assert.Empty(result.ErrorMessages);
        Assert.Equal("Prepopulated Answer 1", answerResult.Inputs[0].Input);
        Assert.Equal("Prepopulated Answer 2", answerResult.Inputs[1].Input);
    }

}