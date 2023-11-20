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

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineExtendedDataList_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string resp,
            int offset,
            EsriSinglePipelineExtendedClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var isNull = await sut.GetSinglePipelineExtendedDataList(offset);

            //Assert
            Assert.Null(isNull);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineExtendedDataList_ReturnsNull_GivenHttpClientGivesBackSuccessResponse_AndJsonSerilizerFailed(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            [Frozen] Mock<IEsriSinglePipelineExtendedDataJsonHelper> jsonHelper,
            string resp,
            int offset,
            EsriSinglePipelineExtendedClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);
            jsonHelper.Setup(x => x.JsonToSinglePipelineExtendedDataList(It.IsAny<string>())).Returns((SinglePipelineExtendedDataList?)null);

            //Act
            var result = await sut.GetSinglePipelineExtendedDataList(offset);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetSinglePipelineExtendedDataList_ReturnsListOfSinglePiplelineData_GivenHttpClientGivesBackSuccessResponse(
          [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
          [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
          [Frozen] Mock<IEsriSinglePipelineExtendedDataJsonHelper> jsonHelper,         
          string resp,
          SinglePipelineExtendedDataList SinglePipelineExtendedDataLists,
          int offset,
          EsriSinglePipelineExtendedClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);
            jsonHelper.Setup(x => x.JsonToSinglePipelineExtendedDataList(It.IsAny<string>())).Returns(SinglePipelineExtendedDataLists);

            //Act
            var result = await sut.GetSinglePipelineExtendedDataList(offset);

            //Assert
            Assert.NotNull(result);
        }
    }

}

