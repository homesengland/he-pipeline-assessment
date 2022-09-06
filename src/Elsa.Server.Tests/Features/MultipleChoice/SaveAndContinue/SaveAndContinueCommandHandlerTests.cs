using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.MultipleChoice.SaveAndContinue;
using Elsa.Server.Features.Shared.SaveAndContinue;
using Elsa.CustomActivities.Activities;
using Elsa.Server.Models;
using Elsa.Services.Models;
using Moq;
using Xunit;
using Constants = Elsa.CustomActivities.Activities.Constants;

namespace Elsa.Server.Tests.Features.MultipleChoice.SaveAndContinue
{
    public class SaveAndContinueCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndDoesNotInsertNewQuestionIfAlreadyExists(
            [Frozen] Mock<IQuestionInvoker> multipleChoiceQuestionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            AssessmentQuestion currentAssessmentQuestion,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            AssessmentQuestion nextAssessmentQuestion,
            MultipleChoiceSaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut)
        {
            //Arrange
            var opResult = new OperationResult<SaveAndContinueResponse>()
            {
                Data = new SaveAndContinueResponse
                {
                    WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
                    NextActivityId = workflowInstance.Output!.ActivityId
                },
                ErrorMessages = new List<string>(),
                ValidationMessages = new List<string>()
            };

            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestion);

            multipleChoiceQuestionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId, Constants.MultipleChoiceQuestion,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestion, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync(workflowInstance);

            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(nextAssessmentQuestion);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            pipelineAssessmentRepository.Verify(x => x.UpdateMultipleChoiceQuestion(currentAssessmentQuestion, CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.CreateMultipleChoiceQuestionAsync(nextAssessmentQuestion, CancellationToken.None), Times.Never);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Empty(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndInsertsNewQuestionIfDoesNotExist(
            [Frozen] Mock<IQuestionInvoker> multipleChoiceQuestionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            [Frozen] Mock<ISaveAndContinueMapper> saveAndContinueMapper,
            AssessmentQuestion currentAssessmentQuestion,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            AssessmentQuestion nextAssessmentQuestion,
            MultipleChoiceSaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut,
            string nextActivityType)
        {
            //Arrange
            var opResult = new OperationResult<SaveAndContinueResponse>()
            {
                Data = new SaveAndContinueResponse
                {
                    WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
                    NextActivityId = workflowInstance.Output!.ActivityId
                },
                ErrorMessages = new List<string>(),
                ValidationMessages = new List<string>()
            };

            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestion);

            multipleChoiceQuestionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,Constants.MultipleChoiceQuestion,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestion, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync(workflowInstance);

            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((AssessmentQuestion?)null);

            saveAndContinueMapper
                .Setup(x => x.SaveAndContinueCommandToNextMultipleChoiceQuestionModel(saveAndContinueCommand,
                    workflowInstance.Output!.ActivityId, nextActivityType)).Returns(nextAssessmentQuestion);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            pipelineAssessmentRepository.Verify(x => x.UpdateMultipleChoiceQuestion(currentAssessmentQuestion, CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.CreateMultipleChoiceQuestionAsync(nextAssessmentQuestion, CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Empty(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenOutputIsNullOnWorkflowInstance(
            [Frozen] Mock<IQuestionInvoker> multipleChoiceQuestionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            AssessmentQuestion currentAssessmentQuestion,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            MultipleChoiceSaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut)
        {
            //Arrange
            workflowInstance.Output = null;
            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestion);

            multipleChoiceQuestionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId, Constants.MultipleChoiceQuestion,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestion, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync(workflowInstance);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            pipelineAssessmentRepository.Verify(x => x.UpdateMultipleChoiceQuestion(currentAssessmentQuestion, CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.CreateMultipleChoiceQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Never);
            Assert.Null(result.Data);
            Assert.Equal($"Workflow instance output for workflow instance Id {saveAndContinueCommand.WorkflowInstanceId} is not set. Unable to determine next activity", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenWorkflowInstanceIsNull(
            [Frozen] Mock<IQuestionInvoker> multipleChoiceQuestionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            AssessmentQuestion currentAssessmentQuestion,
            List<CollectedWorkflow> collectedWorkflows,
            MultipleChoiceSaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut)
        {
            //Arrange
            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestion);

            multipleChoiceQuestionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId, Constants.MultipleChoiceQuestion,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestion, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync((WorkflowInstance?)null);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            pipelineAssessmentRepository.Verify(x => x.UpdateMultipleChoiceQuestion(currentAssessmentQuestion, CancellationToken.None), Times.Once);
            pipelineAssessmentRepository.Verify(x => x.CreateMultipleChoiceQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Never);
            Assert.Null(result.Data);
            Assert.Equal($"Unable to find workflow instance with Id: {saveAndContinueCommand.WorkflowInstanceId} in Elsa database", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenMultipleChoiceQuestionNotFoundInDatabase(
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            MultipleChoiceSaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut)
        {
            //Arrange
            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((AssessmentQuestion?)null);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            pipelineAssessmentRepository.Verify(x => x.UpdateMultipleChoiceQuestion(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Never);
            pipelineAssessmentRepository.Verify(x => x.CreateMultipleChoiceQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Never);
            Assert.Null(result.Data);
            Assert.Equal($"Unable to find workflow instance with Id: {saveAndContinueCommand.WorkflowInstanceId} and Activity Id: {saveAndContinueCommand.ActivityId} in custom database", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenADependencyThrows(
            [Frozen] Mock<IPipelineAssessmentRepository> pipelineAssessmentRepository,
            Exception exception,
            MultipleChoiceSaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut)
        {
            //Arrange
            pipelineAssessmentRepository.Setup(x => x.GetMultipleChoiceQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .Throws(exception);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            pipelineAssessmentRepository.Verify(x => x.UpdateMultipleChoiceQuestion(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Never);
            pipelineAssessmentRepository.Verify(x => x.CreateMultipleChoiceQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Never);
            Assert.Null(result.Data);
            Assert.Equal(exception.Message, result.ErrorMessages.Single());
        }
    }
}