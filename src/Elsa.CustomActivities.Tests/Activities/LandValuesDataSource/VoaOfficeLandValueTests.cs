using AutoFixture.Xunit2;
using Elsa.ActivityResults;
using Elsa.CustomActivities.Activities.LandValuesDataSource;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.VoaLandValues.Models.Office;
using He.PipelineAssessment.Data.VoaLandValues.Office;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.VoaLandValuesDataSource
{
    public class VoaOfficeLandValueTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecute_ReturnsSuspendResult(OfficeLandValueDataSource sut)
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
            [Frozen] Mock<IOfficeLandValuesClient> client,
            [Frozen] Mock<IOfficeLandValuesDataJsonHelper> jsonHelperMock,
            string dataString,
            OfficeLandValues data,
            OfficeLandValueDataSource sut)
        {
            //Arrange
            sut.Output = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            client.Setup(x => x.GetOfficeLandValues(It.IsAny<string>())).ReturnsAsync(dataString);
            jsonHelperMock.Setup(x => x.JsonToOfficeLandValuesData(dataString)).Returns(data);

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
        public async Task ResumeAsync_ReturnsOutcomeResult_GivenClientReturnsNull(
            [Frozen] Mock<IOfficeLandValuesClient> client,
            OfficeLandValueDataSource sut)
        {
            //Arrange
            sut.Output = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            client.Setup(x => x.GetOfficeLandValues(It.IsAny<string>())).ReturnsAsync((string?)null);

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
