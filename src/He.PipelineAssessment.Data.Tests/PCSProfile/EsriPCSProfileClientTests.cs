using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Data.PCSProfile;
using Moq;
using System.Net;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.PCSProfile
{
    public class EsriPCSProfileClientTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetPCSProfile_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string projectIdentifier,
            string resp,
            EsriPCSProfileClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetPCSProfileData(projectIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPCSProfile_ReturnsValue_GivenHttpClientGivesBackSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string projectIdentifier,
            string resp,
            EsriPCSProfileClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetPCSProfileData(projectIdentifier);

            //Assert
            Assert.NotNull(result);
            Assert.Contains(resp, result);
        }
    }
}
