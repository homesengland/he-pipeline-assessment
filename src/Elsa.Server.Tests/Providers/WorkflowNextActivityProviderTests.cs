using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
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
            string activityId,
            string workflowInstanceId,
            List<QuestionScreenAnswer>? questionScreenAnswers,
            WorkflowNextActivityProvider sut)
        {
            //Arrange
            questionInvoker.Setup(x => x.ExecuteWorkflowsAsync(activityId,
                    ActivityTypeConstants.QuestionScreen,
                    workflowInstanceId, questionScreenAnswers, CancellationToken.None))
                .ReturnsAsync(new List<CollectedWorkflow>());

            //Act
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => sut.GetNextActivity(activityId, workflowInstanceId, questionScreenAnswers, ActivityTypeConstants.QuestionScreen, CancellationToken.None));

            //Assert
            Assert.Equal("true", exception.Message);
        }

        [Theory]
        [AutoMoqData]
        public void GetNextActivity_ShouldReturnNextActivity_GivenNotQuestionScreenActivityType()
        {
            Assert.False(true);
        }

        [Theory]
        [AutoMoqData]
        public void GetNextActivity_ShouldReturnNextActivity_GivenQuestionScreenActivityTypeAndConditionReturnsTrue()
        {
            Assert.False(true);
        }

        [Theory]
        [AutoMoqData]
        public void GetNextActivity_ShouldNotReturnNextActivity_GivenQuestionScreenActivityTypeAndConditionReturnsFalse()
        {
            Assert.False(true);
        }

        [Theory]
        [AutoMoqData]
        public void GetNextActivity_ShouldThrowException_GivenNoValidNextActivityFound()
        {
            Assert.False(true);
        }

        [Theory]
        [AutoMoqData]
        public void GetStartWorkflowNextActivity_ShouldReturnSameActivity_GivenConditionReturnsTrue()
        {
            Assert.False(true);
        }

        [Theory]
        [AutoMoqData]
        public void GetStartWorkflowNextActivity_ShouldThrowException_GivenNoCollectedWorkflows()
        {
            Assert.False(true);
        }

        [Theory]
        [AutoMoqData]
        public void GetStartWorkflowNextActivity_ShouldReturnNextValidQuestionActivityType_GivenCurrentConditionFalse()
        {
            Assert.False(true);
        }

        [Theory]
        [AutoMoqData]
        public void GetStartWorkflowNextActivity_ShouldReturnNextActivityType_GivenCurrentConditionFalseAndNoQuestionActivityType()
        {
            Assert.False(true);
        }

        [Theory]
        [AutoMoqData]
        public void GetStartWorkflowNextActivity_ShouldThrowException_GivenNoValidNextActivityFound()
        {
            Assert.False(true);
        }
    }
}
