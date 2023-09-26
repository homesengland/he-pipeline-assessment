using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System.Net;
using Xunit;
using He.PipelineAssessment.Data.RegionalIPU;

namespace He.PipelineAssessment.Data.Tests.RegionalIPU
{
    public class EsriRegionslIPUClientTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetRegionalIPU_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string projectIdentifier,
            string resp,
            EsriRegionalIPUClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetRegionalIPUData(projectIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetRegionalIPU_ReturnsValue_GivenHttpClientGivesBackSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string projectIdentifier,
            string resp,
            EsriRegionalIPUClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetRegionalIPUData(projectIdentifier);

            //Assert
            Assert.NotNull(result);
            Assert.Contains(resp, result);
        }
    }
}
