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
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
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

            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result);
            elsaServerHttpClient.Verify(x => x.LoadQuestionScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
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

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadWorkflowActivityRequest.WorkflowInstanceId))
               .ReturnsAsync(assessmentToolWorkflowInstance);

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
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenIncorrectRole(
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           [Frozen] Mock<IRoleValidation> roleValidation,
           LoadQuestionScreenRequest loadWorkflowActivityRequest,
           AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
           LoadQuestionScreenRequestHandler sut)
        {
            //Arrange

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadWorkflowActivityRequest.WorkflowInstanceId))
               .ReturnsAsync(assessmentToolWorkflowInstance);

            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(false);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.False(result!.IsCorrectBusinessArea);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenException(
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           [Frozen] Mock<IRoleValidation> roleValidation,
           LoadQuestionScreenRequest loadWorkflowActivityRequest,
           AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
           LoadQuestionScreenRequestHandler sut)
        {
            //Arrange

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(loadWorkflowActivityRequest.WorkflowInstanceId))
               .ThrowsAsync(new Exception());

            loadWorkflowActivityRequest.IsReadOnly = true;

            roleValidation.Setup(x => x.ValidateRole(assessmentToolWorkflowInstance.AssessmentId, assessmentToolWorkflowInstance.WorkflowDefinitionId)).ReturnsAsync(false);

            //Act
            var result = await sut.Handle(loadWorkflowActivityRequest, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }
    }
}
