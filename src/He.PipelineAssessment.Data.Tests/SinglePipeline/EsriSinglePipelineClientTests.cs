using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.SinglePipeline;
using Moq;
using System.Net;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.SinglePipeline
{

    public class EsriSinglePipelineClientTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineData_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
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
            var isNull = await sut.GetSinglePipelineData(spid);

            //Assert
            Assert.Null(isNull);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineData_ReturnsValue_GivenHttpClientGivesBackNonSuccessResponse(
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

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineDataList_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string resp,
            int offset,
            EsriSinglePipelineClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var isNull = await sut.GetSinglePipelineDataList(offset);

            //Assert
            Assert.Null(isNull);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineDataList_ReturnsNull_GivenHttpClientGivesBackSuccessResponse_AndJsonSerilizerFailed(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            [Frozen] Mock<IEsriSinglePipelineDataJsonHelper> jsonHelper,
            string resp,
            int offset,
            EsriSinglePipelineClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);
            jsonHelper.Setup(x => x.JsonToSinglePipelineDataList(It.IsAny<string>())).Returns((SinglePipelineDataList?)null);

            //Act
            var result = await sut.GetSinglePipelineDataList(offset);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineDataList_ReturnsListOfSinglePiplelineData_GivenHttpClientGivesBackSuccessResponse(
          [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
          [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
          [Frozen] Mock<IEsriSinglePipelineDataJsonHelper> jsonHelper,         
          string resp,
          SinglePipelineDataList singlePipelineDataLists,
          int offset,
          EsriSinglePipelineClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);
            jsonHelper.Setup(x => x.JsonToSinglePipelineDataList(It.IsAny<string>())).Returns(singlePipelineDataLists);

            //Act
            var result = await sut.GetSinglePipelineDataList(offset);

            //Assert
            Assert.NotNull(result);
        }
    }

}

