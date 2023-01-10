using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            LoadConfirmationScreenRequest request,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result);
            elsaServerHttpClient.Verify(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenNoAssessmentStageFound(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            LoadConfirmationScreenRequest request,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentStage(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync((AssessmentToolWorkflowInstance?)null);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result);
            elsaServerHttpClient.Verify(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            LoadConfirmationScreenRequest request,
            AssessmentToolWorkflowInstance stage,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentStage(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(stage);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<LoadConfirmationScreenResponse>(result);
            Assert.Equal(stage.Assessment.SpId.ToString(), result!.CorrelationId);
            Assert.Equal(stage.AssessmentId, result.AssessmentId);
            elsaServerHttpClient.Verify(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }
    }
}
