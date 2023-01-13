using AutoFixture.Xunit2;
using Elsa.Models;
using Elsa.Server.Providers;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Providers
{
   
    public class NextWorkflowDefinitionProviderTests
    {
        [Theory]
        [AutoMoqData]
        public void GetWorkflowDefinitionIds_ReturnsNextWorkflowDefinitionIds_GivenTheyExistinActivityData
            (
                [Frozen] Mock<IActivityDataProvider> activityDataProviderMock,
                WorkflowInstance workflowInstance,
                string activityId,
                string nextWorkflowDefinitionIds,
                NextWorkflowDefinitionProvider sut
            )
        {

            //Arrange
            var dictionary = new Dictionary<string, object?>() 
            {
                {"NextWorkflowDefinitionIds",nextWorkflowDefinitionIds }
            };
            activityDataProviderMock.Setup(x => x.GetActivityData(workflowInstance, activityId)).Returns(dictionary);

            //Act

            var results = sut.GetNextWorkflowDefinitionIds(workflowInstance, activityId);

            //Assert
            Assert.Equal(nextWorkflowDefinitionIds, results);
        }


        [Theory]
        [AutoMoqData]
        public void GetWorkflowDefinitionIds_ReturnsEmptyNextWorkflowDefinitionIds_GivenTheyDoNotExistinActivityData
            (
                [Frozen] Mock<IActivityDataProvider> activityDataProviderMock,
                WorkflowInstance workflowInstance,
                string activityId,
                string nextWorkflowDefinitionIds,
                NextWorkflowDefinitionProvider sut
            )
        {

            //Arrange
            var dictionary = new Dictionary<string, object?>()
            {
                {"NotNextWorkflowDefinitionIds",nextWorkflowDefinitionIds }
            };
            activityDataProviderMock.Setup(x => x.GetActivityData(workflowInstance, activityId)).Returns(dictionary);

            //Act

            var results = sut.GetNextWorkflowDefinitionIds(workflowInstance, activityId);

            //Assert
            Assert.Equal(string.Empty, results);

        }

        [Theory]
        [AutoMoqData]
        public void GetWorkflowDefinitionIds_ReturnsEmptyNextWorkflowDefinitionIds_GivenActivityDataDictionaryObjectisNull
           (
               [Frozen] Mock<IActivityDataProvider> activityDataProviderMock,
               WorkflowInstance workflowInstance,
               string activityId,
               NextWorkflowDefinitionProvider sut
           )
        {

            //Arrange
            var dictionary = new Dictionary<string, object?>()
            {
                {"NextWorkflowDefinitionIds", null }
            };
            activityDataProviderMock.Setup(x => x.GetActivityData(workflowInstance, activityId)).Returns(dictionary);

            //Act

            var results = sut.GetNextWorkflowDefinitionIds(workflowInstance, activityId);

            //Assert
            Assert.Null(results);

        }        
    }
}
