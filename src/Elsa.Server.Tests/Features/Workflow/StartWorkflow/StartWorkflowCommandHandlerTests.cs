using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Features.Workflow.SubmitAssessmentStage;
using Elsa.Server.Models;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            [Frozen] Mock<IStartsWorkflow> startsWorkflow,
            [Frozen] Mock<IStartWorkflowMapper> startWorkflowMapper,
            [Frozen] Mock<IElsaCustomRepository> pipelineAssessmentRepository,
            WorkflowBlueprint workflowBlueprint,
            ActivityBlueprint activityBlueprint,
            RunWorkflowResult runWorkflowResult,
            StartWorkflowCommand startWorkflowCommand,
            AssessmentQuestion assessmentQuestion,
            StartWorkflowResponse startWorkflowResponse,
            string workflowName,
            StartWorkflowCommandHandler sut)
        {

            //Arrange

            var opResult = new OperationResult<StartWorkflowResponse>()
            {
                Data = startWorkflowResponse,
                ErrorMessages = new List<string>(),
                ValidationMessages = new List<string>()
            };

            activityBlueprint.Id = runWorkflowResult.WorkflowInstance!.LastExecutedActivityId!;
            workflowBlueprint.Activities.Add(activityBlueprint);


            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, null, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            startWorkflowMapper.Setup(x => x.RunWorkflowResultToAssessmentQuestion(runWorkflowResult, activityBlueprint, workflowName))
                .Returns(assessmentQuestion);

            startWorkflowMapper.Setup(x => x.RunWorkflowResultToStartWorkflowResponse(runWorkflowResult))
                .Returns(opResult.Data);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            pipelineAssessmentRepository.Verify(x => x.CreateAssessmentQuestionAsync(assessmentQuestion, CancellationToken.None), Times.Once);
            Assert.Equal(opResult.Data.NextActivityId, result.Data!.NextActivityId);
            Assert.Equal(opResult.Data.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Empty(result.ErrorMessages);
            Assert.Empty(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorOperationResult_WhenCannotDeserialiseWorkflowResult(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            [Frozen] Mock<IStartsWorkflow> startsWorkflow,
            [Frozen] Mock<IStartWorkflowMapper> startWorkflowMapper,
            [Frozen] Mock<IElsaCustomRepository> pipelineAssessmentRepository,
            WorkflowBlueprint workflowBlueprint,
            ActivityBlueprint activityBlueprint,
            RunWorkflowResult runWorkflowResult,
            StartWorkflowCommand startWorkflowCommand,
            string workflowName,
            StartWorkflowCommandHandler sut)
        {
            activityBlueprint.Id = runWorkflowResult.WorkflowInstance!.LastExecutedActivityId!;
            workflowBlueprint.Activities.Add(activityBlueprint);

            //Arrange
            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, null, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            startWorkflowMapper.Setup(x => x.RunWorkflowResultToAssessmentQuestion(runWorkflowResult, activityBlueprint, workflowName))
                .Returns((AssessmentQuestion?)null);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            pipelineAssessmentRepository.Verify(x => x.CreateAssessmentQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Never);
            Assert.Null(result.Data);
            Assert.Equal("Failed to deserialize RunWorkflowResult", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorOperationResult_WhenADependencyThrows(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            [Frozen] Mock<IElsaCustomRepository> pipelineAssessmentRepository,
            StartWorkflowCommand startWorkflowCommand,
            Exception exception,
            StartWorkflowCommandHandler sut)
        {
            //Arrange
            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .Throws(exception);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            pipelineAssessmentRepository.Verify(x => x.CreateAssessmentQuestionAsync(It.IsAny<AssessmentQuestion>(), CancellationToken.None), Times.Never);
            Assert.Null(result.Data);
            Assert.Equal(exception.Message, result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorMessageResult_WhenActivityIsNull(
        [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
        [Frozen] Mock<IStartsWorkflow> startsWorkflow,
        WorkflowBlueprint workflowBlueprint,
        RunWorkflowResult runWorkflowResult,
        StartWorkflowCommand startWorkflowCommand,
        StartWorkflowCommandHandler sut)
        {
            //Arrange
            workflowRegistry
                .Setup(x => x.FindAsync(startWorkflowCommand.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            startsWorkflow.Setup(x => x.StartWorkflowAsync(workflowBlueprint, null, null, null, null, null, CancellationToken.None))
                .ReturnsAsync(runWorkflowResult);

            //Act
            var result = await sut.Handle(startWorkflowCommand, CancellationToken.None);

            //Assert
            Assert.Equal(1, result.ErrorMessages.Count);
            Assert.Equal("Failed to get activity", result.ErrorMessages.Single());
        }
    }
}
