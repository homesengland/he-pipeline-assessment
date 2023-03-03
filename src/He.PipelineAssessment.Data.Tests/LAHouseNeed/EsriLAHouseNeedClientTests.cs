using AutoFixture.Xunit2;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System.Net;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.LAHouseNeed
{
    public class EsriLAHouseNeedClientTests
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
            EsriLAHouseNeedClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetLaHouseNeedData(gsscode, laName, altLaName);

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
            EsriLAHouseNeedClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetLaHouseNeedData(gsscode, laName, altLaName);

            //Assert
            Assert.NotNull(result);
        }
    }
}
