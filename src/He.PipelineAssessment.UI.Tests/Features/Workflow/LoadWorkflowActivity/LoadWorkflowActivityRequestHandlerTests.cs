using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.LoadWorkflowActivity;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
           [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
           [Frozen] Mock<ILoadWorkflowActivityMapper> loadWorkflowActivityMapper,
           LoadWorkflowActivityRequest loadWorkflowActivityRequest,
           LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            var dto = new LoadWorkflowActivityDto()
            {
                ActivityId = loadWorkflowActivityRequest.ActivityId,
                WorkflowInstanceId = loadWorkflowActivityRequest.WorkflowInstanceId
            };
            elsaServerHttpClient.Setup(x => x.LoadWorkflowActivity(dto))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result);
            elsaServerHttpClient.Verify(x => x.LoadWorkflowActivity(dto), Times.Once);
            loadWorkflowActivityMapper.Verify(x => x.WorkflowActivityDataDtoToSaveAndContinueCommand(It.IsAny<WorkflowActivityDataDto>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<ILoadWorkflowActivityMapper> loadWorkflowActivityMapper,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            WorkflowActivityDataDto workflowActivityDataDto,
            SaveAndContinueCommand saveAndContinueCommand,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadWorkflowActivity(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            loadWorkflowActivityMapper
                .Setup(x => x.WorkflowActivityDataDtoToSaveAndContinueCommand(workflowActivityDataDto))
                .Returns(saveAndContinueCommand);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SaveAndContinueCommand>(result);
            elsaServerHttpClient.Verify(x => x.LoadWorkflowActivity(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
            loadWorkflowActivityMapper.Verify(x => x.WorkflowActivityDataDtoToSaveAndContinueCommand(It.IsAny<WorkflowActivityDataDto>()), Times.Once);
        }
    }
}
