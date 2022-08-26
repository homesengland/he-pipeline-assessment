using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue;
using Elsa.CustomWorkflow.Sdk.Models.StartWorkflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.LoadWorkflowActivity;
using Moq;
using System.Net;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.HttpClients
{
    public class ElsaServerHttpClientTests
    {
        [Theory]
        [AutoMoqData]
        public async Task PostStartWorkflow_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            StartWorkflowCommandDto startWorkflowCommandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);


            //Act
            var result = await sut.PostStartWorkflow(startWorkflowCommandDto);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task PostStartWorkflow_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            StartWorkflowCommandDto startWorkflowCommandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.PostStartWorkflow(startWorkflowCommandDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<WorkflowNextActivityDataDto>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task SaveAndContinue_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            SaveAndContinueCommandDto saveAndContinueCommandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);


            //Act
            var result = await sut.SaveAndContinue(saveAndContinueCommandDto);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task SaveAndContinue_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            SaveAndContinueCommandDto saveAndContinueCommandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.SaveAndContinue(saveAndContinueCommandDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<WorkflowNextActivityDataDto>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            LoadWorkflowActivityDto loadWorkflowActivityDto,
            WorkflowActivityDataDto workflowActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowActivityDataDto,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);


            //Act
            var result = await sut.LoadWorkflowActivity(loadWorkflowActivityDto);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadWorkflowActivity_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            LoadWorkflowActivityDto loadWorkflowActivityDto,
            WorkflowActivityDataDto workflowActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowActivityDataDto,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.LoadWorkflowActivity(loadWorkflowActivityDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<WorkflowActivityDataDto>(result);
        }
    }
}
