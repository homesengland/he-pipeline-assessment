using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.Workflow.LoadWorkflowActivity;
using Elsa.Services.Models;
using Moq;
using Xunit;
using Constants = Elsa.CustomActivities.Activities.Constants;

namespace Elsa.Server.Tests.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationWithoutErrors_NoErrorsAreEncountered(
          [Frozen] Mock<IQuestionInvoker> questionInvoker,
          [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
          [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
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

            pipelineAssessmentRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
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
            pipelineAssessmentRepository.Verify(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
            loadWorkflowActivityJsonHelper.Verify(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()), Times.Once);
            Assert.Equal(loadWorkflowActivityRequest.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Equal(loadWorkflowActivityRequest.ActivityId, result.Data.ActivityId);
            Assert.Equal(assessmentQuestion.PreviousActivityId, result.Data.PreviousActivityId);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_SelectedChoicesAreRestored_WhenActivityIsMultiChoice(
                             [Frozen] Mock<IQuestionInvoker> questionInvoker,
                             [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
                             [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
                             [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
                             LoadWorkflowActivityRequest loadWorkflowActivityRequest,
                             List<CollectedWorkflow> collectedWorkflows,
                             WorkflowInstance workflowInstance,
                             AssessmentQuestion assessmentQuestion,
                             LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            assessmentQuestion.Answer = @"[""My choice""]";
            assessmentQuestion.ActivityType = Constants.MultipleChoiceQuestion;

            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                    assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
                    CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            pipelineAssessmentRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);


            var data = new QuestionActivityData
            {
                MultipleChoice = new MultipleChoiceModel
                {
                    Choices = new Choice[]
                    {
                        new Choice
                        {
                            Answer = "My choice",
                        }
                    }
                }
            };

            loadWorkflowActivityJsonHelper
                .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
                .Returns(data);

            var existingDictionaryItem = workflowInstance.ActivityData.First();

            var customDictionary = new Dictionary<string, object?>
            {
                { existingDictionaryItem.Key, existingDictionaryItem.Value }
            };

            workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, customDictionary);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Single(result.Data!.QuestionActivityData.MultipleChoice.SelectedChoices);
            Assert.Equal("My choice", result.Data!.QuestionActivityData.MultipleChoice.SelectedChoices.First());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_SelectedChoicesAreRestored_WhenActivityIsSingleChoice(
                             [Frozen] Mock<IQuestionInvoker> questionInvoker,
                             [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
                             [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
                             [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
                             LoadWorkflowActivityRequest loadWorkflowActivityRequest,
                             List<CollectedWorkflow> collectedWorkflows,
                             WorkflowInstance workflowInstance,
                             AssessmentQuestion assessmentQuestion,
                             LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            assessmentQuestion.Answer = "My choice";
            assessmentQuestion.ActivityType = Constants.SingleChoiceQuestion;

            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId,
                    assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId,
                    CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            pipelineAssessmentRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);


            var data = new QuestionActivityData
            {
                SingleChoice = new SingleChoiceModel()
                {
                    Choices = new SingleChoice[]
                    {
                        new SingleChoice
                        {
                            Answer = "My choice",
                        },
                        new SingleChoice
                        {
                            Answer = "My choice 2",
                        }
                    }
                }
            };

            loadWorkflowActivityJsonHelper
                .Setup(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()))
                .Returns(data);

            var existingDictionaryItem = workflowInstance.ActivityData.First();

            var customDictionary = new Dictionary<string, object?>
            {
                { existingDictionaryItem.Key, existingDictionaryItem.Value }
            };

            workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, customDictionary);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Equal(assessmentQuestion.Answer, result.Data!.QuestionActivityData.SingleChoice.SelectedAnswer);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenActivityDataMappingReturnsNull(
          [Frozen] Mock<IQuestionInvoker> questionInvoker,
          [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
          [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
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

            pipelineAssessmentRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
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
            pipelineAssessmentRepository.Verify(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
            loadWorkflowActivityJsonHelper.Verify(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()), Times.Once);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenActivityIdCannotBeFoundInActivityDictionary(
           [Frozen] Mock<IQuestionInvoker> questionInvoker,
           [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
           [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
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

            pipelineAssessmentRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(assessmentQuestion);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.QuestionActivityData);
            Assert.Equal($"Cannot find activity Id {loadWorkflowActivityRequest.ActivityId} in the workflow activity data dictionary", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
            loadWorkflowActivityJsonHelper.Verify(x => x.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenAssessmentQuestionDoesNotExist(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
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

            pipelineAssessmentRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
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
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
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

            pipelineAssessmentRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
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
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            [Frozen] Mock<ILoadWorkflowActivityJsonHelper> loadWorkflowActivityJsonHelper,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            AssessmentQuestion assessmentQuestion,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            questionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, assessmentQuestion.ActivityType, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(new List<CollectedWorkflow>());

            pipelineAssessmentRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
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
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
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

            pipelineAssessmentRepository.Setup(x => x.GetAssessmentQuestion(loadWorkflowActivityRequest.ActivityId,
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
    }
}
