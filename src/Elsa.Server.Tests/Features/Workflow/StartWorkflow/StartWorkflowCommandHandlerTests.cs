using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;


namespace Elsa.Server.Tests.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            [Frozen] Mock<IStartsWorkflow> startsWorkflow,
            [Frozen] Mock<IStartWorkflowMapper> startWorkflowMapper,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
            WorkflowBlueprint workflowBlueprint,
            ActivityBlueprint activityBlueprint,
            RunWorkflowResult runWorkflowResult,
            StartWorkflowCommand startWorkflowCommand,
            CustomActivityNavigation customActivityNavigation,
            StartWorkflowResponse startWorkflowResponse,
            StartWorkflowCommandHandler sut)
        {

            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint
            };
            var opResult = new OperationResult<StartWorkflowResponse>()
            {
                Data = startWorkflowResponse,
                ErrorMessages = new List<string>(),
                ValidationMessages = null
            };

            activityBlueprint.Id = runWorkflowResult.WorkflowInstance!.LastExecutedActivityId!;
            workflowBlueprint.Activities.Add(activityBlueprint);

            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, startWorkflowCommand.CorrelationId, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            startWorkflowMapper.Setup(x => x.RunWorkflowResultToCustomNavigationActivity(runWorkflowResult, activityBlueprint.Type))
                .Returns(customActivityNavigation);

            startWorkflowMapper.Setup(x => x.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult, activityBlueprint.Type, workflowBlueprint.Name!))
                .Returns(opResult.Data);

            workflowNextActivityProvider
                .Setup(x => x.GetStartWorkflowNextActivity(activityBlueprint, runWorkflowResult.WorkflowInstance.Id,
                    CancellationToken.None)).ReturnsAsync(workflowNextActivityModel);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(x => x.CreateCustomActivityNavigationAsync(customActivityNavigation, CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_GivenQuestionScreenActivityAndFirstQuestionScreenIsNotSkipped(
           [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
           [Frozen] Mock<IStartsWorkflow> startsWorkflow,
           [Frozen] Mock<IStartWorkflowMapper> startWorkflowMapper,
           [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
           [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
           WorkflowBlueprint workflowBlueprint,
           ActivityBlueprint activityBlueprint,
           RunWorkflowResult runWorkflowResult,
           StartWorkflowCommand startWorkflowCommand,
           CustomActivityNavigation customActivityNavigation,
           StartWorkflowResponse startWorkflowResponse,
           List<Question> questions,
           StartWorkflowCommandHandler sut)
        {

            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint
            };

            activityBlueprint.Id = runWorkflowResult.WorkflowInstance!.LastExecutedActivityId!;
            activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
            workflowBlueprint.Activities.Add(activityBlueprint);

            var assessmentQuestionsDictionary = new Dictionary<string, object?>();
            var assessmentQuestions = new AssessmentQuestions() { Questions = questions };
            assessmentQuestionsDictionary.Add("Questions", assessmentQuestions);

            runWorkflowResult.WorkflowInstance.ActivityData.Add(activityBlueprint.Id, assessmentQuestionsDictionary);

            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, startWorkflowCommand.CorrelationId, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            startWorkflowMapper.Setup(x => x.RunWorkflowResultToCustomNavigationActivity(runWorkflowResult, activityBlueprint.Type))
                .Returns(customActivityNavigation);

            startWorkflowMapper.Setup(x => x.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult, activityBlueprint.Type, workflowBlueprint.Name!))
                .Returns(startWorkflowResponse);

            workflowNextActivityProvider
                .Setup(x => x.GetStartWorkflowNextActivity(activityBlueprint, runWorkflowResult.WorkflowInstance.Id,
                    CancellationToken.None)).ReturnsAsync(workflowNextActivityModel);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(x => x.CreateCustomActivityNavigationAsync(customActivityNavigation, CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(x => x.CreateQuestionScreenAnswersAsync(It.IsAny<List<QuestionScreenAnswer>>(), CancellationToken.None), Times.Once);
            Assert.Equal(startWorkflowResponse.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(startWorkflowResponse.WorkflowName, result.Data.WorkflowName);
            Assert.Equal(startWorkflowResponse.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Equal(startWorkflowResponse.ActivityType, result.Data.ActivityType);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_GivenFirstQuestionScreenActivityIsSkipped(
           [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
           [Frozen] Mock<IStartsWorkflow> startsWorkflow,
           [Frozen] Mock<IStartWorkflowMapper> startWorkflowMapper,
           [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
           [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
           WorkflowBlueprint workflowBlueprint,
           ActivityBlueprint activityBlueprint,
           ActivityBlueprint nextActivityBlueprint,
           RunWorkflowResult runWorkflowResult,
           StartWorkflowCommand startWorkflowCommand,
           CustomActivityNavigation customActivityNavigation,
           StartWorkflowResponse startWorkflowResponse,
           List<Question> questions,
           StartWorkflowCommandHandler sut)
        {

            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = nextActivityBlueprint
            };
            var opResult = new OperationResult<StartWorkflowResponse>()
            {
                Data = startWorkflowResponse,
                ErrorMessages = new List<string>(),
                ValidationMessages = null
            };

            activityBlueprint.Id = runWorkflowResult.WorkflowInstance!.LastExecutedActivityId!;
            activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
            workflowBlueprint.Activities.Add(activityBlueprint);

            nextActivityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
            workflowBlueprint.Activities.Add(nextActivityBlueprint);

            var assessmentQuestionsDictionary = new Dictionary<string, object?>();
            var assessmentQuestions = new AssessmentQuestions() { Questions = questions };
            assessmentQuestionsDictionary.Add("Questions", assessmentQuestions);

            runWorkflowResult.WorkflowInstance.ActivityData.Add(activityBlueprint.Id, assessmentQuestionsDictionary);
            runWorkflowResult.WorkflowInstance.ActivityData.Add(nextActivityBlueprint.Id, assessmentQuestionsDictionary);

            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, startWorkflowCommand.CorrelationId, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            startWorkflowMapper.Setup(x => x.RunWorkflowResultToCustomNavigationActivity(runWorkflowResult, nextActivityBlueprint.Type))
                .Returns(customActivityNavigation);

            startWorkflowMapper.Setup(x => x.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult, nextActivityBlueprint.Type, workflowBlueprint.Name!))
                .Returns(opResult.Data);

            workflowNextActivityProvider
                .Setup(x => x.GetStartWorkflowNextActivity(activityBlueprint, runWorkflowResult.WorkflowInstance.Id,
                    CancellationToken.None)).ReturnsAsync(workflowNextActivityModel);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(x => x.CreateCustomActivityNavigationAsync(customActivityNavigation, CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(x => x.CreateQuestionScreenAnswersAsync(It.IsAny<List<QuestionScreenAnswer>>(), CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.NotEqual(activityBlueprint.Id, opResult.Data.NextActivityId);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorOperationResult_StartworkflowReturnsNullWorkflowInstance(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            [Frozen] Mock<IStartsWorkflow> startsWorkflow,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            WorkflowBlueprint workflowBlueprint,
            ActivityBlueprint activityBlueprint,
            StartWorkflowCommand startWorkflowCommand,
            StartWorkflowCommandHandler sut)
        {
            //Arrange
            var emptyRunWorkflowResult = new RunWorkflowResult(null, null, null, true);
            workflowBlueprint.Activities.Add(activityBlueprint);

            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, startWorkflowCommand.CorrelationId, null, null, CancellationToken.None))
                .ReturnsAsync(emptyRunWorkflowResult);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(x => x.CreateCustomActivityNavigationAsync(It.IsAny<CustomActivityNavigation>(), CancellationToken.None), Times.Never);
            Assert.Null(result.Data);
            Assert.Equal("Workflow instance is null", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorOperationResult_WhenCannotDeserialiseWorkflowResult(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            [Frozen] Mock<IStartsWorkflow> startsWorkflow,
            [Frozen] Mock<IStartWorkflowMapper> startWorkflowMapper,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
            WorkflowBlueprint workflowBlueprint,
            ActivityBlueprint activityBlueprint,
            RunWorkflowResult runWorkflowResult,
            StartWorkflowCommand startWorkflowCommand,
            StartWorkflowCommandHandler sut)
        {
            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint
            };
            activityBlueprint.Id = runWorkflowResult.WorkflowInstance!.LastExecutedActivityId!;
            workflowBlueprint.Activities.Add(activityBlueprint);

            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, startWorkflowCommand.CorrelationId, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            startWorkflowMapper.Setup(x => x.RunWorkflowResultToCustomNavigationActivity(runWorkflowResult, activityBlueprint.Type))
                .Returns((CustomActivityNavigation?)null);

            workflowNextActivityProvider
                .Setup(x => x.GetStartWorkflowNextActivity(activityBlueprint, runWorkflowResult.WorkflowInstance.Id,
                    CancellationToken.None)).ReturnsAsync(workflowNextActivityModel);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(x => x.CreateCustomActivityNavigationAsync(It.IsAny<CustomActivityNavigation>(), CancellationToken.None), Times.Never);
            Assert.Null(result.Data);
            Assert.Equal("Failed to deserialize RunWorkflowResult", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorOperationResult_WhenADependencyThrows(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            StartWorkflowCommand startWorkflowCommand,
            Exception exception,
            StartWorkflowCommandHandler sut)
        {
            //Arrange
            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .Throws(exception);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(x => x.CreateCustomActivityNavigationAsync(It.IsAny<CustomActivityNavigation>(), CancellationToken.None), Times.Never);
            Assert.Null(result.Data);
            Assert.Equal(exception.Message, result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorMessageResult_WhenActivityIsNull(
        [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
        [Frozen] Mock<IStartsWorkflow> startsWorkflow,
        WorkflowBlueprint workflowBlueprint,
        RunWorkflowResult runWorkflowResult,
        StartWorkflowCommand startWorkflowCommand,
        StartWorkflowCommandHandler sut)
        {
            //Arrange
            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, startWorkflowCommand.CorrelationId, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            Assert.Equal(1, result.ErrorMessages.Count);
            Assert.Equal("Failed to get activity", result.ErrorMessages.Single());
        }
    }
}
