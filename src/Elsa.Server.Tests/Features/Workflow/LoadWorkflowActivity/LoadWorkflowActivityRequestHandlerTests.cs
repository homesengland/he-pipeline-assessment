using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.Workflow.LoadWorkflowActivity;
using Elsa.Services.Models;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandlerTests
    {
        //[Frozen] Mock<IMultipleChoiceQuestionInvoker> multipleChoiceQuestionInvoker,
        //[Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
        //[Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
        //[Frozen] Mock<ILoadWorkflowActivityMapper> loadWorkflowActivityMapper,

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationWithoutErrors_NoErrorsAreEncountered(
          [Frozen] Mock<IMultipleChoiceQuestionInvoker> multipleChoiceQuestionInvoker,
          [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
          [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
          [Frozen] Mock<ILoadWorkflowActivityMapper> loadWorkflowActivityMapper,
          LoadWorkflowActivityRequest loadWorkflowActivityRequest,
          List<CollectedWorkflow> collectedWorkflows,
          WorkflowInstance workflowInstance,
          MultipleChoiceQuestionModel multipleChoiceQuestionModel,
          MultipleChoiceQuestionActivityData multipleChoiceQuestionActivityData,
          LoadWorkflowActivityRequestHandler sut)
        {

            //Arrange
            multipleChoiceQuestionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(multipleChoiceQuestionModel);

            loadWorkflowActivityMapper
                .Setup(x => x.ActivityDataDictionaryToActivityData(It.IsAny<IDictionary<string, object?>>()))
                .Returns(multipleChoiceQuestionActivityData);

            var existingDictionaryItem = workflowInstance.ActivityData.First();

            var customDictionary = new Dictionary<string, object?>
            {
                { existingDictionaryItem.Key, existingDictionaryItem.Value }
            };

            workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, customDictionary);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result.Data!.MultipleChoiceQuestionActivityData);
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.GetMultipleChoiceQuestions(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
            loadWorkflowActivityMapper.Verify(x => x.ActivityDataDictionaryToActivityData(It.IsAny<IDictionary<string, object?>>()), Times.Once);
            Assert.Equal(loadWorkflowActivityRequest.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Equal(loadWorkflowActivityRequest.ActivityId, result.Data.ActivityId);
            Assert.Equal(multipleChoiceQuestionModel.PreviousActivityId, result.Data.PreviousActivityId);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenActivityDataMappingReturnsNull(
          [Frozen] Mock<IMultipleChoiceQuestionInvoker> multipleChoiceQuestionInvoker,
          [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
          [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
          [Frozen] Mock<ILoadWorkflowActivityMapper> loadWorkflowActivityMapper,
          LoadWorkflowActivityRequest loadWorkflowActivityRequest,
          List<CollectedWorkflow> collectedWorkflows,
          WorkflowInstance workflowInstance,
          MultipleChoiceQuestionModel multipleChoiceQuestionModel,
          LoadWorkflowActivityRequestHandler sut)
        {

            //Arrange
            multipleChoiceQuestionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(multipleChoiceQuestionModel);

            loadWorkflowActivityMapper
                .Setup(x => x.ActivityDataDictionaryToActivityData(It.IsAny<IDictionary<string, object?>>()))
                .Returns((MultipleChoiceQuestionActivityData?)null);

            var existingDictionaryItem = workflowInstance.ActivityData.First();

            var customDictionary = new Dictionary<string, object?>
            {
                { existingDictionaryItem.Key, existingDictionaryItem.Value }
            };

            workflowInstance.ActivityData.Add(loadWorkflowActivityRequest.ActivityId, customDictionary);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.MultipleChoiceQuestionActivityData);
            Assert.Equal($"Failed to map activity data", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.GetMultipleChoiceQuestions(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
            loadWorkflowActivityMapper.Verify(x => x.ActivityDataDictionaryToActivityData(It.IsAny<IDictionary<string, object?>>()), Times.Once);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenActivityIdCannotBeFoundInActivityDictionary(
           [Frozen] Mock<IMultipleChoiceQuestionInvoker> multipleChoiceQuestionInvoker,
           [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
           [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
           [Frozen] Mock<ILoadWorkflowActivityMapper> loadWorkflowActivityMapper,
           LoadWorkflowActivityRequest loadWorkflowActivityRequest,
           List<CollectedWorkflow> collectedWorkflows,
           WorkflowInstance workflowInstance,
           MultipleChoiceQuestionModel multipleChoiceQuestionModel,
           LoadWorkflowActivityRequestHandler sut)
        {

            //Arrange
            multipleChoiceQuestionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(multipleChoiceQuestionModel);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.MultipleChoiceQuestionActivityData);
            Assert.Equal($"Cannot find activity Id {loadWorkflowActivityRequest.ActivityId} in the workflow activity data dictionary", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.GetMultipleChoiceQuestions(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
            loadWorkflowActivityMapper.Verify(x => x.ActivityDataDictionaryToActivityData(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenMultipleChoiceQuestioinResponseDoesNotExist(
            [Frozen] Mock<IMultipleChoiceQuestionInvoker> multipleChoiceQuestionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            [Frozen] Mock<ILoadWorkflowActivityMapper> loadWorkflowActivityMapper,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            multipleChoiceQuestionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(loadWorkflowActivityRequest.ActivityId,
                    loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((MultipleChoiceQuestionModel?)null);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.MultipleChoiceQuestionActivityData);
            Assert.Equal($"Unable to find workflow instance with Id: {loadWorkflowActivityRequest.WorkflowInstanceId} and Activity Id: {loadWorkflowActivityRequest.ActivityId}", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.GetMultipleChoiceQuestions(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None), Times.Once);
            loadWorkflowActivityMapper.Verify(x => x.ActivityDataDictionaryToActivityData(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenCollectedWorkflowInstaceIsNull(
            [Frozen] Mock<IMultipleChoiceQuestionInvoker> multipleChoiceQuestionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            [Frozen] Mock<ILoadWorkflowActivityMapper> loadWorkflowActivityMapper,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            List<CollectedWorkflow> collectedWorkflows,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            multipleChoiceQuestionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore.Setup(x =>
                    x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None))
                .ReturnsAsync((WorkflowInstance?)null);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.MultipleChoiceQuestionActivityData);
            Assert.Equal($"Unable to find workflow instance with Id: {loadWorkflowActivityRequest.WorkflowInstanceId} in Elsa database", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.GetMultipleChoiceQuestions(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None), Times.Never);
            loadWorkflowActivityMapper.Verify(x => x.ActivityDataDictionaryToActivityData(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenNoWorkflowsAreCollected(
            [Frozen] Mock<IMultipleChoiceQuestionInvoker> multipleChoiceQuestionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            [Frozen] Mock<ILoadWorkflowActivityMapper> loadWorkflowActivityMapper,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            multipleChoiceQuestionInvoker
                .Setup(x => x.FindWorkflowsAsync(loadWorkflowActivityRequest.ActivityId, loadWorkflowActivityRequest.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(new List<CollectedWorkflow>());

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.MultipleChoiceQuestionActivityData);
            Assert.Equal($"Unable to progress workflow instance Id {loadWorkflowActivityRequest.WorkflowInstanceId}. No collected workflows", result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Never);
            pipelineAssessmentRepository.Verify(x => x.GetMultipleChoiceQuestions(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None), Times.Never);
            loadWorkflowActivityMapper.Verify(x => x.ActivityDataDictionaryToActivityData(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_ExceptionIsThrown(
            [Frozen] Mock<IMultipleChoiceQuestionInvoker> multipleChoiceQuestionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            [Frozen] Mock<ILoadWorkflowActivityMapper> loadWorkflowActivityMapper,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            Exception exception,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            multipleChoiceQuestionInvoker
                .Setup(x => x.FindWorkflowsAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .Throws(exception);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.MultipleChoiceQuestionActivityData);
            Assert.Equal(exception.Message, result.ErrorMessages.Single());
            workflowInstanceStore.Verify(x => x.FindAsync(It.IsAny<WorkflowInstanceIdSpecification>(), CancellationToken.None), Times.Never);
            pipelineAssessmentRepository.Verify(x => x.GetMultipleChoiceQuestions(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None), Times.Never);
            loadWorkflowActivityMapper.Verify(x => x.ActivityDataDictionaryToActivityData(It.IsAny<IDictionary<string, object?>>()), Times.Never);

        }
    }
}
