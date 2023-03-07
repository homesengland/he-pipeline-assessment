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

            var context = new ActivityExecutionContext(default!, default!, default!, sut.SpId, default, default);

            //Act
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            var outcomeResult = (OutcomeResult)result;
            Assert.IsType<OutcomeResult>(outcomeResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecute_WithNullData_ReturnsSuspendResult(
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
