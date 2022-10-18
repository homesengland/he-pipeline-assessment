

using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Data.SinglePipeline;
using Moq;
using System.Net;
using Xunit;

namespace He.PipelineAssessment.Data.Tests
{

    //public class EsriSinglePipelineClientTests
    //    {
    //        [Theory]
    //        [AutoMoqData]
    //        public async Task PostStartWorkflow_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
    //            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
    //            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
    //            EsriSinglePipelineClient sut)
    //        {
    //            //Arrange
    //            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
    //                HttpStatusCode.BadRequest,
    //                httpClientFactoryMock,
    //                httpMessageHandlerMock);

    //            //Act
    //            var exception = await Assert.ThrowsAsync<ApplicationException>(() => sut.GetSinglePipelineData(startWorkflowCommandDto));

    //            //Assert
    //            Assert.Equal("Failed to start workflow", exception.Message);
    //        }

    //        [Theory]
    //        [AutoMoqData]
    //        public async Task PostStartWorkflow_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
    //            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
    //            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
    //            StartWorkflowCommandDto startWorkflowCommandDto,
    //            WorkflowNextActivityDataDto workflowNextActivityDataDto,
    //            EsriSinglePipelineClient sut)
    //        {
    //            //Arrange
    //            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
    //                HttpStatusCode.OK,
    //                httpClientFactoryMock,
    //                httpMessageHandlerMock);

    //            //Act
    //            var result = await sut.PostStartWorkflow(startWorkflowCommandDto);

    //            //Assert
    //            Assert.NotNull(result);
    //            Assert.IsType<WorkflowNextActivityDataDto>(result);
    //        }

    //        [Theory]
    //        [AutoMoqData]
    //        public async Task SaveAndContinue_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
    //            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
    //            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
    //            SaveAndContinueCommandDto saveAndContinueCommandDto,
    //            WorkflowNextActivityDataDto workflowNextActivityDataDto,
    //            EsriSinglePipelineClient sut)
    //        {
    //            //Arrange
    //            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
    //                HttpStatusCode.BadRequest,
    //                httpClientFactoryMock,
    //                httpMessageHandlerMock);


    //            //Act
    //            var result = await sut.SaveAndContinue(saveAndContinueCommandDto);

    //            //Assert
    //            Assert.Null(result);
    //        }

    //        [Theory]
    //        [AutoMoqData]
    //        public async Task SaveAndContinue_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
    //            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
    //            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
    //            SaveAndContinueCommandDto saveAndContinueCommandDto,
    //            WorkflowNextActivityDataDto workflowNextActivityDataDto,
    //            EsriSinglePipelineClient sut)
    //        {
    //            //Arrange
    //            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
    //                HttpStatusCode.OK,
    //                httpClientFactoryMock,
    //                httpMessageHandlerMock);

    //            //Act
    //            var result = await sut.SaveAndContinue(saveAndContinueCommandDto);

    //            //Assert
    //            Assert.NotNull(result);
    //            Assert.IsType<WorkflowNextActivityDataDto>(result);
    //        }

    //        [Theory]
    //        [AutoMoqData]
    //        public async Task LoadWorkflowActivity_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
    //            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
    //            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
    //            LoadWorkflowActivityDto loadWorkflowActivityDto,
    //            WorkflowActivityDataDto workflowActivityDataDto,
    //            EsriSinglePipelineClient sut)
    //        {
    //            //Arrange
    //            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowActivityDataDto,
    //                HttpStatusCode.BadRequest,
    //                httpClientFactoryMock,
    //                httpMessageHandlerMock);


    //            //Act
    //            var result = await sut.LoadWorkflowActivity(loadWorkflowActivityDto);

    //            //Assert
    //            Assert.Null(result);
    //        }

    //        [Theory]
    //        [AutoMoqData]
    //        public async Task LoadWorkflowActivity_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
    //            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
    //            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
    //            LoadWorkflowActivityDto loadWorkflowActivityDto,
    //            WorkflowActivityDataDto workflowActivityDataDto,
    //            EsriSinglePipelineClient sut)
    //        {
    //            //Arrange
    //            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowActivityDataDto,
    //                HttpStatusCode.OK,
    //                httpClientFactoryMock,
    //                httpMessageHandlerMock);

    //            //Act
    //            var result = await sut.LoadWorkflowActivity(loadWorkflowActivityDto);

    //            //Assert
    //            Assert.NotNull(result);
    //            Assert.IsType<WorkflowActivityDataDto>(result);
    //        }
    //    }
 
}

