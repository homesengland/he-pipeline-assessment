using AutoFixture.Xunit2;
using Elsa.Models;
using Elsa.Server.Providers;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Providers
{
    public class ActivityDataProviderTests
    {

        [Theory]
        [AutoMoqData]
        public async Task
            GetActivityData_ShouldThrowException_WhenActivityDataCannotBeFound(
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProviderMock,
            string workflowInstanceId,
            string activityId,
            CancellationToken cancellationToken,
            WorkflowInstance workflowInstance,
            ActivityDataProvider sut)
        {
            // Arrange
            workflowInstanceProviderMock.Setup(x => x.GetWorkflowInstance(workflowInstanceId, cancellationToken)).ReturnsAsync(workflowInstance);

            //Act

            var exception = await Assert.ThrowsAsync<Exception>(() => sut.GetActivityData(workflowInstanceId, activityId, cancellationToken));

            //Assert
            Assert.Equal($"Cannot find activity data with id {activityId}.", exception.Message);
        }


        [Theory]
        [AutoMoqData]
        public async Task GetActivityData_ReturnsActivity_WhenActivityDataFound(
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProviderMock,
            string workflowInstanceId,
            string activityId,
            CancellationToken cancellationToken,
            WorkflowInstance workflowInstance,
            ActivityDataProvider sut)
        {
            // Arrange
            workflowInstanceProviderMock.Setup(x => x.GetWorkflowInstance(workflowInstanceId, cancellationToken)).ReturnsAsync(workflowInstance);
            IDictionary<string, object?> activityData = new Dictionary<string, object?>();

            var assessmentQuestionsDictionary = new Dictionary<string, object?>();
            assessmentQuestionsDictionary.Add("title", "test");

            workflowInstance.ActivityData.Add(activityId, assessmentQuestionsDictionary);

            //Act
            activityData = await sut.GetActivityData(workflowInstanceId, activityId, cancellationToken);

            //Assert
            Assert.Equal(1, activityData.Count);
            Assert.Equal("test", activityData["title"]);
        }


        [Theory]
        [AutoMoqData]
        public void GetActivityData_ReturnsActivity_GivenWorkFlowInstance
          (  
           string activityId,          
           WorkflowInstance workflowInstance,
           ActivityDataProvider sut
          )
        {
            // Arrange           
            var assessmentQuestionsDictionary = new Dictionary<string, object?>();
            assessmentQuestionsDictionary.Add("title", "test");

            workflowInstance.ActivityData.Add(activityId, assessmentQuestionsDictionary);

            //Act
           var activityData =  sut.GetActivityData(workflowInstance, activityId);

            //Assert
            Assert.Equal(1, activityData.Count);
            Assert.Equal("test", activityData["title"]);
        }
    }
}
