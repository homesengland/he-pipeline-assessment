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
            AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
            CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            CheckYourAnswersSaveAndContinueCommandHandler sut
        )
        {
            //Arrange

            elsaServerHttpClient.Setup(x => x.CheckYourAnswersSaveAndContinue(It.IsAny<CheckYourAnswersSaveAndContinueCommandDto>()))
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
        public async Task Handle_CreatesNextWorkflows_GivenTheyDoNotExistAndNextWorkflowDefinitionIdsIsNotEmpty
            (
                [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
                [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                AssessmentToolWorkflowInstance assessmentToolWorkflowInstance,
                CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
                WorkflowNextActivityDataDto workflowNextActivityDataDto,
                CheckYourAnswersSaveAndContinueCommandHandler sut
            )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.CheckYourAnswersSaveAndContinue(It.IsAny<CheckYourAnswersSaveAndContinueCommandDto>()))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowNextActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            workflowNextActivityDataDto.Data.NextWorkflowDefinitionIds = "workflowDefinition1, workflowDefinition2";
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition1")).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition2")).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);

            //Act
            await sut.Handle(saveAndContinueCommand, CancellationToken.None);

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
                CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
                WorkflowNextActivityDataDto workflowNextActivityDataDto,
                AssessmentToolInstanceNextWorkflow nextWorkflow1,
                CheckYourAnswersSaveAndContinueCommandHandler sut
            )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.CheckYourAnswersSaveAndContinue(It.IsAny<CheckYourAnswersSaveAndContinueCommandDto>()))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowNextActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            workflowNextActivityDataDto.Data.NextWorkflowDefinitionIds = "workflowDefinition1, workflowDefinition2";
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition1")).ReturnsAsync(nextWorkflow1);
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition2")).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);

            //Act
            await sut.Handle(saveAndContinueCommand, CancellationToken.None);

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
                CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
                WorkflowNextActivityDataDto workflowNextActivityDataDto,
                AssessmentToolInstanceNextWorkflow nextWorkflow1,
                AssessmentToolInstanceNextWorkflow nextWorkflow2,
                CheckYourAnswersSaveAndContinueCommandHandler sut
            )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.CheckYourAnswersSaveAndContinue(It.IsAny<CheckYourAnswersSaveAndContinueCommandDto>()))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowNextActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            workflowNextActivityDataDto.Data.NextWorkflowDefinitionIds = "workflowDefinition1, workflowDefinition2";
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition1")).ReturnsAsync(nextWorkflow1);
            assessmentRepository
                .Setup(x => x.GetAssessmentToolInstanceNextWorkflow(assessmentToolWorkflowInstance.Id,
                    "workflowDefinition2")).ReturnsAsync(nextWorkflow2);

            //Act
            await sut.Handle(saveAndContinueCommand, CancellationToken.None);

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
                CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
                WorkflowNextActivityDataDto workflowNextActivityDataDto,
                CheckYourAnswersSaveAndContinueCommandHandler sut
            )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.CheckYourAnswersSaveAndContinue(It.IsAny<CheckYourAnswersSaveAndContinueCommandDto>()))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowNextActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            workflowNextActivityDataDto.Data.NextWorkflowDefinitionIds = string.Empty;

            //Act
            await sut.Handle(saveAndContinueCommand, CancellationToken.None);

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
            CheckYourAnswersSaveAndContinueCommand saveAndContinueCommand,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            CheckYourAnswersSaveAndContinueCommandHandler sut
        )
        {
            //Arrange
            elsaServerHttpClient.Setup(x => x.CheckYourAnswersSaveAndContinue(It.IsAny<CheckYourAnswersSaveAndContinueCommandDto>()))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolWorkflowInstance(workflowNextActivityDataDto.Data.WorkflowInstanceId))
                .ReturnsAsync(assessmentToolWorkflowInstance);
            workflowNextActivityDataDto.Data.NextWorkflowDefinitionIds = null;

            //Act
            await sut.Handle(saveAndContinueCommand, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x =>
                x.CreateAssessmentToolInstanceNextWorkflows(It.IsAny<List<AssessmentToolInstanceNextWorkflow>>()), Times.Never);
        }
    }
}
