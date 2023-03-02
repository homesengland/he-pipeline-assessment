using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.LoadQuestionScreen
{
    public class LoadQuestionScreenRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
           [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
           LoadQuestionScreenRequest loadWorkflowActivityRequest,
           LoadQuestionScreenRequestHandler sut)
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result);
            elsaServerHttpClient.Verify(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadQuestionScreenRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<QuestionScreenSaveAndContinueCommand>(result);
            elsaServerHttpClient.Verify(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }
    }
}
