﻿using AutoFixture.Xunit2;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.RegionalFigs;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.CustomActivities.Tests.Activities.RegionalFigsDataSource
{
    public class RegionalFigsDataSourceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task OnExecute_ReturnsSuspendResult(
           CustomActivities.Activities.RegionalFigsDataSource.RegionalFigsDataSource sut)
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
            [Frozen] Mock<IEsriRegionalFigsClient> client,
            [Frozen] Mock<IEsriRegionalFigsJsonHelper> jsonHelperMock,
            string dataString,
            string region,
            string appraisalYear,
            RegionalFigsData regionalFigsData,
            CustomActivities.Activities.RegionalFigsDataSource.RegionalFigsDataSource sut)
        {
            //Arrange
            sut.Output = null;
            sut.Region = region;
            sut.AppraisalYear = appraisalYear;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            client.Setup(x => x.GetRegionalFigsData(sut.Region, sut.AppraisalYear)).ReturnsAsync(dataString);
            jsonHelperMock.Setup(x => x.JsonToRegionalFigsData(dataString)).Returns(regionalFigsData);

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
            [Frozen] Mock<IEsriRegionalFigsClient> client,
            string region,
            string appraisalYear,
            CustomActivities.Activities.RegionalFigsDataSource.RegionalFigsDataSource sut)
        {
            //Arrange
            sut.Output = null;
            sut.Region = region;
            sut.AppraisalYear = appraisalYear;
            var context = new ActivityExecutionContext(default!, default!, default!, sut.Id, default, default);
            client.Setup(x => x.GetRegionalFigsData(sut.Region, sut.AppraisalYear)).ReturnsAsync((string?)null);

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
