using AutoFixture.Xunit2;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.RegionalIPU;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.RegionalIPUDataSource
{
    public class RegionalIPUDataSourceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecute_ReturnsSuspendResult(
           CustomActivities.Activities.RegionalIPUDataSource.RegionalIPUDataSource sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            //Act
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SuspendResult>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task ResumeAsync_ReturnsOutcomeResult(
            [Frozen] Mock<IEsriRegionalIPUClient> client,
            [Frozen] Mock<IEsriRegionalIPUJsonHelper> jsonHelperMock,
            string dataString,
            string region,
            string product,
            RegionalIPUData regionalIPUData,
            CustomActivities.Activities.RegionalIPUDataSource.RegionalIPUDataSource sut)
        {
            //Arrange
            sut.Output = null;
            sut.Region = region;
            sut.Product = product;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            client.Setup(x => x.GetRegionalIPUData(sut.Region, sut.Product)).ReturnsAsync(dataString);
            jsonHelperMock.Setup(x => x.JsonToRegionalIPUData(dataString)).Returns(regionalIPUData);

            //Act
            var result = await sut.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);

            var combinedResult = (CombinedResult)result;
            Assert.Equal(2, combinedResult.Results.Count);
            var outcomeResult = (OutcomeResult)combinedResult.Results.First(x => x.GetType() == typeof(OutcomeResult));
            Assert.Equal("Done", outcomeResult.Outcomes.First());
            Assert.Contains(combinedResult.Results, x => x.GetType() == typeof(SuspendResult));

            Assert.NotNull(sut.Output);
        }

        [Theory]
        [AutoMoqData]
        public async Task ResumeAsync_ReturnsOutcomeResult_GivenClientDataIsNull(
            [Frozen] Mock<IEsriRegionalIPUClient> client,
            string region,
            string product,
            CustomActivities.Activities.RegionalIPUDataSource.RegionalIPUDataSource sut)
        {
            //Arrange
            sut.Output = null;
            sut.Region = region;
            sut.Product = product;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            client.Setup(x => x.GetRegionalIPUData(sut.Region, sut.Product)).ReturnsAsync((string?)null);

            //Act
            var result = await sut.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);

            var combinedResult = (CombinedResult)result;
            Assert.Equal(2, combinedResult.Results.Count);
            var outcomeResult = (OutcomeResult)combinedResult.Results.First(x => x.GetType() == typeof(OutcomeResult));
            Assert.Equal("Done", outcomeResult.Outcomes.First());
            Assert.Contains(combinedResult.Results, x => x.GetType() == typeof(SuspendResult));

            Assert.Null(sut.Output);
        }

    }
}

