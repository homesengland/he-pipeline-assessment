using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.SetResult;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SetResult
{
    public class SetResultRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            SetResultRequest request,
            SetResultRequestHandler sut)
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.SetResult(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result);
            elsaServerHttpClient.Verify(x => x.SetResult(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsQuestionScreenSaveAndContinueCommandResponse_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            SetResultRequest request,
            WorkflowActivityDataDto workflowActivityDataDto,
            SetResultRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.SetResult(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<QuestionScreenSaveAndContinueCommandResponse>(result);
            Assert.Equal(workflowActivityDataDto.Data.ActivityId, result!.ActivityId);
            Assert.Equal(workflowActivityDataDto.Data.ActivityType, result.ActivityType);
            Assert.Equal(workflowActivityDataDto.Data.WorkflowInstanceId, result.WorkflowInstanceId);
            elsaServerHttpClient.Verify(x => x.SetResult(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }
    }
}
