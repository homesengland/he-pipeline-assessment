using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Providers;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
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
            Assert.Equal(activityBlueprint.Id, result.Id);
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
            Assert.Equal(activityBlueprint.Id, result.Id);
        }

        [Fact]

        public void GetNextActivity_ShouldReturnNextActivity_GivenQuestionScreenActivityTypeAndConditionReturnsTrue()
        {
            Assert.False(true);
        }

        [Fact]
        public void GetNextActivity_ShouldNotReturnNextActivity_GivenQuestionScreenActivityTypeAndConditionReturnsFalse()
        {
            Assert.False(true);
        }

        [Fact]
        public void GetNextActivity_ShouldThrowException_GivenNoValidNextActivityFound()
        {
            Assert.False(true);
        }

        [Fact]
        public void GetStartWorkflowNextActivity_ShouldReturnSameActivity_GivenConditionReturnsTrue()
        {
            Assert.False(true);
        }

        [Fact]
        public void GetStartWorkflowNextActivity_ShouldThrowException_GivenNoCollectedWorkflows()
        {
            Assert.False(true);
        }

        [Fact]
        public void GetStartWorkflowNextActivity_ShouldReturnNextValidQuestionActivityType_GivenCurrentConditionFalse()
        {
            Assert.False(true);
        }

        [Fact]
        public void GetStartWorkflowNextActivity_ShouldReturnNextActivityType_GivenCurrentConditionFalseAndNoQuestionActivityType()
        {
            Assert.False(true);
        }

        [Fact]
        public void GetStartWorkflowNextActivity_ShouldThrowException_GivenNoValidNextActivityFound()
        {
            Assert.False(true);
        }
    }
}
