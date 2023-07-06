using AutoFixture.Xunit2;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.SinglePipeline;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.SinglePipelineDataSource
{
    public class SinglePipelineDataSourceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecute_WithSuccessfulData_ReturnsOutcomeResult(
            [Frozen] Mock<IEsriSinglePipelineClient> pipelineMock,
            [Frozen] Mock<IEsriSinglePipelineDataJsonHelper> jsonHelperMock,
            string singlePipelineString,
            SinglePipelineData singlePipelineData,
            CustomActivities.Activities.SinglePipelineDataSource.SinglePipelineDataSource sut)
        {
            //Arrange
            pipelineMock.Setup(x => x.GetSinglePipelineData(sut.SpId))
                .ReturnsAsync(singlePipelineString);

            jsonHelperMock.Setup(x => x.JsonToSinglePipelineData(singlePipelineString))
                .Returns(singlePipelineData);
            singlePipelineData.multi_local_authority = "[{\"la_name\":\"Bedford\",\"la_homes\":\"333\"},{\"la_name\":\"Milton Keynes\",\"la_homes\":\"333\"}]";
            var context = new ActivityExecutionContext(default!, default!, default!, sut.SpId, default, default);

            //Act
            var result = await sut.ResumeAsync(context);

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
            [Frozen] Mock<IEsriSinglePipelineClient> pipelineMock,
            CustomActivities.Activities.SinglePipelineDataSource.SinglePipelineDataSource sut)
        {
            //Arrange
            pipelineMock.Setup(x => x.GetSinglePipelineData(sut.SpId))
                .ReturnsAsync((string?)null);

            var context = new ActivityExecutionContext(default!, default!, default!, sut.SpId, default, default);

            //Act
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            var suspendResult = (SuspendResult)result;
            Assert.IsType<SuspendResult>(suspendResult);
        }
    }
}
