using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.LoadCheckYourAnswersScreen;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersScreenRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_GivenHttpClientResponseIsNull(
           [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           [Frozen] Mock<IRoleValidation> roleValidation,
           LoadCheckYourAnswersScreenRequest loadCheckYourAnswersScreenRequest,
           AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
           LoadCheckYourAnswersScreenRequestHandler sut)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadCheckYourAnswersScreenRequest.WorkflowInstanceId))
               .ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.LoadCheckYourAnswersScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(()=> sut.Handle(loadCheckYourAnswersScreenRequest, CancellationToken.None));

            //Assert
            elsaServerHttpClient.Verify(x => x.LoadCheckYourAnswersScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
            Assert.Equal("Failed to load check your answers screen activity", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            LoadCheckYourAnswersScreenRequest loadCheckYourAnswersScreenRequest,
            WorkflowActivityDataDto workflowActivityDataDto,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            LoadCheckYourAnswersScreenRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadCheckYourAnswersScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadCheckYourAnswersScreenRequest.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

            //Act
            var result = await sut.Handle(loadCheckYourAnswersScreenRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.True(result!.IsAuthorised);
            Assert.IsType<QuestionScreenSaveAndContinueCommand>(result);
            Assert.Equal(assessmentToolWorkflowInstance.Assessment.SpId, result!.CorrelationId);
            Assert.Equal(assessmentToolWorkflowInstance.AssessmentId, result.AssessmentId);
            elsaServerHttpClient.Verify(x => x.LoadCheckYourAnswersScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_GivenErrorsEncountered(
           [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
           LoadCheckYourAnswersScreenRequest loadCheckYourAnswersScreenRequest,
           LoadCheckYourAnswersScreenRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadCheckYourAnswersScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .Throws(new Exception("There is an issue"));

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(loadCheckYourAnswersScreenRequest, CancellationToken.None));

            //Assert
            Assert.Equal("Failed to load check your answers screen activity", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_GivenIncorrectBusinessArea(
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           [Frozen] Mock<IRoleValidation> roleValidation,
           LoadCheckYourAnswersScreenRequest loadCheckYourAnswersScreenRequest,
           AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
           LoadCheckYourAnswersScreenRequestHandler sut)
        {
            //Arrange
            loadCheckYourAnswersScreenRequest.IsReadOnly = false;

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadCheckYourAnswersScreenRequest.WorkflowInstanceId))
              .ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(false);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(loadCheckYourAnswersScreenRequest, CancellationToken.None));

            //Assert
            Assert.Equal("Failed to load check your answers screen activity", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldThrowException_GivenRoleValidationReturnsFalse(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            LoadCheckYourAnswersScreenRequest loadCheckYourAnswersScreenRequest,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            LoadCheckYourAnswersScreenRequestHandler sut)
        {
            //TODO: come back to this as the logic needs to change
            //Arrange
            loadCheckYourAnswersScreenRequest.IsReadOnly = false;
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadCheckYourAnswersScreenRequest.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x =>
                    x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(false);

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(loadCheckYourAnswersScreenRequest, CancellationToken.None));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);
            Assert.False(true);
        }

    }
}
