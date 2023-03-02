using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using WorkflowInstance = Elsa.Models.WorkflowInstance;


namespace Elsa.Server.Tests.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            [Frozen] Mock<IStartsWorkflow> startsWorkflow,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
            [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
            WorkflowBlueprint workflowBlueprint,
            ActivityBlueprint activityBlueprint,
            RunWorkflowResult runWorkflowResult,
            StartWorkflowCommand startWorkflowCommand,
            CustomActivityNavigation customActivityNavigation,
            WorkflowInstance workflowInstance,
            StartWorkflowCommandHandler sut)
        {

            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint,
                WorkflowInstance = workflowInstance
            };

            activityBlueprint.Id = runWorkflowResult.WorkflowInstance!.LastExecutedActivityId!;
            workflowBlueprint.Activities.Add(activityBlueprint);

            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, startWorkflowCommand.CorrelationId, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            workflowNextActivityProvider
                .Setup(x => x.GetStartWorkflowNextActivity(activityBlueprint, runWorkflowResult.WorkflowInstance.Id,
                    CancellationToken.None)).ReturnsAsync(workflowNextActivityModel);
            elsaCustomRepository
                .Setup(x => x.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                    workflowInstance.Id, CancellationToken.None)).ReturnsAsync(customActivityNavigation);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(workflowNextActivityModel.NextActivity.Id, customActivityNavigation,
                    workflowNextActivityModel.NextActivity, workflowNextActivityModel.WorkflowInstance, CancellationToken.None), Times.Once);
            Assert.Equal(workflowNextActivityModel.NextActivity.Id, result.Data!.NextActivityId);
            Assert.Equal(workflowBlueprint.Name, result.Data.WorkflowName);
            Assert.Equal(workflowInstance.Id, result.Data.WorkflowInstanceId);
            Assert.Equal(workflowNextActivityModel.NextActivity.Type, result.Data.ActivityType);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_GivenQuestionScreenActivityAndFirstQuestionScreenIsNotSkipped(
           [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
           [Frozen] Mock<IStartsWorkflow> startsWorkflow,
           [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
           [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
           [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
           WorkflowBlueprint workflowBlueprint,
           ActivityBlueprint activityBlueprint,
           RunWorkflowResult runWorkflowResult,
           StartWorkflowCommand startWorkflowCommand,
           CustomActivityNavigation customActivityNavigation,
           List<Question> questions,
           WorkflowInstance workflowInstance,
           StartWorkflowCommandHandler sut)
        {

            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint,
                WorkflowInstance = workflowInstance
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

            workflowNextActivityProvider
                .Setup(x => x.GetStartWorkflowNextActivity(activityBlueprint, runWorkflowResult.WorkflowInstance.Id,
                    CancellationToken.None)).ReturnsAsync(workflowNextActivityModel);

            elsaCustomRepository
                .Setup(x => x.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                    workflowInstance.Id, CancellationToken.None)).ReturnsAsync(customActivityNavigation);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(workflowNextActivityModel.NextActivity.Id, customActivityNavigation,
                    workflowNextActivityModel.NextActivity, workflowNextActivityModel.WorkflowInstance, CancellationToken.None), Times.Once);
            Assert.Equal(workflowNextActivityModel.NextActivity.Id, result.Data!.NextActivityId);
            Assert.Equal(workflowBlueprint.Name, result.Data.WorkflowName);
            Assert.Equal(workflowInstance.Id, result.Data.WorkflowInstanceId);
            Assert.Equal(workflowNextActivityModel.NextActivity.Type, result.Data.ActivityType);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_GivenFirstQuestionScreenActivityIsSkipped(
           [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
           [Frozen] Mock<IStartsWorkflow> startsWorkflow,
           [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
           [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
           [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
           WorkflowBlueprint workflowBlueprint,
           ActivityBlueprint activityBlueprint,
           ActivityBlueprint nextActivityBlueprint,
           RunWorkflowResult runWorkflowResult,
           StartWorkflowCommand startWorkflowCommand,
           CustomActivityNavigation customActivityNavigation,
           List<Question> questions,
           WorkflowInstance workflowInstance,
           StartWorkflowCommandHandler sut)
        {

            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = nextActivityBlueprint,
                WorkflowInstance = workflowInstance
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

            workflowNextActivityProvider
                .Setup(x => x.GetStartWorkflowNextActivity(activityBlueprint, runWorkflowResult.WorkflowInstance.Id,
                    CancellationToken.None)).ReturnsAsync(workflowNextActivityModel);

            elsaCustomRepository
                .Setup(x => x.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                    workflowInstance.Id, CancellationToken.None)).ReturnsAsync(customActivityNavigation);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(workflowNextActivityModel.NextActivity.Id, customActivityNavigation,
                    workflowNextActivityModel.NextActivity, workflowNextActivityModel.WorkflowInstance, CancellationToken.None), Times.Once);
            Assert.Equal(workflowNextActivityModel.WorkflowInstance.Id, result.Data!.WorkflowInstanceId);
            Assert.Equal(workflowNextActivityModel.NextActivity.Id, result.Data.NextActivityId);
            Assert.Equal(workflowNextActivityModel.NextActivity.Type, result.Data.ActivityType);
            Assert.Equal(workflowBlueprint.Name, result.Data.WorkflowName);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnDataFromCorrectWorkflowInstance(
           [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
           [Frozen] Mock<IStartsWorkflow> startsWorkflow,
           [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
           [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
           [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
           WorkflowBlueprint workflowBlueprint,
           ActivityBlueprint activityBlueprint,
           RunWorkflowResult runWorkflowResult,
           StartWorkflowCommand startWorkflowCommand,
           CustomActivityNavigation customActivityNavigation,
           List<Question> questions,
           WorkflowInstance workflowInstance,
           StartWorkflowCommandHandler sut)
        {

            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint,
                WorkflowInstance = workflowInstance
            };

            activityBlueprint.Id = runWorkflowResult.WorkflowInstance!.LastExecutedActivityId!;
            activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
            workflowBlueprint.Activities.Add(activityBlueprint);

            var assessmentQuestionsDictionary = new Dictionary<string, object?>();
            var assessmentQuestions = new AssessmentQuestions() { Questions = questions };
            assessmentQuestionsDictionary.Add("Questions", assessmentQuestions);

            workflowInstance.ActivityData.Add(activityBlueprint.Id, assessmentQuestionsDictionary);

            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, startWorkflowCommand.CorrelationId, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            workflowNextActivityProvider
                .Setup(x => x.GetStartWorkflowNextActivity(activityBlueprint, runWorkflowResult.WorkflowInstance.Id,
                    CancellationToken.None)).ReturnsAsync(workflowNextActivityModel);

            elsaCustomRepository
                .Setup(x => x.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                    workflowInstance.Id, CancellationToken.None)).ReturnsAsync(customActivityNavigation);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(workflowNextActivityModel.NextActivity.Id, customActivityNavigation,
                    workflowNextActivityModel.NextActivity, workflowNextActivityModel.WorkflowInstance, CancellationToken.None), Times.Once);
            Assert.Equal(workflowNextActivityModel.WorkflowInstance.Id, result.Data!.WorkflowInstanceId);
            Assert.Equal(workflowNextActivityModel.NextActivity.Id, result.Data.NextActivityId);
            Assert.Equal(workflowNextActivityModel.NextActivity.Type, result.Data.ActivityType);
            Assert.Equal(workflowBlueprint.Name, result.Data.WorkflowName);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnDataFromOriginalWorkflowInstance_GivenWorkflowNextActivityHasNullWorkflowInstance(
           [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
           [Frozen] Mock<IStartsWorkflow> startsWorkflow,
           [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
           [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
           [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
           WorkflowBlueprint workflowBlueprint,
           ActivityBlueprint activityBlueprint,
           RunWorkflowResult runWorkflowResult,
           StartWorkflowCommand startWorkflowCommand,
           CustomActivityNavigation customActivityNavigation,
           List<Question> questions,
           StartWorkflowCommandHandler sut)
        {

            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint,
                WorkflowInstance = (WorkflowInstance?)null
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

            workflowNextActivityProvider
                .Setup(x => x.GetStartWorkflowNextActivity(activityBlueprint, runWorkflowResult.WorkflowInstance.Id,
                    CancellationToken.None)).ReturnsAsync(workflowNextActivityModel);

            elsaCustomRepository
                .Setup(x => x.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                    runWorkflowResult.WorkflowInstance.Id, CancellationToken.None)).ReturnsAsync(customActivityNavigation);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(workflowNextActivityModel.NextActivity.Id, customActivityNavigation,
                    workflowNextActivityModel.NextActivity, runWorkflowResult.WorkflowInstance, CancellationToken.None), Times.Once);
            Assert.Equal(runWorkflowResult.WorkflowInstance.Id, result.Data!.WorkflowInstanceId);
            Assert.Equal(workflowNextActivityModel.NextActivity.Id, result.Data.NextActivityId);
            Assert.Equal(workflowNextActivityModel.NextActivity.Type, result.Data.ActivityType);
            Assert.Equal(workflowBlueprint.Name, result.Data.WorkflowName);
            Assert.Empty(result.ErrorMessages);
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

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_WhenGetCustomNavigationReturnsNull(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            [Frozen] Mock<IStartsWorkflow> startsWorkflow,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
            [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
            WorkflowBlueprint workflowBlueprint,
            ActivityBlueprint activityBlueprint,
            RunWorkflowResult runWorkflowResult,
            StartWorkflowCommand startWorkflowCommand,
            WorkflowInstance workflowInstance,
            StartWorkflowCommandHandler sut)
        {
            //Arrange
            var workflowNextActivityModel = new WorkflowNextActivityModel
            {
                NextActivity = activityBlueprint,
                WorkflowInstance = workflowInstance
            };

            activityBlueprint.Id = runWorkflowResult.WorkflowInstance!.LastExecutedActivityId!;
            workflowBlueprint.Activities.Add(activityBlueprint);

            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, startWorkflowCommand.CorrelationId, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            workflowNextActivityProvider
                .Setup(x => x.GetStartWorkflowNextActivity(activityBlueprint, runWorkflowResult.WorkflowInstance.Id,
                    CancellationToken.None)).ReturnsAsync(workflowNextActivityModel);
            elsaCustomRepository
                .Setup(x => x.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id,
                    workflowInstance.Id, CancellationToken.None)).ReturnsAsync((CustomActivityNavigation?)null);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(workflowNextActivityModel.NextActivity.Id, null,
                    workflowNextActivityModel.NextActivity, workflowNextActivityModel.WorkflowInstance, CancellationToken.None), Times.Once);
            Assert.Equal(workflowNextActivityModel.NextActivity.Id, result.Data!.NextActivityId);
            Assert.Equal(workflowBlueprint.Name, result.Data.WorkflowName);
            Assert.Equal(workflowInstance.Id, result.Data.WorkflowInstanceId);
            Assert.Equal(workflowNextActivityModel.NextActivity.Type, result.Data.ActivityType);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }
    }
}
