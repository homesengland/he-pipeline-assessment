using AutoFixture.Xunit2;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Data.SinglePipeline;
using Microsoft.Identity.Client.Extensions.Msal;

namespace Elsa.CustomActivities.Tests.Activities.HousingNeed
{
    public class HousingNeedDataSourceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecute_ReturnsSuspendResult(
            CustomActivities.Activities.HousingNeed.HousingNeedDataSource sut)
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
        public async Task ResumeAsync_WithMultiLaData_ReturnsOutcomeResult(
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
            var result = await sut.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);

            var combinedResult = (CombinedResult)result;
            Assert.Equal(2, combinedResult.Results.Count);
            var outcomeResult = (OutcomeResult)combinedResult.Results.First(x => x.GetType() == typeof(OutcomeResult));
            Assert.Equal("Done", outcomeResult.Outcomes.First());
            Assert.Contains(combinedResult.Results, x => x.GetType() == typeof(SuspendResult));

            Assert.Equal(laHouseNeedDataList, sut.OutputList);
            Assert.Null(sut.Output);
        }

        [Theory]
        [AutoMoqData]
        public async Task ResumeAsync_WithSingleLaData_ReturnsOutcomeResult(
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
            var result = await sut.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);
            var combinedResult = (CombinedResult)result;
            Assert.Equal(2, combinedResult.Results.Count);
            var outcomeResult = (OutcomeResult)combinedResult.Results.First(x => x.GetType() == typeof(OutcomeResult));
            Assert.Equal("Done", outcomeResult.Outcomes.First());
            Assert.Contains(combinedResult.Results, x => x.GetType() == typeof(SuspendResult));
            Assert.Equal(laHouseNeedData, sut.Output);
            Assert.Null(sut.OutputList);
        }

        [Theory]
        [AutoMoqData]
        public async Task ResumeAsync_WithEmptyLaDataList_ReturnsSuspendedResult(
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
        public async Task ResumeAsync_WhenClientReturnsNull_ReturnsSuspendedResult(
        [Frozen] Mock<IEsriLaHouseNeedClient> laHouseNeedClient,
        CustomActivities.Activities.HousingNeed.HousingNeedDataSource sut)
        {
            //Arrange
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            laHouseNeedClient.Setup(x => x.GetLaHouseNeedData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string?)null);

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
        public async Task ResumeAsync_WhenClientReturnsNullForCurrentValues_PreviousValuesUsed(
        [Frozen] Mock<IEsriLaHouseNeedClient> laHouseNeedClient,
        [Frozen] Mock<IEsriLaHouseNeedDataJsonHelper> jsonHelperMock,
        string dataString,
        LaHouseNeedData laHouseNeedData,
        CustomActivities.Activities.HousingNeed.HousingNeedDataSource sut)
        {
            sut.Output = null;
            sut.OutputList = null;
            //Arrange
            sut.PreviousGssCodes = "E08000001";
            sut.PreviousLocalAuthorities = "Harrogate";
            sut.GssCodes = "E08000002";
            sut.LocalAuthorities = "North Yorkshire";
            sut.LocalAuthoritiesAlt = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            laHouseNeedClient.Setup(x => x.GetLaHouseNeedData(
                It.Is<string?>(x => x != null && x.Equals(sut.GssCodes)),
                It.Is<string?>(x => x != null && x.Equals(sut.LocalAuthorities)),
                It.IsAny<string>()))
                .ReturnsAsync((string?)null);
            laHouseNeedClient.Setup(x => x.GetLaHouseNeedData(
                It.Is<string?>(x => x != null && x.Equals(sut.PreviousGssCodes)),
                It.Is<string?>(x => x != null && x.Equals(sut.PreviousLocalAuthorities)),
                null))
                .ReturnsAsync(dataString); 
            jsonHelperMock.Setup(x => x.JsonToLAHouseNeedData(It.Is<string>(x => x != null && x.Equals(dataString)))).Returns(new List<LaHouseNeedData>() { laHouseNeedData });



            //Act
            var result = await sut.ResumeAsync(context);

            //Assert
            Assert.NotNull(result);
            var combinedResult = (CombinedResult)result;
            Assert.Equal(2, combinedResult.Results.Count);
            var outcomeResult = (OutcomeResult)combinedResult.Results.First(x => x.GetType() == typeof(OutcomeResult));
            Assert.Equal("Done", outcomeResult.Outcomes.First());
            Assert.Contains(combinedResult.Results, x => x.GetType() == typeof(SuspendResult));
            // Verify the expected client call was made
            laHouseNeedClient.Verify(x => x.GetLaHouseNeedData(
                sut.PreviousGssCodes,
                sut.PreviousLocalAuthorities,
                null), Times.Once);
            Assert.Equal(laHouseNeedData, sut.Output);
            Assert.Null(sut.OutputList);
        }


    }
}
