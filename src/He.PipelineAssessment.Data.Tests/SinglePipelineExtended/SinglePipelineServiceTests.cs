using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System;
using Xunit;
using He.PipelineAssessment.Data.ExtendedSinglePipeline;

namespace He.PipelineAssessment.Data.Tests.SinglePipelineExtendedExtended
{
    public class SinglePipelineExtendedServiceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineExtendedData(
        [Frozen]Mock<IEsriSinglePipelineExtendedClient> singlePiplelineClient,
        SinglePipelineExtendedProvider sut )
        {
            //Arrange
            singlePiplelineClient.Setup(x => x.GetSinglePipelineExtendedDataList(0)).Throws(new Exception());

            //Act
            var result = await sut.GetSinglePipelineData();


            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineExtendedData_ReturnsNotNull_GivenSinglePipelineExtendedDataListisNull(
       [Frozen] Mock<IEsriSinglePipelineExtendedClient> singlePiplelineClient,
       SinglePipelineExtendedDataList SinglePipelineExtendedDataList,
       SinglePipelineExtendedProvider sut)
        {
            //Arrange
            singlePiplelineClient.Setup(x => x.GetSinglePipelineExtendedDataList(0)).ReturnsAsync(SinglePipelineExtendedDataList);

            //Act
            var result = await sut.GetSinglePipelineData();


            //Assert
            Assert.NotNull(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineExtendedData_ReturnsEmptyList_GivenSinglePipelineExtendedDataListisNull(
       [Frozen] Mock<IEsriSinglePipelineExtendedClient> singlePiplelineClient,       
       SinglePipelineExtendedProvider sut)
        {
            //Arrange
            singlePiplelineClient.Setup(x => x.GetSinglePipelineExtendedDataList(0)).ReturnsAsync((SinglePipelineExtendedDataList?)null);

            //Act
            var result = await sut.GetSinglePipelineData();


            //Assert
            Assert.Empty(result);
        }
    }
}
