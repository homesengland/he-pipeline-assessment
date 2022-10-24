using AutoFixture.Xunit2;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Features.Workflow.SubmitAssessmentStage;
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
        public void RunWorkflowResultToAssessmentQuestion_ShouldReturnAssessmentQuestion_WhenWorkflowInstanceIsNotNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            RunWorkflowResult runWorkflowResult,
            ActivityBlueprint activity,
            string workflowName,
            StartWorkflowMapper sut
            )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.RunWorkflowResultToAssessmentQuestion(runWorkflowResult, activity, workflowName);

            //Assert
            Assert.IsType<AssessmentQuestion>(result);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.LastExecutedActivityId, result!.ActivityId.ToString());
            Assert.Equal(runWorkflowResult.WorkflowInstance!.Id, result.WorkflowInstanceId.ToString());
            Assert.Equal(activity.Type, result.ActivityType);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.LastExecutedActivityId, result.PreviousActivityId.ToString());
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }

        [Theory]
        [AutoData]
        public void RunWorkflowResultToAssessmentQuestion_ShouldReturnNull_WhenWorkflowInstanceNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            ActivityBlueprint activity,
            string workflowName
            )
        {
            //Arrange
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);
            var runWorkflowResult = new RunWorkflowResult(null, null, null, false);

            //Act
            var result = sut.RunWorkflowResultToAssessmentQuestion(runWorkflowResult, activity, workflowName);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoData]
        public void RunWorkflowResultToAssessmentQuestion_ShouldReturnNull_WhenWorkflowInstanceIdNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            ActivityBlueprint activity,
            string workflowName)
        {
            //Arrange
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);

            var workflowInstance = new Elsa.Models.WorkflowInstance();
            var runWorkflowResult = new RunWorkflowResult(workflowInstance, null, null, false);

            //Act
            var result = sut.RunWorkflowResultToAssessmentQuestion(runWorkflowResult, activity, workflowName);

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
            var result = sut.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult);

            //Assert
            Assert.IsType<SubmitAssessmentStageResponse>(result);
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
            var result = sut.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult);

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
            var result = sut.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult);

            //Assert
            Assert.Null(result);
        }
    }
}
