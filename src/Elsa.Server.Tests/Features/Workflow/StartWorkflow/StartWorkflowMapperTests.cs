using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Providers;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.StartWorkflow
{
    public class StartWorkflowMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void RunWorkflowResultToCustomNavigationActivity_ShouldReturnCustomNavigationActivity_WhenWorkflowInstanceIsNotNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            RunWorkflowResult runWorkflowResult,
            string activityType,
            StartWorkflowMapper sut
            )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.RunWorkflowResultToCustomNavigationActivity(runWorkflowResult, activityType);

            //Assert
            Assert.IsType<CustomActivityNavigation>(result);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.LastExecutedActivityId, result!.ActivityId);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.Id, result.WorkflowInstanceId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }

        [Theory]
        [AutoData]
        public void RunWorkflowResultToCustomNavigationActivity_ShouldReturnNull_WhenWorkflowInstanceNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string activityType
            )
        {
            //Arrange
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);
            var runWorkflowResult = new RunWorkflowResult(null, null, null, false);

            //Act
            var result = sut.RunWorkflowResultToCustomNavigationActivity(runWorkflowResult, activityType);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoData]
        public void RunWorkflowResultToQuestionScreenAnswer_WithQuestionParameter_ShouldReturnNull_WhenWorkflowInstanceIdNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string activityType,
            Question question)
        {
            //Arrange
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);

            var workflowInstance = new Elsa.Models.WorkflowInstance();
            var runWorkflowResult = new RunWorkflowResult(workflowInstance, null, null, false);

            //Act
            var result = sut.RunWorkflowResultToQuestionScreenAnswer(runWorkflowResult, activityType, question);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void RunWorkflowResultToQuestionScreenAnswer_WithQuestionParameter_ShouldReturnQuestionScreenAnswer_WhenWorkflowInstanceIsNotNull(
           [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
           RunWorkflowResult runWorkflowResult,
           string activityType,
           Question question,
           StartWorkflowMapper sut
           )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.RunWorkflowResultToQuestionScreenAnswer(runWorkflowResult, activityType, question);

            //Assert
            Assert.IsType<QuestionScreenAnswer>(result);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.LastExecutedActivityId, result!.ActivityId);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.Id, result.WorkflowInstanceId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }

        [Theory]
        [AutoData]
        public void RunWorkflowResultToQuestionScreenAnswer_WithQuestionParameter_ShouldReturnNull_WhenWorkflowInstanceNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            string activityType,
            Question question
            )
        {
            //Arrange
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);
            var runWorkflowResult = new RunWorkflowResult(null, null, null, false);

            //Act
            var result = sut.RunWorkflowResultToQuestionScreenAnswer(runWorkflowResult, activityType, question);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoData]
        public void RunWorkflowResultToCustomNavigationActivity_ShouldReturnNull_WhenWorkflowInstanceIdNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider, string activityType)
        {
            //Arrange
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);

            var workflowInstance = new Elsa.Models.WorkflowInstance();
            var runWorkflowResult = new RunWorkflowResult(workflowInstance, null, null, false);

            //Act
            var result = sut.RunWorkflowResultToCustomNavigationActivity(runWorkflowResult, activityType);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void RunWorkflowResultToStartWorkflowResponse_ShouldReturnStartWorkflowResponse_WhenWorkflowInstanceIsNotNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            RunWorkflowResult runWorkflowResult
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);


            //Act
            var result = sut.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult, "", "");

            //Assert
            Assert.IsType<StartWorkflowResponse>(result);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.LastExecutedActivityId, result!.NextActivityId);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.Id, result.WorkflowInstanceId);
        }

        [Theory]
        [AutoData]
        public void RunWorkflowResultToStartWorkflowResponse_ShouldReturnNull_WhenWorkflowInstanceNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider
        )
        {
            //Arrange
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);
            var runWorkflowResult = new RunWorkflowResult(null, null, null, false);

            //Act
            var result = sut.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult, "", "");

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoData]
        public void RunWorkflowResultToStartWorkflowResponse_ShouldReturnNull_WhenWorkflowInstanceIdNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            //Arrange
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);

            var workflowInstance = new Elsa.Models.WorkflowInstance();
            var runWorkflowResult = new RunWorkflowResult(workflowInstance, null, null, false);

            //Act
            var result = sut.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult, "", "");

            //Assert
            Assert.Null(result);
        }


    }
}
