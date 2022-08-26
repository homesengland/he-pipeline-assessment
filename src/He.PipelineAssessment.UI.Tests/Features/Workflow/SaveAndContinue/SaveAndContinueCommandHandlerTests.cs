using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.MultipleChoice.SaveAndContinue;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandHandlerTests
    {

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<ISaveAndContinueMapper> saveAndContinueMapper,
            SaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandDto saveAndContinueCommandDto,
            SaveAndContinueCommandHandler sut
        )
        {
            //Arrange
            saveAndContinueMapper
                .Setup(x => x.SaveAndContinueCommandToSaveAndContinueCommandDto(saveAndContinueCommand))
                .Returns(saveAndContinueCommandDto);

            elsaServerHttpClient.Setup(x => x.SaveAndContinue(saveAndContinueCommandDto))
                .ReturnsAsync((WorkflowNextActivityDataDto?)null);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsLoadWorkflowActivityRequest_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<ISaveAndContinueMapper> saveAndContinueMapper,
            SaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandDto saveAndContinueCommandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            SaveAndContinueCommandHandler sut
        )
        {
            //Arrange
            saveAndContinueMapper
                .Setup(x => x.SaveAndContinueCommandToSaveAndContinueCommandDto(saveAndContinueCommand))
                .Returns(saveAndContinueCommandDto);

            elsaServerHttpClient.Setup(x => x.SaveAndContinue(saveAndContinueCommandDto))
                .ReturnsAsync(workflowNextActivityDataDto);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(workflowNextActivityDataDto.Data.NextActivityId, result!.ActivityId);
            Assert.Equal(workflowNextActivityDataDto.Data.WorkflowInstanceId, result.WorkflowInstanceId);
        }
    }
}
