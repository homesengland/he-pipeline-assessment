using AutoFixture.Xunit2;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Data.SinglePipeline;

namespace Elsa.CustomActivities.Tests.Activities.HousingNeed
{
    public class HousingNeedDataSourceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecute_WithMultiLaData_ReturnsOutcomeResult(
        [Frozen] Mock<IEsriLaHouseNeedClient> laHouseNeedClient,
        [Frozen] Mock<IEsriLaHouseNeedDataJsonHelper> jsonHelperMock,
        string dataString,
        List<LaHouseNeedData> laHouseNeedDataList,
        CustomActivities.Activities.HousingNeed.HousingNeedDataSource sut)
        {
            //Arrange
            sut.Output = null;
            sut.OutputList = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            laHouseNeedClient.Setup(x=> x.GetLaHouseNeedData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(dataString);
            jsonHelperMock.Setup(x => x.JsonToLAHouseNeedData(dataString)).Returns(laHouseNeedDataList);

            //Act
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            var outcomeResult = (OutcomeResult)result;
            Assert.IsType<OutcomeResult>(outcomeResult);
            Assert.Equal(laHouseNeedDataList, sut.OutputList);
            Assert.Null(sut.Output);
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecute_WithSingleLaData_ReturnsOutcomeResult(
        [Frozen] Mock<IEsriLaHouseNeedClient> laHouseNeedClient,
        [Frozen] Mock<IEsriLaHouseNeedDataJsonHelper> jsonHelperMock,
        string dataString,
        LaHouseNeedData laHouseNeedData,
        CustomActivities.Activities.HousingNeed.HousingNeedDataSource sut)
        {
            //Arrange
            sut.Output = null;
            sut.OutputList = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            laHouseNeedClient.Setup(x => x.GetLaHouseNeedData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(dataString);
            jsonHelperMock.Setup(x => x.JsonToLAHouseNeedData(dataString)).Returns(new List<LaHouseNeedData>() { laHouseNeedData });

            //Act
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            var outcomeResult = (OutcomeResult)result;
            Assert.IsType<OutcomeResult>(outcomeResult);
            Assert.Equal(laHouseNeedData, sut.Output);
            Assert.Null(sut.OutputList);
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecute_WithEmptyLaDataList_ReturnsSuspendedResult(
        [Frozen] Mock<IEsriLaHouseNeedClient> laHouseNeedClient,
        [Frozen] Mock<IEsriLaHouseNeedDataJsonHelper> jsonHelperMock,
        string dataString,
        CustomActivities.Activities.HousingNeed.HousingNeedDataSource sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            laHouseNeedClient.Setup(x => x.GetLaHouseNeedData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(dataString);
            jsonHelperMock.Setup(x => x.JsonToLAHouseNeedData(dataString)).Returns(new List<LaHouseNeedData>() {});

            //Act
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            var outcomeResult = (SuspendResult)result;
            Assert.IsType<SuspendResult>(outcomeResult);
        }

        [Theory]
        [AutoMoqData]
        public async Task OnExecute_WhenClientReturnsNull_ReturnsSuspendedResult(
        [Frozen] Mock<IEsriLaHouseNeedClient> laHouseNeedClient,
        CustomActivities.Activities.HousingNeed.HousingNeedDataSource sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            laHouseNeedClient.Setup(x => x.GetLaHouseNeedData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string?)null);

            //Act
            var result = await sut.ExecuteAsync(context);

            //Assert
            Assert.NotNull(result);
            var outcomeResult = (SuspendResult)result;
            Assert.IsType<SuspendResult>(outcomeResult);
        }
    }
}
