﻿using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Tests.Common;
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
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => sut.PostStartWorkflow(startWorkflowCommandDto));

            //Assert
            Assert.Equal("Failed to start workflow", exception.Message);
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
        public async Task PostExecuteWorkflow_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            ExecuteWorkflowCommandDto commandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => sut.PostExecuteWorkflow(commandDto));

            //Assert
            Assert.Equal("Failed to execute workflow", exception.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task PostExecuteWorkflow_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            ExecuteWorkflowCommandDto commandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.PostExecuteWorkflow(commandDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<WorkflowNextActivityDataDto>(result);
        }


        [Theory]
        [AutoMoqData]
        public async Task QuestionScreenSaveAndContinue_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            QuestionScreenSaveAndContinueCommandDto saveAndContinueCommandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);


            //Act
            var result = await sut.QuestionScreenSaveAndContinue(saveAndContinueCommandDto);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task QuestionScreenSaveAndContinue_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            QuestionScreenSaveAndContinueCommandDto saveAndContinueCommandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.QuestionScreenSaveAndContinue(saveAndContinueCommandDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<WorkflowNextActivityDataDto>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadQuestionScreen_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
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
            var result = await sut.LoadQuestionScreen(loadWorkflowActivityDto);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadQuestionScreen_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
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
            var result = await sut.LoadQuestionScreen(loadWorkflowActivityDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<WorkflowActivityDataDto>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadCheckYourAnswersScreen_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
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
            var result = await sut.LoadCheckYourAnswersScreen(loadWorkflowActivityDto);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadCheckYourAnswersScreen_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
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
            var result = await sut.LoadCheckYourAnswersScreen(loadWorkflowActivityDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<WorkflowActivityDataDto>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task CheckYourAnswersSaveAndContinue_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            CheckYourAnswersSaveAndContinueCommandDto command,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);


            //Act
            var result = await sut.CheckYourAnswersSaveAndContinue(command);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task CheckYourAnswersSaveAndContinue_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            CheckYourAnswersSaveAndContinueCommandDto command,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.CheckYourAnswersSaveAndContinue(command);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<WorkflowNextActivityDataDto>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadConfirmationScreen_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
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
            var result = await sut.LoadConfirmationScreen(loadWorkflowActivityDto);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadConfirmationScreen_ReturnsWorkflowNextActivityDataDto_GivenHttpClientGivesBackNonSuccessResponse(
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
            var result = await sut.LoadConfirmationScreen(loadWorkflowActivityDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<WorkflowActivityDataDto>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadCustomActivities_ReturnsCustomActivity_GivenHttpClientGivesBackNonSuccessResponse(
          [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
          [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
          string elsaServer,
          WorkflowNextActivityDataDto workflowNextActivityDataDto,
          ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.LoadCustomActivities(elsaServer);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task LoadDataDictionary_ReturnsDataDictionary_GivenHttpClientGivesBackSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string elsaServer,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            var result = await sut.LoadDataDictionary(elsaServer);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task PostArchiveQuestions_ReturnsSuccess_GivenHttpClientGivesBackSuccessResponse(
            [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
            [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
            string[] workflowDefinitionIds,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(workflowNextActivityDataDto,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);

            //Act
            await sut.PostArchiveQuestions(workflowDefinitionIds);

            //Assert
            httpClientFactoryMock.Verify(x=>x.CreateClient("ElsaServerClient"),Times.Once); 
        }

        [Theory]
        [AutoMoqData]
        public async Task ReturnToActivity_ReturnsNull_GivenHttpClientGivesBackNonSuccessResponse(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
        ReturnToActivityData returnToActivityDto,
         ReturnToActivityDataDto returnToActivityDataDto,
        ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(returnToActivityDataDto,
                HttpStatusCode.BadRequest,
                httpClientFactoryMock,
                httpMessageHandlerMock);


            //Act
            var result = await sut.ReturnToActivity(returnToActivityDto);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task ReturnToActivity_ReturnsActivityDto_GivenHttpClientGivesBackSuccessResponse(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<HttpMessageHandler> httpMessageHandlerMock,
        ReturnToActivityData returnToActivityDto,
        ReturnToActivityDataDto returnToActivityDataDto,
        ElsaServerHttpClient sut)
        {
            //Arrange
            HttpClientTestHelpers.SetupHttpClientWithExpectedStatusCode(returnToActivityDataDto,
                HttpStatusCode.OK,
                httpClientFactoryMock,
                httpMessageHandlerMock);


            //Act
            var result = await sut.ReturnToActivity(returnToActivityDto);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ReturnToActivityDataDto>(result);
        }
    }
}
