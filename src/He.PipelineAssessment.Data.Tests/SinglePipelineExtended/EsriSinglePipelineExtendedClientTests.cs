using AutoFixture.Xunit2;
using He.PipelineAssessment.Data.ExtendedSinglePipeline;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System.Net;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.SinglePipelineExtendedExtended
{

    public class EsriSinglePipelineExtendedClientTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineExtendedData_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string spid,
            string resp,
            EsriSinglePipelineExtendedClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var isNull = await sut.GetSinglePipelineExtendedData(spid);

            //Assert
            Assert.Null(isNull);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineExtendedData_ReturnsValue_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string spid,
            string resp,
            EsriSinglePipelineExtendedClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetSinglePipelineExtendedData(spid);

            //Assert
            Assert.NotNull(result);
        }
    }

}

