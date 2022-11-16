using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.Workflow.LoadWorkflowActivity;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;


namespace Elsa.Server.Tests.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationWithoutErrors_NoErrorsAreEncountered(
          [Frozen] Mock<IQuestionInvoker> questionInvoker,
          [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
          [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
          [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
          LoadWorkflowActivityRequest loadWorkflowActivityRequest,
          List<CollectedWorkflow> collectedWorkflows,
          WorkflowInstance workflowInstance,
          AssessmentQuestion assessmentQuestion,
          QuestionActivityData questionActivityData,
          LoadWorkflowActivityRequestHandler sut)
        {

            //Arrange
            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            loadWorkflowActivityJsonHelper
                .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
                .Returns(questionActivityData);

            var existingDictionaryItem = workflowInstance.ActivityData.First();

            var customDictionary = new Dictionary<string, object?>
            {
                { existingDictionaryItem.Key, existingDictionaryItem.Value }
            };

            workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, customDictionary);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result.Data!.QuestionActivityData);
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
            loadWorkflowActivityJsonHelper.Verify(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()), Times.Once);
            Assert.Equal(loadWorkflowActivityRequest.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Equal(loadWorkflowActivityRequest.ActivityId, result.Data.ActivityId);
            Assert.Equal(assessmentQuestion.PreviousActivityId, result.Data.PreviousActivityId);
        }

        //[Theory]
        //[AutoMoqData]
        //public async Task Handle_SelectedChoicesAreRestored_WhenActivityIsMultiChoice(
        //                     [Frozen] Mock<IQuestionInvoker> questionInvoker,
        //                     [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        //                     [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        //                     [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
        //                     LoadWorkflowActivityRequest loadWorkflowActivityRequest,
        //                     List<CollectedWorkflow> collectedWorkflows,
        //                     WorkflowInstance workflowInstance,
        //                     AssessmentQuestion assessmentQuestion,
        //                     LoadWorkflowActivityRequestHandler sut)
        //{
        //    //Arrange
        //    assessmentQuestion.Answer = @"[""My choice""]";
        //    assessmentQuestion.ActivityType = ActivityTypeConstants.MultipleChoiceQuestion;

        //    questionInvoker
        //        .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
        //            assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
        //            CancellationToken.None))
        //        .ReturnsAsync(collectedWorkflows);

        //    workflowInstanceStore.Setup(x =>
        //            x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
        //        .ReturnsAsync(workflowInstance);

        //    elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
        //            loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
        //        .ReturnsAsync(assessmentQuestion);


        //    var data = new QuestionActivityData
        //    {
        //        Checkbox = new Checkbox
        //        {
        //            Choices = new Choice[]
        //            {
        //                new Choice
        //                {
        //                    Answer = "My choice",
        //                }
        //            }
        //        }
        //    };

        //    loadWorkflowActivityJsonHelper
        //        .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
        //        .Returns(data);

        //    var existingDictionaryItem = workflowInstance.ActivityData.First();

        //    var customDictionary = new Dictionary<string, object?>
        //    {
        //        { existingDictionaryItem.Key, existingDictionaryItem.Value }
        //    };

        //    workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, customDictionary);

        //    //Act
        //    var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //    //Assert
        //    Assert.Single(result.Data!.QuestionActivityData.Checkbox.SelectedChoices);
        //    Assert.Equal("My choice", result.Data!.QuestionActivityData.Checkbox.SelectedChoices.First());
        //}

        //[Theory]
        //[AutoMoqData]
        //public async Task Handle_SelectedChoicesAreRestored_WhenActivityIsSingleChoice(
        //                     [Frozen] Mock<IQuestionInvoker> questionInvoker,
        //                     [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        //                     [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        //                     [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
        //                     LoadWorkflowActivityRequest loadWorkflowActivityRequest,
        //                     List<CollectedWorkflow> collectedWorkflows,
        //                     WorkflowInstance workflowInstance,
        //                     AssessmentQuestion assessmentQuestion,
        //                     LoadWorkflowActivityRequestHandler sut)
        //{
        //    //Arrange
        //    assessmentQuestion.Answer = "My choice";
        //    assessmentQuestion.ActivityType = Constants.SingleChoiceQuestion;

        //    questionInvoker
        //        .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
        //            assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
        //            CancellationToken.None))
        //        .ReturnsAsync(collectedWorkflows);

        //    workflowInstanceStore.Setup(x =>
        //            x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
        //        .ReturnsAsync(workflowInstance);

        //    elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
        //            loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
        //        .ReturnsAsync(assessmentQuestion);


        //    var data = new QuestionActivityData
        //    {
        //        Radio = new Radio()
        //        {
        //            Choices = new Choice[]
        //            {
        //                new Choice
        //                {
        //                    Answer = "My choice",
        //                },
        //                new Choice
        //                {
        //                    Answer = "My choice 2",
        //                }
        //            }
        //        }
        //    };

        //    loadWorkflowActivityJsonHelper
        //        .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
        //        .Returns(data);

        //    var existingDictionaryItem = workflowInstance.ActivityData.First();

        //    var customDictionary = new Dictionary<string, object?>
        //    {
        //        { existingDictionaryItem.Key, existingDictionaryItem.Value }
        //    };

        //    workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, customDictionary);

        //    //Act
        //    var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

        //    //Assert
        //    Assert.Equal(assessmentQuestion.Answer, result.Data!.QuestionActivityData.Radio.SelectedAnswer);
        //}


        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenActivityDataMappingReturnsNull(
          [Frozen] Mock<IQuestionInvoker> questionInvoker,
          [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
          [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
          [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
          LoadWorkflowActivityRequest loadWorkflowActivityRequest,
          List<CollectedWorkflow> collectedWorkflows,
          WorkflowInstance workflowInstance,
          AssessmentQuestion assessmentQuestion,
          LoadWorkflowActivityRequestHandler sut)
        {

            //Arrange
            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            loadWorkflowActivityJsonHelper
                .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
                .Returns((QuestionActivityData?)null);

            var existingDictionaryItem = workflowInstance.ActivityData.First();

            var customDictionary = new Dictionary<string, object?>
            {
                { existingDictionaryItem.Key, existingDictionaryItem.Value }
            };

            workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, customDictionary);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.QuestionActivityData);
            Assert.Equal($"Failed to map activity data", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
            loadWorkflowActivityJsonHelper.Verify(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()), Times.Once);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenActivityIdCannotBeFoundInActivityDictionary(
           [Frozen] Mock<IQuestionInvoker> questionInvoker,
           [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
           [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
           [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
           LoadWorkflowActivityRequest loadWorkflowActivityRequest,
           List<CollectedWorkflow> collectedWorkflows,
           WorkflowInstance workflowInstance,
           AssessmentQuestion assessmentQuestion,
           LoadWorkflowActivityRequestHandler sut)
        {

            //Arrange
            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.QuestionActivityData);
            Assert.Equal($"Cannot find activity Id {loadWorkflowActivityRequest.ActivityId} in the workflow activity data dictionary", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
            loadWorkflowActivityJsonHelper.Verify(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenAssessmentQuestionDoesNotExist(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                    It.IsAny<string>(), loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((AssessmentQuestion?)null);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.QuestionActivityData);
            Assert.Equal($"Unable to find workflow instance with Id: {loadWorkflowActivityRequest.WorkflowInstanceId} and Activity Id: {loadWorkflowActivityRequest.ActivityId} in Pipeline Assessment database", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Never);
            loadWorkflowActivityJsonHelper.Verify(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenCollectedWorkflowInstanceIsNull(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            AssessmentQuestion assessmentQuestion,
            List<CollectedWorkflow> collectedWorkflows,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync((WorkflowInstance?)null);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.QuestionActivityData);
            Assert.Equal($"Unable to find workflow instance with Id: {loadWorkflowActivityRequest.WorkflowInstanceId} in Elsa database", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
            loadWorkflowActivityJsonHelper.Verify(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenNoWorkflowsAreCollected(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            AssessmentQuestion assessmentQuestion,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(new List<CollectedWorkflow>());

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.QuestionActivityData);
            Assert.Equal($"Unable to progress workflow instance Id {loadWorkflowActivityRequest.WorkflowInstanceId}. No collected workflows", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Never);
            loadWorkflowActivityJsonHelper.Verify(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenExceptionIsThrown(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            AssessmentQuestion assessmentQuestion,
            Exception exception,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(It.IsAny<string>(), assessmentQuestion.ActivityType, It.IsAny<string>(), CancellationToken.None))
                .Throws(exception);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.QuestionActivityData);
            Assert.Equal(exception.Message, result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Never);
            loadWorkflowActivityJsonHelper.Verify(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsErrors_GivenActiovityDataReturnsNullForAcitivtyId(
         [Frozen] Mock<IQuestionInvoker> questionInvoker,
         [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
         [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
         [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
         LoadWorkflowActivityRequest loadWorkflowActivityRequest,
         List<CollectedWorkflow> collectedWorkflows,
         WorkflowInstance workflowInstance,
         AssessmentQuestion assessmentQuestion,
         QuestionActivityData questionActivityData,
         LoadWorkflowActivityRequestHandler sut)
        {

            //Arrange
            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            loadWorkflowActivityJsonHelper
                .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
                .Returns(questionActivityData);

            IDictionary<string, object?>? customDictionary = null;
            workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, customDictionary!);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.QuestionActivityData);
            Assert.Equal($"Activity data is null for Activity Id: {loadWorkflowActivityRequest.ActivityId}", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnErrors_GivenActivityIsQuestionScreenAndAssessmentQuestionsAreNull(
  [Frozen] Mock<IQuestionInvoker> questionInvoker,
  [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
  [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
  [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
  LoadWorkflowActivityRequest loadWorkflowActivityRequest,
  List<CollectedWorkflow> collectedWorkflows,
  WorkflowInstance workflowInstance,
  AssessmentQuestion assessmentQuestion,
  List<AssessmentQuestion> assessmentQuestions,
  QuestionActivityData questionActivityData,
  LoadWorkflowActivityRequestHandler sut)
        {

            //Arrange
            assessmentQuestion.ActivityType = ActivityTypeConstants.QuestionScreen;

            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(loadWorkflowActivityRequest.ActivityId,
            loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

            loadWorkflowActivityJsonHelper
                .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
            .Returns(questionActivityData);

            var assessmentQuestionsDictionary = new Dictionary<string, object?>();
            AssessmentQuestions? elsaAssessmentQuestions = null;
            assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);

            workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, assessmentQuestionsDictionary);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.MultiQuestionActivityData);
            Assert.Equal($"Failed to map activity data to MultiQuestionActivityData", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnMultiQuestionActivityData_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
          [Frozen] Mock<IQuestionInvoker> questionInvoker,
          [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
          [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
          [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
          LoadWorkflowActivityRequest loadWorkflowActivityRequest,
          List<CollectedWorkflow> collectedWorkflows,
          WorkflowInstance workflowInstance,
          AssessmentQuestion assessmentQuestion,
          List<AssessmentQuestion> assessmentQuestions,
          QuestionActivityData questionActivityData,
          AssessmentQuestions elsaAssessmentQuestions,
          LoadWorkflowActivityRequestHandler sut)
        {

            //Arrange
            assessmentQuestion.ActivityType = ActivityTypeConstants.QuestionScreen;

            for (int i = 0; i < assessmentQuestions.Count; i++)
            {
                var questionId = assessmentQuestions[i].QuestionId;
                elsaAssessmentQuestions.Questions[i].Id = questionId!;
            }

            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(loadWorkflowActivityRequest.ActivityId,
            loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

            loadWorkflowActivityJsonHelper
                .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
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
        public async Task Handle_ReturnMultiQuestionActivityDataWithMultiChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
          [Frozen] Mock<IQuestionInvoker> questionInvoker,
          [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
          [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
          [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
          LoadWorkflowActivityRequest loadWorkflowActivityRequest,
          List<CollectedWorkflow> collectedWorkflows,
          WorkflowInstance workflowInstance,
          AssessmentQuestion assessmentQuestion,
          List<AssessmentQuestion> assessmentQuestions,
          QuestionActivityData questionActivityData,
          AssessmentQuestions elsaAssessmentQuestions,
          LoadWorkflowActivityRequestHandler sut)
        {
            var myChoice = @"[""Choice1""]";
            //Arrange
            assessmentQuestion.ActivityType = ActivityTypeConstants.QuestionScreen;
            assessmentQuestions[0].Answer = myChoice;
            for (int i = 0; i < assessmentQuestions.Count; i++)
            {
                var questionId = assessmentQuestions[i].QuestionId;
                elsaAssessmentQuestions.Questions[i].Id = questionId!;
            }
            elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.CheckboxQuestion;
            elsaAssessmentQuestions.Questions[0].Checkbox.Choices = new List<CheckboxRecord>()
            {
                new CheckboxRecord("Choice1", false),
                new CheckboxRecord("Choice2", false),
                new CheckboxRecord("Choice3", false)
            };

            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(loadWorkflowActivityRequest.ActivityId,
            loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

            loadWorkflowActivityJsonHelper
                .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
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
        public async Task Handle_ReturnMultiQuestionActivityDataWithSingleChoiceOptionsCorrectlySelected_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
  [Frozen] Mock<IQuestionInvoker> questionInvoker,
  [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
  [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
  [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
  LoadWorkflowActivityRequest loadWorkflowActivityRequest,
  List<CollectedWorkflow> collectedWorkflows,
  WorkflowInstance workflowInstance,
  AssessmentQuestion assessmentQuestion,
  List<AssessmentQuestion> assessmentQuestions,
  QuestionActivityData questionActivityData,
  AssessmentQuestions elsaAssessmentQuestions,
  LoadWorkflowActivityRequestHandler sut)
        {

            //Arrange
            var myChoice = "Choice1";
            assessmentQuestion.ActivityType = ActivityTypeConstants.QuestionScreen;
            assessmentQuestions[0].Answer = "Choice1";
            for (int i = 0; i < assessmentQuestions.Count; i++)
            {
                var questionId = assessmentQuestions[i].QuestionId;
                elsaAssessmentQuestions.Questions[i].Id = questionId!;
            }
            elsaAssessmentQuestions.Questions[0].QuestionType = QuestionTypeConstants.RadioQuestion;
            elsaAssessmentQuestions.Questions[0].Radio.Choices = new List<RadioRecord>()
            {
                new RadioRecord("Choice1"),
                new RadioRecord("Choice2"),
                new RadioRecord("Choice3")
            };

            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(loadWorkflowActivityRequest.ActivityId,
            loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(assessmentQuestions);

            loadWorkflowActivityJsonHelper
                .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
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
}
