

using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Data.SinglePipeline;
using Moq;
using System.Net;
using Xunit;

namespace He.PipelineAssessment.Data.Tests
{

    public class EsriSinglePipelineClientTests
    {
        [Theory]
        [AutoMoqData]
        public async Task PostStartWorkflow_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string spid,
            string resp,
            EsriSinglePipelineClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var isNull = await  sut.GetSinglePipelineData(spid);

            //Assert
            Assert.Null(isNull);
        }

        [Theory]
        [AutoMoqData]
        public async Task PostStartWorkflow_ReturnsValue_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string spid,
            string resp,
            EsriSinglePipelineClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetSinglePipelineData(spid);

            //Assert
            Assert.NotNull(result);
        }
    }

}

