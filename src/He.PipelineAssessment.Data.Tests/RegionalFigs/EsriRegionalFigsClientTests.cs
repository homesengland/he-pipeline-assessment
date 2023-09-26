﻿using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System.Net;
using Xunit;
using He.PipelineAssessment.Data.RegionalFigs;

namespace He.PipelineAssessment.Data.Tests.RegionalFigs
{
    public class EsriRegionslFigsClientTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetRegionalFigs_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string projectIdentifier,
            string resp,
            EsriRegionalFigsClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetRegionalFigsData(projectIdentifier);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetRegionalFigs_ReturnsValue_GivenHttpClientGivesBackSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string projectIdentifier,
            string resp,
            EsriRegionalFigsClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(resp,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.GetRegionalFigsData(projectIdentifier);

            //Assert
            Assert.NotNull(result);
            Assert.Contains(resp, result);
        }
    }
}
