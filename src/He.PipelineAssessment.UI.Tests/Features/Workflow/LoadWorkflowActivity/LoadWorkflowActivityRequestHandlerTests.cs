using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.LoadWorkflowActivity;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using Moq;
using Xunit;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
           [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
           LoadWorkflowActivityRequest loadWorkflowActivityRequest,
           LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.LoadWorkflowActivity(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result);
            elsaServerHttpClient.Verify(x => x.LoadWorkflowActivity(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            LoadWorkflowActivityRequest loadWorkflowActivityRequest,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadWorkflowActivityRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadWorkflowActivity(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SaveAndContinueCommand>(result);
            elsaServerHttpClient.Verify(x => x.LoadWorkflowActivity(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }
    }
}
