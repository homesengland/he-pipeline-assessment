using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Providers;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Providers
{
    public class WorkflowNextActivityProviderTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetNextActivity_ShouldThrowException_GivenNoCollectedWorkflows(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            string activityId,
            string workflowInstanceId,
            WorkflowInstance workflowInstance,
            List<QuestionScreenAnswer>? questionScreenAnswers,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(activityId,
                    ActivityTypeConstants.QuestionScreen,
                    workflowInstanceId, questionScreenAnswers, CancellationToken.None))
                .ReturnsAsync(new List<CollectedWorkflow>());

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(workflowInstanceId, CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            //Act
            var exception = await Assert.ThrowsAsync<Exception>(() => sut.GetNextActivity(activityId, workflowInstanceId, questionScreenAnswers, ActivityTypeConstants.QuestionScreen, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to progress workflow. Workflow status is: {workflowInstance.WorkflowStatus}", exception.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNextActivity_ShouldReturnNextActivity_GivenNotQuestionScreenActivityType(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<IWorkflowRegistryProvider> workflowRegistryProvider,
            string activityId,
            string workflowInstanceId,
            WorkflowInstance workflowInstance,
            List<CollectedWorkflow> collectedWorkflows,
            ActivityBlueprint activityBlueprint,
            List<QuestionScreenAnswer>? questionScreenAnswers,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(activityId,
                    ActivityTypeConstants.QuestionScreen,
                    workflowInstanceId, questionScreenAnswers, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(workflowInstanceId, CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowRegistryProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(activityBlueprint);

            //Act
            var result = await sut.GetNextActivity(activityId, workflowInstanceId, questionScreenAnswers, ActivityTypeConstants.QuestionScreen, CancellationToken.None);

            //Assert
            Assert.Equal(activityBlueprint.Id, result.NextActivity.Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNextActivity_ShouldReturnNextActivity_GivenNextActivityIsSameAsCurrentActivity(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<IWorkflowRegistryProvider> workflowRegistryProvider,
            string activityId,
            string workflowInstanceId,
            WorkflowInstance workflowInstance,
            List<CollectedWorkflow> collectedWorkflows,
            ActivityBlueprint activityBlueprint,
            List<QuestionScreenAnswer>? questionScreenAnswers,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            activityBlueprint.Id = activityId;

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(activityId,
                    ActivityTypeConstants.QuestionScreen,
                    workflowInstanceId, questionScreenAnswers, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(workflowInstanceId, CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowRegistryProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(activityBlueprint);

            //Act
            var result = await sut.GetNextActivity(activityId, workflowInstanceId, questionScreenAnswers, ActivityTypeConstants.QuestionScreen, CancellationToken.None);

            //Assert
            Assert.Equal(activityBlueprint.Id, result.NextActivity.Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNextActivity_ShouldReturnNextActivity_GivenQuestionScreenActivityTypeAndConditionReturnsTrue(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<IWorkflowRegistryProvider> workflowRegistryProvider,
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            string activityId,
            string workflowInstanceId,
            WorkflowInstance workflowInstance,
            List<CollectedWorkflow> collectedWorkflows,
            ActivityBlueprint activityBlueprint,
            List<QuestionScreenAnswer>? questionScreenAnswers,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(activityId,
                    ActivityTypeConstants.QuestionScreen,
                    workflowInstanceId, questionScreenAnswers, CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(workflowInstanceId, CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowRegistryProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(activityBlueprint);

            var dictionary = new Dictionary<string, object?>()
            {
                { "Condition", true }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstance.Id, activityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(dictionary);

            //Act
            var result = await sut.GetNextActivity(activityId, workflowInstanceId, questionScreenAnswers, ActivityTypeConstants.QuestionScreen, CancellationToken.None);

            //Assert
            Assert.Equal(activityBlueprint.Id, result.NextActivity.Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNextActivity_ShouldNotReturnNextActivity_GivenQuestionScreenActivityTypeAndConditionReturnsFalse(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<IWorkflowRegistryProvider> workflowRegistryProvider,
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            string activityId,
            string workflowInstanceId,
            WorkflowInstance workflowInstance,
            List<CollectedWorkflow> collectedWorkflows,
            ActivityBlueprint activityBlueprint,
            ActivityBlueprint anotherActivityBlueprint,
            List<QuestionScreenAnswer>? questionScreenAnswers,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(It.IsAny<string>(),
                    ActivityTypeConstants.QuestionScreen,
                    workflowInstanceId, It.IsAny<List<QuestionScreenAnswer>?>(), CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(workflowInstanceId, CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowRegistryProvider.SetupSequence(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(activityBlueprint)
                .ReturnsAsync(anotherActivityBlueprint);

            var dictionary = new Dictionary<string, object?>()
            {
                { "Condition", false }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstance.Id, activityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(dictionary);

            var anotherDictionary = new Dictionary<string, object?>()
            {
                { "Condition", true }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstance.Id, anotherActivityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(anotherDictionary);

            //Act
            var result = await sut.GetNextActivity(activityId, workflowInstanceId, questionScreenAnswers, ActivityTypeConstants.QuestionScreen, CancellationToken.None);

            //Assert
            Assert.Equal(anotherActivityBlueprint.Id, result.NextActivity.Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNextActivity_ShouldNotReturnNextActivity_GivenQuestionScreenActivityTypeAndConditionDoesNotHaveValue(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<IWorkflowRegistryProvider> workflowRegistryProvider,
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            string activityId,
            string workflowInstanceId,
            WorkflowInstance workflowInstance,
            List<CollectedWorkflow> collectedWorkflows,
            ActivityBlueprint activityBlueprint,
            ActivityBlueprint anotherActivityBlueprint,
            List<QuestionScreenAnswer>? questionScreenAnswers,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(It.IsAny<string>(),
                    ActivityTypeConstants.QuestionScreen,
                    workflowInstanceId, It.IsAny<List<QuestionScreenAnswer>?>(), CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(workflowInstanceId, CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowRegistryProvider.SetupSequence(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(activityBlueprint)
                .ReturnsAsync(anotherActivityBlueprint);

            var dictionary = new Dictionary<string, object?>()
            {
                { "Condition", null }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstance.Id, activityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(dictionary);

            var anotherDictionary = new Dictionary<string, object?>()
            {
                { "Condition", true }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstance.Id, anotherActivityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(anotherDictionary);

            //Act
            var result = await sut.GetNextActivity(activityId, workflowInstanceId, questionScreenAnswers, ActivityTypeConstants.QuestionScreen, CancellationToken.None);

            //Assert
            Assert.Equal(anotherActivityBlueprint.Id, result.NextActivity.Id);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNextActivity_ShouldThrowException_GivenDependencyThrows(
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            string activityId,
            string workflowInstanceId,
            Exception exception,
            List<QuestionScreenAnswer>? questionScreenAnswers,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            questionInvoker
                .Setup(x => x.ExecuteWorkflowsAsync(It.IsAny<string>(),
                    ActivityTypeConstants.QuestionScreen,
                    workflowInstanceId, It.IsAny<List<QuestionScreenAnswer>?>(), CancellationToken.None))
                .Throws(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.GetNextActivity(activityId, workflowInstanceId,
                questionScreenAnswers, ActivityTypeConstants.QuestionScreen, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStartWorkflowNextActivity_ShouldReturnSameActivity_GivenConditionReturnsTrue(
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            string workflowInstanceId,
            ActivityBlueprint activityBlueprint,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            var dictionary = new Dictionary<string, object?>()
            {
                { "Condition", true }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstanceId, activityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(dictionary);

            //Act
            var result = await sut.GetStartWorkflowNextActivity(activityBlueprint, workflowInstanceId, CancellationToken.None);

            //Assert
            Assert.Equal(activityBlueprint, result.NextActivity);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStartWorkflowNextActivity_ShouldThrowException_GivenNoCollectedWorkflows(
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            string workflowInstanceId,
            ActivityBlueprint activityBlueprint,
            WorkflowInstance workflowInstance,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            var dictionary = new Dictionary<string, object?>()
            {
                { "Condition", false }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstanceId, activityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(dictionary);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(activityBlueprint.Id,
                    activityBlueprint.Type,
                    workflowInstanceId, null, CancellationToken.None))
                .ReturnsAsync(new List<CollectedWorkflow>());

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(workflowInstanceId, CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            //Act
            var exception = await Assert.ThrowsAsync<Exception>(() => sut.GetStartWorkflowNextActivity(activityBlueprint, workflowInstanceId, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to progress workflow. Workflow status is: {workflowInstance.WorkflowStatus}", exception.Message);

        }

        [Theory]
        [AutoMoqData]
        public async Task GetStartWorkflowNextActivity_ShouldReturnNextValidQuestionActivityType_GivenCurrentConditionFalse(
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<IWorkflowRegistryProvider> workflowRegistryProvider,
            string workflowInstanceId,
            ActivityBlueprint activityBlueprint,
            ActivityBlueprint anotherActivityBlueprint,
            WorkflowInstance workflowInstance,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            anotherActivityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
            var dictionary = new Dictionary<string, object?>()
            {
                { "Condition", false }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstanceId, activityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(dictionary);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(It.IsAny<string>(),
                    It.IsAny<string>(),
                    workflowInstanceId, It.IsAny<List<QuestionScreenAnswer>?>(), CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(workflowInstanceId, CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowRegistryProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(anotherActivityBlueprint);

            var anotherDictionary = new Dictionary<string, object?>()
            {
                { "Condition", true }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstance.Id, anotherActivityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(anotherDictionary);

            //Act
            var result = await sut.GetStartWorkflowNextActivity(activityBlueprint, workflowInstanceId, CancellationToken.None);

            //Assert
            Assert.Equal(anotherActivityBlueprint, result.NextActivity);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStartWorkflowNextActivity_ShouldReturnNextValidQuestionActivityType_GivenCurrentConditionNotSet(
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<IWorkflowRegistryProvider> workflowRegistryProvider,
            string workflowInstanceId,
            ActivityBlueprint activityBlueprint,
            ActivityBlueprint anotherActivityBlueprint,
            WorkflowInstance workflowInstance,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            anotherActivityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
            var dictionary = new Dictionary<string, object?>()
            {
                { "Condition", null }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstanceId, activityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(dictionary);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(It.IsAny<string>(),
                    It.IsAny<string>(),
                    workflowInstanceId, It.IsAny<List<QuestionScreenAnswer>?>(), CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(workflowInstanceId, CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowRegistryProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(anotherActivityBlueprint);

            var anotherDictionary = new Dictionary<string, object?>()
            {
                { "Condition", true }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstance.Id, anotherActivityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(anotherDictionary);

            //Act
            var result = await sut.GetStartWorkflowNextActivity(activityBlueprint, workflowInstanceId, CancellationToken.None);

            //Assert
            Assert.Equal(anotherActivityBlueprint, result.NextActivity);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStartWorkflowNextActivity_ShouldReturnNextActivityType_GivenCurrentConditionFalseAndNoQuestionActivityType(
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            [Frozen] Mock<IQuestionInvoker> questionInvoker,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<IWorkflowRegistryProvider> workflowRegistryProvider,
            string workflowInstanceId,
            ActivityBlueprint activityBlueprint,
            ActivityBlueprint anotherActivityBlueprint,
            WorkflowInstance workflowInstance,
            List<CollectedWorkflow> collectedWorkflows,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            anotherActivityBlueprint.Type = "NotQuestionScreen";
            var dictionary = new Dictionary<string, object?>()
            {
                { "Condition", null }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstanceId, activityBlueprint.Id, CancellationToken.None))
                .ReturnsAsync(dictionary);

            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(It.IsAny<string>(),
                    It.IsAny<string>(),
                    workflowInstanceId, It.IsAny<List<QuestionScreenAnswer>?>(), CancellationToken.None))
                .ReturnsAsync(collectedWorkflows);

            workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(workflowInstanceId, CancellationToken.None))
                .ReturnsAsync(workflowInstance);

            workflowRegistryProvider.Setup(x => x.GetNextActivity(workflowInstance, CancellationToken.None))
                .ReturnsAsync(anotherActivityBlueprint);

            //Act
            var result = await sut.GetStartWorkflowNextActivity(activityBlueprint, workflowInstanceId, CancellationToken.None);

            //Assert
            Assert.Equal(anotherActivityBlueprint, result.NextActivity);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetStartWorkflowNextActivity_ShouldThrowException_GivenDependencyThrows(
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            IActivityBlueprint activityBlueprint,
            string workflowInstanceId,
            Exception exception,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            activityDataProvider
                .Setup(x => x.GetActivityData(workflowInstanceId, activityBlueprint.Id, CancellationToken.None))
                .Throws(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.GetStartWorkflowNextActivity(activityBlueprint, workflowInstanceId, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }
    }
}
