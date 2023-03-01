using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Workflow.StartWorkflow;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            StartWorkflowCommand command,
            StartWorkflowCommandHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.PostStartWorkflow(It.IsAny<StartWorkflowCommandDto>()))
                .ReturnsAsync((WorkflowNextActivityDataDto?)null);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsLoadWorkflowActivityRequest_GivenNoErrorsOccur(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            StartWorkflowCommand command,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            StartWorkflowCommandHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.PostStartWorkflow(It.IsAny<StartWorkflowCommandDto>()))
                .ReturnsAsync(workflowNextActivityDataDto);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(workflowNextActivityDataDto.Data.NextActivityId, result!.ActivityId);
            Assert.Equal(workflowNextActivityDataDto.Data.WorkflowInstanceId, result.WorkflowInstanceId);
        }
    }
}
