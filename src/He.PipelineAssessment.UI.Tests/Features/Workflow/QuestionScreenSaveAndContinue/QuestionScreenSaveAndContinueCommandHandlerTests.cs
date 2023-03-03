using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommandHandlerTests
    {

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IQuestionScreenSaveAndContinueMapper> saveAndContinueMapper,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            QuestionScreenSaveAndContinueCommandDto saveAndContinueCommandDto,
            QuestionScreenSaveAndContinueCommandHandler sut
        )
        {
            //Arrange
            saveAndContinueMapper
                .Setup(x => x.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(saveAndContinueCommand))
                .Returns(saveAndContinueCommandDto);

            elsaServerHttpClient.Setup(x => x.QuestionScreenSaveAndContinue(saveAndContinueCommandDto))
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
            [Frozen] Mock<IQuestionScreenSaveAndContinueMapper> saveAndContinueMapper,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            QuestionScreenSaveAndContinueCommandDto saveAndContinueCommandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            QuestionScreenSaveAndContinueCommandHandler sut
        )
        {
            //Arrange
            saveAndContinueMapper
                .Setup(x => x.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(saveAndContinueCommand))
                .Returns(saveAndContinueCommandDto);

            elsaServerHttpClient.Setup(x => x.QuestionScreenSaveAndContinue(saveAndContinueCommandDto))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowNextActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

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
        [Frozen] Mock<IQuestionScreenSaveAndContinueMapper> saveAndContinueMapper,
        [Frozen] Mock<IAssessmentRepository> assessmentRepository,
        AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
        QuestionScreenSaveAndContinueCommandDto saveAndContinueCommandDto,
        WorkflowNextActivityDataDto workflowNextActivityDataDto,
        QuestionScreenSaveAndContinueCommandHandler sut
        )
        {
            //Arrange
            saveAndContinueCommand.Data!.ActivityType = ActivityTypeConstants.QuestionScreen;
            saveAndContinueMapper
                .Setup(x => x.SaveAndContinueCommandToMultiSaveAndContinueCommandDto(saveAndContinueCommand))
                .Returns(saveAndContinueCommandDto);

            elsaServerHttpClient.Setup(x => x.QuestionScreenSaveAndContinue(saveAndContinueCommandDto))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowNextActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

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
