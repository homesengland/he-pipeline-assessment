using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
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
                .Setup(x => x.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(saveAndContinueCommand))
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
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            AssessmentStage assessmentStage,
            SaveAndContinueCommand saveAndContinueCommand,
            SaveAndContinueCommandDto saveAndContinueCommandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            SaveAndContinueCommandHandler sut
        )
        {
            //Arrange
            saveAndContinueMapper
                .Setup(x => x.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(saveAndContinueCommand))
                .Returns(saveAndContinueCommandDto);

            elsaServerHttpClient.Setup(x => x.SaveAndContinue(saveAndContinueCommandDto))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentStage(workflowNextActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentStage);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(workflowNextActivityDataDto.Data.NextActivityId, result!.ActivityId);
            Assert.Equal(workflowNextActivityDataDto.Data.WorkflowInstanceId, result.WorkflowInstanceId);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsLoadWorkflowActivityRequest_GivenNoErrorsEncounteredAndActivityTypeIsQuestionScreen(
        [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
        [Frozen] Mock<ISaveAndContinueMapper> saveAndContinueMapper,
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        AssessmentStage assessmentStage,
        SaveAndContinueCommand saveAndContinueCommand,
        SaveAndContinueCommandDto saveAndContinueCommandDto,
        WorkflowNextActivityDataDto workflowNextActivityDataDto,
        SaveAndContinueCommandHandler sut
        )
        {
            //Arrange
            saveAndContinueCommand.Data!.ActivityType = ActivityTypeConstants.QuestionScreen;
            saveAndContinueMapper
                .Setup(x => x.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(saveAndContinueCommand))
                .Returns(saveAndContinueCommandDto);

            elsaServerHttpClient.Setup(x => x.SaveAndContinue(saveAndContinueCommandDto))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentStage(workflowNextActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentStage);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(workflowNextActivityDataDto.Data.NextActivityId, result!.ActivityId);
            Assert.Equal(workflowNextActivityDataDto.Data.WorkflowInstanceId, result.WorkflowInstanceId);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
