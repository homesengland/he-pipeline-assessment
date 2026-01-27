using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
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
        public async Task Handle_ThrowsApplicationException_GivenHttpClientResponseIsNull(
           [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           [Frozen] Mock<IRoleValidation> roleValidation,
           LoadQuestionScreenRequest loadWorkflowActivityRequest,
           AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
           LoadQuestionScreenRequestHandler sut)
        {
            //Arrange

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadWorkflowActivityRequest.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            loadWorkflowActivityRequest.IsReadOnly = false;
            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(()=>sut.Handle(loadWorkflowActivityRequest, CancellationToken.None));

            //Assert
            Assert.Equal("Failed to load Question Screen activity.", ex.Message);
            elsaServerHttpClient.Verify(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsReadonly_GivenStatusIsNotDraft(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            LoadQuestionScreenRequestHandler sut)
        {
            //Arrange

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadWorkflowActivityRequest.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Submitted;

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
           Assert.True(result!.IsReadOnly);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            WorkflowActivityDataDto workflowActivityDataDto,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            LoadQuestionScreenRequestHandler sut)
        {
            //Arrange
            loadWorkflowActivityRequest.IsReadOnly = false;
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadWorkflowActivityRequest.WorkflowInstanceId))
               .ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<QuestionScreenSaveAndContinueCommand>(result);
            elsaServerHttpClient.Verify(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommandWithIsAuthorisedFalse_GivenIncorrectRole(
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           [Frozen] Mock<IRoleValidation> roleValidation,
           LoadQuestionScreenRequest loadWorkflowActivityRequest,
           AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
           LoadQuestionScreenRequestHandler sut)
        {
            //Arrange
            loadWorkflowActivityRequest.IsReadOnly = false;
            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadWorkflowActivityRequest.WorkflowInstanceId))
               .ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(false);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.False(result!.IsAuthorised);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenUserCannotViewSensitiveRecords(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            LoadQuestionScreenRequest loadWorkflowActivityRequest,
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            LoadQuestionScreenRequestHandler sut)
        {
            //Arrange

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadWorkflowActivityRequest.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(false);

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(loadWorkflowActivityRequest, CancellationToken.None));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);

        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_GivenException(
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           LoadQuestionScreenRequest loadWorkflowActivityRequest,
           LoadQuestionScreenRequestHandler sut)
        {
            //Arrange

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadWorkflowActivityRequest.WorkflowInstanceId))
               .ThrowsAsync(new Exception());

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(()=>sut.Handle(loadWorkflowActivityRequest, CancellationToken.None));

            //Assert
            Assert.Equal("Failed to load Question Screen activity.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenUserIsWhitelistedForSensitiveRecord(
           [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           [Frozen] Mock<IRoleValidation> roleValidation,
           LoadQuestionScreenRequest loadQuestionScreenRequest,
           WorkflowActivityDataDto workflowActivityDataDto,
           AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
           LoadQuestionScreenRequestHandler sut)
        {
            //Arrange
            assessmentToolWorkflowInstance.Assessment.SensitiveStatus = "Sensitive - NDA in place";
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            loadQuestionScreenRequest.IsReadOnly = false;

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadQuestionScreenRequest.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

            // User is not admin/PM, but is whitelisted
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(false);
            roleValidation.Setup(x => x.IsUserWhitelistedForSensitiveRecord(assessmentToolWorkflowInstance.AssessmentId))
                .ReturnsAsync(true);
            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            //Act
            var result = await sut.Handle(loadQuestionScreenRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<QuestionScreenSaveAndContinueCommand>(result);
            roleValidation.Verify(x => x.IsUserWhitelistedForSensitiveRecord(assessmentToolWorkflowInstance.AssessmentId), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsUnauthorizedException_GivenUserIsNotWhitelistedForSensitiveRecord(
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           [Frozen] Mock<IRoleValidation> roleValidation,
           LoadQuestionScreenRequest loadQuestionScreenRequest,
           AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
           LoadQuestionScreenRequestHandler sut)
        {
            //Arrange
            assessmentToolWorkflowInstance.Assessment.SensitiveStatus = "Sensitive - NDA in place";
            loadQuestionScreenRequest.IsReadOnly = false;

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadQuestionScreenRequest.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);

            // User is not admin/PM and is NOT whitelisted
            roleValidation.Setup(x => x.ValidateSensitiveRecords(assessmentToolWorkflowInstance.Assessment)).Returns(false);
            roleValidation.Setup(x => x.IsUserWhitelistedForSensitiveRecord(assessmentToolWorkflowInstance.AssessmentId))
                .ReturnsAsync(false);

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(loadQuestionScreenRequest, CancellationToken.None));

            //Assert
            Assert.Equal("You do not have permission to access this resource.", ex.Message);
            roleValidation.Verify(x => x.IsUserWhitelistedForSensitiveRecord(assessmentToolWorkflowInstance.AssessmentId), Times.Once);
        }
    }
}
