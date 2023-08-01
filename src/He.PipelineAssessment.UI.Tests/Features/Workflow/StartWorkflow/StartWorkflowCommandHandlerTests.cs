using AutoFixture.Xunit2;
using Azure;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.StartWorkflow;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_GivenHttpClientResponseIsNull(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IRoleValidation> roleValidation,
            StartWorkflowCommand command,
            StartWorkflowCommandHandler sut)
        {
            //Arrange
            roleValidation.Setup(x => x.ValidateRole(command.AssessmentId, command.WorkflowDefinitionId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.PostStartWorkflow(It.IsAny<StartWorkflowCommandDto>()))
                .ReturnsAsync((WorkflowNextActivityDataDto?)null);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(()=>sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal("Failed to start workflow",ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsLoadWorkflowActivityRequest_GivenNoErrorsOccur(
            [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            StartWorkflowCommand command,
            AssessmentToolInstanceNextWorkflow assessmentToolInstanceNextWorkflow,
            WorkflowNextActivityDataDto workflowNextActivityDataDto,
            StartWorkflowCommandHandler sut)
        {
            //Arrange
            roleValidation.Setup(x => x.ValidateRole(command.AssessmentId, command.WorkflowDefinitionId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.PostStartWorkflow(It.IsAny<StartWorkflowCommandDto>()))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolInstanceNextWorkflowByAssessmentId(command.AssessmentId, command.WorkflowDefinitionId))
                .ReturnsAsync(assessmentToolInstanceNextWorkflow);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(workflowNextActivityDataDto.Data.NextActivityId, result!.ActivityId);
            Assert.Equal(workflowNextActivityDataDto.Data.WorkflowInstanceId, result.WorkflowInstanceId);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsLoadWorkflowActivityRequest_GivenIncorrectRole(
           StartWorkflowCommand command,
           StartWorkflowCommandHandler sut)
        {
            //Arrange
           
            //Act
            var exceptionThrown = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal("Failed to start workflow", exceptionThrown.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsApplicationException_ErrorsOccur(
            [Frozen] Mock<IRoleValidation> roleValidation,
            StartWorkflowCommand command,
            StartWorkflowCommandHandler sut)
        {
            //Arrange
            roleValidation.Setup(x => x.ValidateRole(command.AssessmentId, command.WorkflowDefinitionId)).ThrowsAsync(new Exception());

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(()=>sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal("Failed to start workflow", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ToolWorkflowInstanceCreatedWithCorrectValues(
    [Frozen] Mock<IElsaServerHttpClient> elsaServerHttpClient,
    [Frozen] Mock<IRoleValidation> roleValidation,
    [Frozen] Mock<IAssessmentRepository> assessmentRepository,
    StartWorkflowCommand command,
    AssessmentToolInstanceNextWorkflow assessmentToolInstanceNextWorkflow,
    WorkflowNextActivityDataDto workflowNextActivityDataDto,
    StartWorkflowCommandHandler sut)
        {
            //Arrange
            roleValidation.Setup(x => x.ValidateRole(command.AssessmentId, command.WorkflowDefinitionId)).ReturnsAsync(true);

            elsaServerHttpClient.Setup(x => x.PostStartWorkflow(It.IsAny<StartWorkflowCommandDto>()))
                .ReturnsAsync(workflowNextActivityDataDto);

            assessmentRepository.Setup(x => x.GetAssessmentToolInstanceNextWorkflowByAssessmentId(command.AssessmentId, command.WorkflowDefinitionId))
                .ReturnsAsync(assessmentToolInstanceNextWorkflow);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x => x.CreateAssessmentToolWorkflowInstance(It.Is<AssessmentToolWorkflowInstance>(y =>
            y.FirstActivityId == workflowNextActivityDataDto.Data.FirstActivityId
            && y.FirstActivityType == workflowNextActivityDataDto.Data.FirstActivityType
            && y.CurrentActivityId == workflowNextActivityDataDto.Data.NextActivityId
            && y.CurrentActivityType == workflowNextActivityDataDto.Data.ActivityType
            && y.WorkflowInstanceId == workflowNextActivityDataDto.Data.WorkflowInstanceId
            && y.AssessmentId == command.AssessmentId
            && y.Status == AssessmentToolWorkflowInstanceConstants.Draft
            && y.WorkflowName == workflowNextActivityDataDto.Data.WorkflowName
            && y.WorkflowDefinitionId == command.WorkflowDefinitionId
            && y.AssessmentToolWorkflowId == command.AssessmentToolWorkflowId)), Times.Once);
        }
    }
}
