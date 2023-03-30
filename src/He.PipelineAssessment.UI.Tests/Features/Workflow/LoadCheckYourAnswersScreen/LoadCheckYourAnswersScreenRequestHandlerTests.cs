using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
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
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
           [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
           LoadCheckYourAnswersScreenRequest loadCheckYourAnswersScreenRequest,
           LoadCheckYourAnswersScreenRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadCheckYourAnswersScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            //Act
            var result = await sut.Handle(loadCheckYourAnswersScreenRequest, CancellationToken.None);

            //Assert
            Assert.Null(result);
            elsaServerHttpClient.Verify(x => x.LoadCheckYourAnswersScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
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
            Assert.IsType<QuestionScreenSaveAndContinueCommand>(result);
            Assert.Equal(assessmentToolWorkflowInstance.Assessment.SpId.ToString(), result!.CorrelationId);
            Assert.Equal(assessmentToolWorkflowInstance.AssessmentId, result.AssessmentId);
            elsaServerHttpClient.Verify(x => x.LoadCheckYourAnswersScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenErrorsEncountered(
           [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
           LoadCheckYourAnswersScreenRequest loadCheckYourAnswersScreenRequest,
           LoadCheckYourAnswersScreenRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadCheckYourAnswersScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .Throws(new Exception("There is an issue"));

            //Act
            var result = await sut.Handle(loadCheckYourAnswersScreenRequest, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

    }
}
