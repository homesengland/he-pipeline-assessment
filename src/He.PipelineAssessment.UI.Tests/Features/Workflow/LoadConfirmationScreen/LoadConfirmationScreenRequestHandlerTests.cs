using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            LoadConfirmationScreenRequest request,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync((WorkflowActivityDataDto?)null);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result);
            elsaServerHttpClient.Verify(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenNoAssessmentToolWorkflowInstanceFound(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            LoadConfirmationScreenRequest request,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync((AssessmentToolWorkflowInstance?)null);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result);
            elsaServerHttpClient.Verify(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsSaveAndContinueCommand_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            LoadConfirmationScreenRequest request,
            AssessmentToolWorkflowInstance stage,
            WorkflowActivityDataDto workflowActivityDataDto,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(stage);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<LoadConfirmationScreenResponse>(result);
            Assert.Equal(stage.Assessment.SpId.ToString(), result!.CorrelationId);
            Assert.Equal(stage.AssessmentId, result.AssessmentId);
            assessmentRepository.Verify(x => x.SaveChanges(), Times.Once);
            elsaServerHttpClient.Verify(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_CreatesNextWorkflows_GivenTheyDoNotExistAndNextWorkflowDefinitionIdsIsNotEmpty
            (
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
                LoadConfirmationScreenRequest request,
                WorkflowActivityDataDto workflowActivityDataDto,
                LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = "workflowDefinition1, workflowDefinition2";
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition1")).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition2")).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                    y.Count == 2 && y.Any(z => z.NextWorkflowDefinitionId == "workflowDefinition1"))));
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                    y.Count == 2 && y.Any(z => z.NextWorkflowDefinitionId == "workflowDefinition2"))));
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_CreatesOnlyNextWorkflowsWhichDoNotExist_GivenNextWorkflowDefinitionIdsIsNotEmpty
            (
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
                LoadConfirmationScreenRequest request,
                WorkflowActivityDataDto workflowActivityDataDto,
                AssessmentToolInstanceNextWorkflow nextWorkflow1,
                LoadConfirmationScreenRequestHandler sut
            )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = "workflowDefinition1, workflowDefinition2";
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition1")).ReturnsAsync(nextWorkflow1);
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition2")).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.Is<List<AssessmentToolInstanceNextWorkflow>>(y =>
                    y.First().NextWorkflowDefinitionId == "workflowDefinition2")));
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotCreateNextWorkflows_GivenTheyExistAndNextWorkflowDefinitionIdsIsNotEmpty
            (
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
                LoadConfirmationScreenRequest request,
                WorkflowActivityDataDto workflowActivityDataDto,
                AssessmentToolInstanceNextWorkflow nextWorkflow1,
                AssessmentToolInstanceNextWorkflow nextWorkflow2,
                LoadConfirmationScreenRequestHandler sut
            )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = "workflowDefinition1, workflowDefinition2";
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition1")).ReturnsAsync(nextWorkflow1);
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition2")).ReturnsAsync(nextWorkflow2);

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.Is<List<AssessmentToolInstanceNextWorkflow>>(z =>
                    z.Count == 2 && z.Any(y => y.NextWorkflowDefinitionId == "workflowDefinition1"))), Times.Never);
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.Is<List<AssessmentToolInstanceNextWorkflow>>(z =>
                    z.Count == 2 && z.Any(y => y.NextWorkflowDefinitionId == "workflowDefinition2"))), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotAttemptToCreateNextWorkflows_GivenNextWorkflowDefinitionIdsIsEmpty
            (
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
                LoadConfirmationScreenRequest request,
                WorkflowActivityDataDto workflowActivityDataDto,
                LoadConfirmationScreenRequestHandler sut
            )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = string.Empty;

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.IsAny<List<AssessmentToolInstanceNextWorkflow>>()), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotAttemptToCreateNextWorkflows_GivenNextWorkflowDefinitionIdsIsNull
            (
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
                LoadConfirmationScreenRequest request,
                WorkflowActivityDataDto workflowActivityDataDto,
                LoadConfirmationScreenRequestHandler sut
            )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.LoadConfirmationScreen(It.IsAny<LoadWorkflowActivityDto>()))
                .ReturnsAsync(workflowActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            workflowActivityDataDto.Data.NextWorkflowDefinitionIds = null;

            //Act
            await sut.Handle(request, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.IsAny<List<AssessmentToolInstanceNextWorkflow>>()), Times.Never);
        }
    }
}
