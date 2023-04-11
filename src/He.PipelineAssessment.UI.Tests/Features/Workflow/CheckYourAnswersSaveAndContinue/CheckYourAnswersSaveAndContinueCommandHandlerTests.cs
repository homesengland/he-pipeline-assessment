﻿using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
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
            [Frozen] Mock<IRoleValidation> roleValidation,
            CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
            CheckYourAnswersSaveAndContinueCommandHandler sut
        )
        {
            //Arrange

            roleValidation.Setup(x => x.ValidateRole(saveAndContinueCommand.AssessmentId)).ReturnsAsync(true);

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
            [Frozen] Mock<IRoleValidation> roleValidation,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            CheckYourAnswersSaveAndContinueCommandHandler sut
        )
        {
            //Arrange

            roleValidation.Setup(x => x.ValidateRole(saveAndContinueCommand.AssessmentId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.CheckYourAnswersSaveAndContinue(It.IsAny<CheckYourAnswersSaveAndContinueCommandDto>()))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowNextActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.True(result!.IsCorrectBusinessArea);
            Assert.Equal(workflowNextActivityDataDto.Data.NextActivityId, result!.ActivityId);
            Assert.Equal(workflowNextActivityDataDto.Data.WorkflowInstanceId, result.WorkflowInstanceId);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenIncorrectBusinessArea(

            [Frozen] Mock<IRoleValidation> roleValidation,
            CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
            CheckYourAnswersSaveAndContinueCommandHandler sut
        )
        {
            //Arrange

            roleValidation.Setup(x => x.ValidateRole(saveAndContinueCommand.AssessmentId)).ReturnsAsync(false);


            //Act
            var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            Assert.False(result!.IsCorrectBusinessArea);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenException(
          [Frozen] Mock<IRoleValidation> roleValidation,
          CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
          CheckYourAnswersSaveAndContinueCommandHandler sut
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
