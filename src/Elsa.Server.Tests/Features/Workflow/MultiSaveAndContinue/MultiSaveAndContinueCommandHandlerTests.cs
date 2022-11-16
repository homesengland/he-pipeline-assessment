using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.Workflow.MultiSaveAndContinue;
using Elsa.Server.Models;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.MultiSaveAndContinue
{
    public class MultiSaveAndContinueCommandHandlerTests
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
                List<AssessmentQuestion> currentAssessmentQuestions,
                List<CollectedWorkflow> collectedWorkflows,
                WorkflowInstance workflowInstance,
                AssessmentQuestion nextAssessmentQuestion,
                MultiSaveAndContinueCommand saveAndContinueCommand,
                MultiSaveAndContinueCommandHandler sut)
        {
            //Arrange
            for (var i = 0; i < currentAssessmentQuestions.Count; i++)
            {
                var questionId = saveAndContinueCommand.Answers![i].Id;
                currentAssessmentQuestions[i].QuestionId = questionId;
            }

            var opResult = new OperationResult<MultiSaveAndContinueResponse>()
            {
                Data = new MultiSaveAndContinueResponse
                {
                    WorkflowInstanceId = saveAndContinueCommand.WorkflowInstanceId,
                    NextActivityId = workflowInstance.Output!.ActivityId
                },
                ErrorMessages = new List<string>(),
                ValidationMessages = null
            };

            activityBlueprint.Id = workflowInstance.Output.ActivityId;
            workflowBlueprint.Activities.Add(activityBlueprint);

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestions);

            workflowRegistry.Setup(x =>
                    x.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.QuestionScreen,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
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
                x => x.SaveChanges(CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(
                x => x.CreateAssessmentQuestionAsync(nextAssessmentQuestion, CancellationToken.None), Times.Never);

            Assert.Equal(saveAndContinueCommand.Answers![0].AnswerText, currentAssessmentQuestions[0].Answer);
            Assert.Equal(saveAndContinueCommand.Answers![1].AnswerText, currentAssessmentQuestions[1].Answer);
            Assert.Equal(saveAndContinueCommand.Answers![2].AnswerText, currentAssessmentQuestions[2].Answer);

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
               [Frozen] Mock<IMultiSaveAndContinueMapper> saveAndContinueMapper,
               [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
               WorkflowBlueprint workflowBlueprint,
               ActivityBlueprint activityBlueprint,
               List<AssessmentQuestion> currentAssessmentQuestions,
               List<CollectedWorkflow> collectedWorkflows,
               WorkflowInstance workflowInstance,
               AssessmentQuestion nextAssessmentQuestion,
               MultiSaveAndContinueCommand saveAndContinueCommand,
               string nextActivityType,
               MultiSaveAndContinueCommandHandler sut
           )
        {
            //Arrange
            var opResult = new OperationResult<MultiSaveAndContinueResponse>()
            {
                Data = new MultiSaveAndContinueResponse
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

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestions);

            workflowRegistry.Setup(x =>
                    x.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.QuestionScreen,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
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
                x => x.SaveChanges(CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(
                x => x.CreateAssessmentQuestionAsync(nextAssessmentQuestion, CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenOutputIsNullOnWorkflowInstance(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceStore> workflowInstanceStore,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            List<AssessmentQuestion> currentAssessmentQuestions,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            MultiSaveAndContinueCommand saveAndContinueCommand,
            MultiSaveAndContinueCommandHandler sut)
        {
            //Arrange
            workflowInstance.Output = null;
            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestions);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.QuestionScreen,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync(workflowInstance);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.SaveChanges(CancellationToken.None), Times.Once);
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
            List<AssessmentQuestion> currentAssessmentQuestions,
            List<CollectedWorkflow> collectedWorkflows,
            MultiSaveAndContinueCommand saveAndContinueCommand,
            MultiSaveAndContinueCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestions);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.QuestionScreen,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceStore
                .Setup(x => x.FindAsync(
                    It.Is<WorkflowInstanceIdSpecification>(y => y.Id == collectedWorkflows.First().WorkflowInstanceId),
                    CancellationToken.None)).ReturnsAsync((WorkflowInstance?)null);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.SaveChanges(CancellationToken.None), Times.Once);
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
        public async Task Handle_ShouldReturnErrors_WhenAssessmentQuestionsNotFoundInDatabase(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            MultiSaveAndContinueCommand saveAndContinueCommand,
            MultiSaveAndContinueCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(new List<AssessmentQuestion>());

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.SaveChanges(CancellationToken.None), Times.Never);
            elsaCustomRepository.Verify(
                x => x.CreateAssessmentQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None),
                Times.Never);
            Assert.Null(result.Data);
            Assert.Equal(
                $"Unable to find any questions for Question Screen activity Workflow Instance Id: {saveAndContinueCommand.WorkflowInstanceId} and Activity Id: {saveAndContinueCommand.ActivityId} in custom database",
                result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrors_WhenADependencyThrows(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            Exception exception,
            MultiSaveAndContinueCommand saveAndContinueCommand,
            MultiSaveAndContinueCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .Throws(exception);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.SaveChanges(CancellationToken.None), Times.Never);
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
            List<AssessmentQuestion> currentAssessmentQuestions,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowInstance workflowInstance,
            MultiSaveAndContinueCommand saveAndContinueCommand,
            MultiSaveAndContinueCommandHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestions);

            workflowRegistry.Setup(x =>
                    x.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.QuestionScreen,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
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
              [Frozen] Mock<IMultiSaveAndContinueMapper> saveAndContinueMapper,
              [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
              WorkflowBlueprint workflowBlueprint,
              ActivityBlueprint activityBlueprint,
              List<AssessmentQuestion> currentAssessmentQuestions,
              List<CollectedWorkflow> collectedWorkflows,
              WorkflowInstance workflowInstance,
              AssessmentQuestion nextAssessmentQuestion,
              MultiSaveAndContinueCommand saveAndContinueCommand,
              AssessmentQuestions elsaAssessmentQuestions,
              string nextActivityType,
              MultiSaveAndContinueCommandHandler sut
          )
        {
            //Arrange
            nextActivityType = ActivityTypeConstants.QuestionScreen;
            var opResult = new OperationResult<MultiSaveAndContinueResponse>()
            {
                Data = new MultiSaveAndContinueResponse
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

            elsaCustomRepository.Setup(x => x.GetAssessmentQuestions(saveAndContinueCommand.ActivityId,
                    saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(currentAssessmentQuestions);

            workflowRegistry.Setup(x =>
                    x.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(saveAndContinueCommand.ActivityId,
                    ActivityTypeConstants.QuestionScreen,
                    saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, CancellationToken.None))
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
