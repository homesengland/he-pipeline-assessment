using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.SinglePipeline;
using Moq;
using System;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.SinglePipeline
{
    public class SinglePipelineServiceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineData(
        [Frozen]Mock<IEsriSinglePipelineClient> singlePiplelineClient,
        SinglePipelineProvider sut )
        {
            //Arrange
            singlePiplelineClient.Setup(x => x.GetSinglePipelineDataList(0)).Throws(new Exception());

            //Act
            var result = await sut.GetSinglePipelineData();


            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineData_ReturnsNotNull_GivenSinglePipelineDataListisNull(
       [Frozen] Mock<IEsriSinglePipelineClient> singlePiplelineClient,
       SinglePipelineDataList singlePipelineDataList,
       SinglePipelineProvider sut)
        {
            //Arrange
            singlePiplelineClient.Setup(x => x.GetSinglePipelineDataList(0)).ReturnsAsync(singlePipelineDataList);

            //Act
            var result = await sut.GetSinglePipelineData();


            //Assert
            Assert.NotNull(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineData_ReturnsEmptyList_GivenSinglePipelineDataListisNull(
       [Frozen] Mock<IEsriSinglePipelineClient> singlePiplelineClient,       
       SinglePipelineProvider sut)
        {
            //Arrange
            singlePiplelineClient.Setup(x => x.GetSinglePipelineDataList(0)).ReturnsAsync((SinglePipelineDataList?)null);

            //Act
            var result = await sut.GetSinglePipelineData();


            //Assert
            Assert.Empty(result);
        }
    }
}
