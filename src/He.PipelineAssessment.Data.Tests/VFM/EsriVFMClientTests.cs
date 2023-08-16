using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.VFM;
using Moq;
using System.Net;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.VFM
{
    public class EsriVFMClientTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetLaHouseNeedData_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string gsscode,
            string laName,
            string altLaName,
            string resp,
            EsriVFMClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetVFMCalculationData(gsscode, laName, altLaName);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetLaHouseNeedData_ReturnsValue_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string gsscode,
            string laName,
            string altLaName,
            string resp,
            EsriVFMClient sut)

        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetVFMCalculationData(gsscode, laName, altLaName);

            //Assert
            Assert.NotNull(result);
        }
    }
}
