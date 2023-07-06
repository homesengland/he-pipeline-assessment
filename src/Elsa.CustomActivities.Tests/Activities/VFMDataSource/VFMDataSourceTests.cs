using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Data.VFM;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.VFMDataSource
{
    public class VFMDataSourceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecute_ReturnsSuspendResult(
           CustomActivities.Activities.VFMDataSource.VFMDataSource sut)
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
            [Frozen] Mock<IEsriVFMClient> client,
            [Frozen] Mock<IEsriVFMDataJsonHelper> jsonHelperMock,
            string dataString,
            VFMCalculationData vfmCalculationData,
            CustomActivities.Activities.VFMDataSource.VFMDataSource sut)
        {
            //Arrange
            sut.Output = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            client.Setup(x => x.GetVFMCalculationData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(dataString);
            jsonHelperMock.Setup(x => x.JsonToVFMCalculationData(dataString)).Returns(vfmCalculationData);

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
            [Frozen] Mock<IEsriVFMClient> laHouseNeedClient,
            [Frozen] Mock<IEsriVFMDataJsonHelper> jsonHelperMock,
            string dataString,
            VFMCalculationData vfmCalculationData,
            CustomActivities.Activities.VFMDataSource.VFMDataSource sut)
        {
            //Arrange
            sut.Output = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            laHouseNeedClient.Setup(x => x.GetVFMCalculationData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string?)null);
            jsonHelperMock.Setup(x => x.JsonToVFMCalculationData(dataString)).Returns(vfmCalculationData);

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
