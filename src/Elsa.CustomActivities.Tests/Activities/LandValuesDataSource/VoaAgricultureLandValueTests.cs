using AutoFixture.Xunit2;
using Elsa.ActivityResults;
using Elsa.CustomActivities.Activities.BilDataSource;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.VoaLandValues.Agricultural;
using He.PipelineAssessment.Data.VoaLandValues.Models.Agriculture;
using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.BILDataSource
{
    public class VoaAgricultureLandValueTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecute_ReturnsSuspendResult(BILVoaAgricultureLandValueDataSource sut)
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
            [Frozen] Mock<IAgricultureLandValuesClient> client,
            [Frozen] Mock<IAgricultureLandValuesDataJsonHelper> jsonHelperMock,
            string dataString,
            AgricultureLandValues data,
            BILVoaAgricultureLandValueDataSource sut)
        {
            //Arrange
            sut.Output = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            client.Setup(x => x.GetAgricultureLandValues(It.IsAny<string>())).ReturnsAsync(dataString);
            jsonHelperMock.Setup(x => x.JsonToVoaAgricultureLandValuesData(dataString)).Returns(data);

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
            [Frozen] Mock<IAgricultureLandValuesClient> client,
            BILVoaAgricultureLandValueDataSource sut)
        {
            //Arrange
            sut.Output = null;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            client.Setup(x => x.GetAgricultureLandValues(It.IsAny<string>())).ReturnsAsync((string?)null);

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
