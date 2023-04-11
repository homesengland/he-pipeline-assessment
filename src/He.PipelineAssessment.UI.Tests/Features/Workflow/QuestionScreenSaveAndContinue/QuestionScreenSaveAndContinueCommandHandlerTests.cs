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
using He.PipelineAssessment.UI.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.SaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommandHandlerTests
    {

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IQuestionScreenSaveAndContinueMapper> saveAndContinueMapper,
            [Frozen] Mock<IRoleValidation> roleValidation,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            QuestionScreenSaveAndContinueCommandDto saveAndContinueCommandDto,
            QuestionScreenSaveAndContinueCommandHandler sut
        )
        {
            //Arrange
            roleValidation.Setup(x => x.ValidateRole(saveAndContinueCommand.AssessmentId)).ReturnsAsync(true);

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
            [Frozen] Mock<IRoleValidation> roleValidation,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            QuestionScreenSaveAndContinueCommandDto saveAndContinueCommandDto,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            QuestionScreenSaveAndContinueCommandHandler sut
        )
        {
            //Arrange
            roleValidation.Setup(x => x.ValidateRole(saveAndContinueCommand.AssessmentId)).ReturnsAsync(true);

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
        public async Task Handle_ReturnsLoadWorkflowActivityRequest_GivenIncorrectRole(
          QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
          QuestionScreenSaveAndContinueCommandHandler sut
      )
        {
            //Arrange
           

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            Assert.False(result!.IsCorrectBusinessArea);
        }



        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsLoadWorkflowActivityRequest_ErrorsOccur(
          [Frozen] Mock<IRoleValidation> roleValidation,
          QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
          QuestionScreenSaveAndContinueCommandHandler sut
      )
        {
            //Arrange
            roleValidation.Setup(x => x.ValidateRole(saveAndContinueCommand.AssessmentId)).ThrowsAsync(new Exception());

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

    }
}
