using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
            CheckYourAnswersSaveAndContinueCommandHandler sut
        )
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.CheckYourAnswersSaveAndContinue(It.IsAny<CheckYourAnswersSaveAndContinueCommandDto>()))
                .ReturnsAsync((WorkflowNextActivityDataDto?)null);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsCheckYourAnswersSaveAndContinueCommandResponse_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            AssessmentToolWorkFlowInstance assessmentStage,
            CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            CheckYourAnswersSaveAndContinueCommandHandler sut
        )
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.CheckYourAnswersSaveAndContinue(It.IsAny<CheckYourAnswersSaveAndContinueCommandDto>()))
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
