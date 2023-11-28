using AutoFixture.Xunit2;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using He.PipelineAssessment.Data.ExtendedSinglePipeline;
using Elsa.CustomActivities.Activities.SinglePipelineExtendedDataSource;

namespace Elsa.CustomActivities.Tests.Activities.SinglePipelineExtendedDataSourceTests
{
    public class SinglePipelineExtendedDataSourceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecute_WithSuccessfulData_ReturnsOutcomeResult(
            [Frozen] Mock<IEsriSinglePipelineExtendedClient> pipelineMock,
            [Frozen] Mock<IEsriSinglePipelineExtendedDataJsonHelper> jsonHelperMock,
            string singlePipelineString,
            SinglePipelineExtendedData SinglePipelineExtendedData,
            SinglePipelineExtendedDataSource sut)
        {
            //Arrange
            pipelineMock.Setup(x => x.GetSinglePipelineExtendedData(sut.SpId))
                .ReturnsAsync(singlePipelineString);

            jsonHelperMock.Setup(x => x.JsonToSinglePipelineExtendedData(singlePipelineString))
                .Returns(SinglePipelineExtendedData);
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
            [Frozen] Mock<IEsriSinglePipelineExtendedClient> pipelineMock,
            SinglePipelineExtendedDataSource sut)
        {
            //Arrange
            pipelineMock.Setup(x => x.GetSinglePipelineExtendedData(sut.SpId))
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
