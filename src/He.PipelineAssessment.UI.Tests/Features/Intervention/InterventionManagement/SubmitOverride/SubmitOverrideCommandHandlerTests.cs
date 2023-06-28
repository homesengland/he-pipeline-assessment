using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.Providers;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Override.SubmitOverride;
using MediatR;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.SubmitOverride
{
    public class SubmitOverrideCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_Throws_GivenADependencyThrowsException(
            [Frozen] Mock<IAssessmentRepository> repo,
            SubmitOverrideCommand command,
            Exception exception,
            SubmitOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).Throws(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(exception, ex);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Throws_GivenAssessmentInterventionNotFound(
            [Frozen] Mock<IAssessmentRepository> repo,
            SubmitOverrideCommand command,
            SubmitOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync((AssessmentIntervention?)null);

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {command.AssessmentInterventionId} not found", ex.Message);
        }

        [Theory]
        [InlineAutoMoqData(InterventionStatus.Rejected)]
        [InlineAutoMoqData(InterventionStatus.Pending)]
        public async Task Handle_UpdatesRepositoryAndReturns_GivenAssessmentInterventionStatusIsNotApproved(
            string status,
            [Frozen] Mock<IAssessmentRepository> repo,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
        SubmitOverrideCommand command,
            AssessmentIntervention intervention,
            SubmitOverrideCommandHandler sut
        )
        {
            //Arrange
            command.Status = status;
            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            dateTimeProvider.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(Unit.Value, result);
            repo.Verify(x => x.GetSubsequentWorkflowInstancesForOverride(intervention.AssessmentToolWorkflowInstance.WorkflowInstanceId), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_UpdatesRepositoryAndCreatesNextWorkflow_GivenAssessmentInterventionStatusIsApproved(
            [Frozen] Mock<IAssessmentRepository> repo,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            SubmitOverrideCommand command,
            AssessmentIntervention intervention,
            List<AssessmentToolWorkflowInstance> allWorkflowInstances,
            List<AssessmentToolWorkflowInstance> previousWorkflowInstances,
            SubmitOverrideCommandHandler sut
        )
        {
            //Arrange
            var lastWorkflowInstance = previousWorkflowInstances.OrderByDescending(x => x.CreatedDateTime).First();

            allWorkflowInstances.First().Id = intervention.AssessmentToolWorkflowInstanceId;

            command.Status = InterventionStatus.Approved;

            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);

            repo.Setup(x => x.GetSubsequentWorkflowInstancesForOverride(intervention
                .AssessmentToolWorkflowInstance.WorkflowInstanceId)).ReturnsAsync(allWorkflowInstances);

            repo.Setup(x => x.GetAssessmentToolInstanceNextWorkflow(intervention.AssessmentToolWorkflowInstanceId,
                intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId)).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);

            repo.Setup(x => x.CreateAssessmentToolInstanceNextWorkflows(null!));

            dateTimeProvider.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);
            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(Unit.Value, result);
           
            repo.Verify(x => x.SaveChanges(), Times.Exactly(1));
        }


    }
}
