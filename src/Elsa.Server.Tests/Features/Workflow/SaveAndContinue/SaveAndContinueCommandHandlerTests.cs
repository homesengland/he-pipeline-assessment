using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.Workflow.SaveAndContinue;
using Elsa.Server.Models;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;
using Constants = Elsa.CustomActivities.Activities.Constants;

namespace Elsa.Server.Tests.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task
            Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndDoesNotInsertNewQuestionIfAlreadyExists(
                [Frozen] Mock<IQuestionInvoker> questionInvoker,
                [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
                [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
                [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
                WorkflowBlueprint workflowBlueprint,
                ActivityBlueprint activityBlueprint,
                AssessmentQuestion currentAssessmentQuestion,
                List<CollectedWorkflow> collectedWorkflows,
                WorkflowInstance workflowInstance,
                AssessmentQuestion nextAssessmentQuestion,
                SaveAndContinueCommand saveAndContinueCommand,
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
                ValidationMessages = null
            };
            activityBlueprint.Id = workflowInstance.Output.ActivityId;
            workflowBlueprint.Activities.Add(activityBlueprint);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestion);

            workflowRegistry.Setup(x =>
                    x.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    currentAssessmentQuestion.ActivityType,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestion, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(nextAssessmentQuestion);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.UpdateAssessmentQuestion(currentAssessmentQuestion, CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(
                x => x.CreateAssessmentQuestionAsync(nextAssessmentQuestion, CancellationToken.None), Times.Never);
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
                [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
                [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
                [Frozen] Mock<ISaveAndContinueMapper> saveAndContinueMapper,
                [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
                WorkflowBlueprint workflowBlueprint,
                ActivityBlueprint activityBlueprint,
                AssessmentQuestion currentAssessmentQuestion,
                List<CollectedWorkflow> collectedWorkflows,
                WorkflowInstance workflowInstance,
                AssessmentQuestion nextAssessmentQuestion,
                SaveAndContinueCommand saveAndContinueCommand,
                string nextActivityType,
                SaveAndContinueCommandHandler sut
            )
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
                ValidationMessages = null
            };

            activityBlueprint.Id = workflowInstance.Output.ActivityId;
            activityBlueprint.Type = nextActivityType;
            workflowBlueprint.Activities.Add(activityBlueprint);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestion);

            workflowRegistry.Setup(x =>
                    x.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    currentAssessmentQuestion.ActivityType,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestion, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((AssessmentQuestion?)null);

            saveAndContinueMapper
                .Setup(x => x.SaveAndContinueCommandToNextAssessmentQuestion(saveAndContinueCommand,
                    workflowInstance.Output!.ActivityId, nextActivityType)).Returns(nextAssessmentQuestion);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.UpdateAssessmentQuestion(currentAssessmentQuestion, CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(
                x => x.CreateAssessmentQuestionAsync(nextAssessmentQuestion, CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        //re-write to test SaveAndContinueCommandHandler Handle method
        //extract this test to the shared SaveAndContinueHandler
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenOutputIsNullOnWorkflowInstance(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            AssessmentQuestion currentAssessmentQuestion,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            SaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut)
        {
            //Arrange
            workflowInstance.Output = null;
            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestion);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    currentAssessmentQuestion.ActivityType,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestion, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync(workflowInstance);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.UpdateAssessmentQuestion(currentAssessmentQuestion, CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(
                x => x.CreateAssessmentQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None),
                Times.Never);
            Assert.Null(result.Data);
            Assert.Equal(
                $"Workflow instance output for workflow instance Id {saveAndContinueCommand.WorkflowInstanceId} is not set. Unable to determine next activity",
                result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenWorkflowInstanceIsNull(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            AssessmentQuestion currentAssessmentQuestion,
            List<CollectedWorkflow> collectedWorkflows,
            SaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestion);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    currentAssessmentQuestion.ActivityType,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestion, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync((WorkflowInstance?)null);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.UpdateAssessmentQuestion(currentAssessmentQuestion, CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(
                x => x.CreateAssessmentQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None),
                Times.Never);
            Assert.Null(result.Data);
            Assert.Equal(
                $"Unable to find workflow instance with Id: {saveAndContinueCommand.WorkflowInstanceId} in Elsa database",
                result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenAssessmentQuestionNotFoundInDatabase(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            SaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((AssessmentQuestion?)null);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.UpdateAssessmentQuestion(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Never);
            elsaCustomRepository.Verify(
                x => x.CreateAssessmentQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None),
                Times.Never);
            Assert.Null(result.Data);
            Assert.Equal(
                $"Unable to find activity instance with Workflow Instance Id: {saveAndContinueCommand.WorkflowInstanceId} and Activity Id: {saveAndContinueCommand.ActivityId} in custom database",
                result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenADependencyThrows(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            Exception exception,
            SaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .Throws(exception);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.UpdateAssessmentQuestion(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Never);
            elsaCustomRepository.Verify(
                x => x.CreateAssessmentQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None),
                Times.Never);
            Assert.Null(result.Data);
            Assert.Equal(exception.Message, result.ErrorMessages.Single());
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorMessageResult_WhenNextActivityIsNull(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            WorkflowBlueprint workflowBlueprint,
            AssessmentQuestion currentAssessmentQuestion,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            SaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestion);

            workflowRegistry.Setup(x =>
                    x.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    currentAssessmentQuestion.ActivityType,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestion, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync(workflowInstance);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            Assert.Equal(1, result.ErrorMessages.Count);
            Assert.Equal("Unable to determine next activity ID", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task
           Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful_AndInsertsNewMultiScreenQuestionIfDoesNotExist(
               [Frozen] Mock<IQuestionInvoker> questionInvoker,
               [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
               [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
               [Frozen] Mock<ISaveAndContinueMapper> saveAndContinueMapper,
               [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
               WorkflowBlueprint workflowBlueprint,
               ActivityBlueprint activityBlueprint,
               AssessmentQuestion currentAssessmentQuestion,
               List<CollectedWorkflow> collectedWorkflows,
               WorkflowInstance workflowInstance,
               AssessmentQuestion nextAssessmentQuestion,
               SaveAndContinueCommand saveAndContinueCommand,
               AssessmentQuestions elsaAssessmentQuestions,
               string nextActivityType,
               SaveAndContinueCommandHandler sut
           )
        {
            //Arrange
            nextActivityType = Constants.QuestionScreen;
            var opResult = new OperationResult<SaveAndContinueResponse>()
            {
                Data = new SaveAndContinueResponse
                {
                    WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
                    NextActivityId = workflowInstance.Output!.ActivityId
                },
                ErrorMessages = new List<string>(),
                ValidationMessages = null
            };

            activityBlueprint.Id = workflowInstance.Output.ActivityId;
            activityBlueprint.Type = nextActivityType;
            workflowBlueprint.Activities.Add(activityBlueprint);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestion);

            workflowRegistry.Setup(x =>
                    x.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    currentAssessmentQuestion.ActivityType,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestion, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync(workflowInstance);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestion(workflowInstance.Output.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((AssessmentQuestion?)null);

            saveAndContinueMapper
                .Setup(x => x.SaveAndContinueCommandToNextAssessmentQuestion(saveAndContinueCommand,
                    workflowInstance.Output!.ActivityId, nextActivityType)).Returns(nextAssessmentQuestion);

            var assessmentQuestionsDictionary = new Dictionary<string, object?>();
            assessmentQuestionsDictionary.Add("Questions", elsaAssessmentQuestions);
            workflowInstance.ActivityData.Add(workflowInstance.Output.ActivityId, assessmentQuestionsDictionary);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert          
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
            elsaCustomRepository.Verify(x => x.CreateAssessmentQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Once);
        }

    }
}