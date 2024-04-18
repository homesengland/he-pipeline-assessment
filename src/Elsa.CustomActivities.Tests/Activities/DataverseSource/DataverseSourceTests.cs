using AutoFixture.Xunit2;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.SinglePipeline;
using Moq;
using Xunit;
using He.PipelineAssessment.Data.Dataverse;
using Elsa.CustomActivities.Activities.DataverseDataSource;
using System.ServiceModel;

namespace Elsa.CustomActivities.Tests.Activities.SinglePipelineDataSource
{
    public class DataverseSourceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecute_WithSuccessfulData_ReturnsOutcomeResult(
            [Frozen] Mock<IDataverseClient> clientMock,
            DataverseResults singlePipelineData,
            DataverseDataSource dataSourceActivity)
        {
            //Arrange
            //string emptyFetxhXmlQuery = "<fetch><entity name=\"he_pipeline\"></entity></fetch>";
            clientMock.Setup(x => x.RunFetchXML(dataSourceActivity.FetchXML))
                .Returns(singlePipelineData);
            var context = new ActivityExecutionContext(default!, default!, default!, null, default, default);

            //Act
            var result = await dataSourceActivity.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);
            var combinedResult = (CombinedResult)result;
            Assert.Equal(2, combinedResult.Results.Count);
            var outcomeResult = (OutcomeResult)combinedResult.Results.First(x => x.GetType() == typeof(OutcomeResult));
            Assert.Equal("Done", outcomeResult.Outcomes.First());
            Assert.Contains(combinedResult.Results, x => x.GetType() == typeof(SuspendResult));
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecute_ReturnsSuspendResult(
            [Frozen] Mock<IDataverseClient> clientMock,
            DataverseResults singlePipelineData,
            DataverseDataSource dataSourceActivity)
        {
            //Arrange
            clientMock.Setup(x => x.RunFetchXML(dataSourceActivity.FetchXML))
                .Returns(singlePipelineData);
            var context = new ActivityExecutionContext(default!, default!, default!, null, default, default);
            dataSourceActivity.RunAsNonBlockingActivity = false;

            //Act
            var result = await dataSourceActivity.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            var suspendResult = (SuspendResult)result;
            Assert.IsType<SuspendResult>(suspendResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecute_ReturnsResult(
            [Frozen] Mock<IDataverseClient> clientMock,
            DataverseResults singlePipelineData,
            DataverseDataSource dataSourceActivity)
        {
            //Arrange
            clientMock.Setup(x => x.RunFetchXML(dataSourceActivity.FetchXML))
                .Returns(singlePipelineData);
            var context = new ActivityExecutionContext(default!, default!, default!, null, default, default);
            dataSourceActivity.RunAsNonBlockingActivity = true;

            //Act
            var result = await dataSourceActivity.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            var outcomeResult = (OutcomeResult)result;
            Assert.Equal("Done", outcomeResult.Outcomes.First());
        }

        [Theory]
        [AutoMoqData]
        public void OnExecute_ReturnsException(
            [Frozen] Mock<IDataverseClient> clientMock,
            DataverseDataSource dataSourceActivity)
        {
            //Arrange
            clientMock.Setup(x => x.RunFetchXML(dataSourceActivity.FetchXML))
                .Throws<FaultException>();
            var context = new ActivityExecutionContext(default!, default!, default!, null, default, default);
            dataSourceActivity.RunAsNonBlockingActivity = true;

            //Act and Assert
            Assert.Throws<FaultException>(()=>
                dataSourceActivity.ExecuteAsync(context).Result
            );
        }
    }
}
