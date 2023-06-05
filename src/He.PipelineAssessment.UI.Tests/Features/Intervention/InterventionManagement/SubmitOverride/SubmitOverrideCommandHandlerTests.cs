using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride;
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
        [InlineAutoMoqData(InterventionStatus.NotSubmitted)]
        [InlineAutoMoqData(InterventionStatus.Pending)]
        public async Task Handle_UpdatesRepositoryAndReturns_GivenAssessmentInterventionStatusIsNotApproved(
            string status,
            [Frozen] Mock<IAssessmentRepository> repo,
            SubmitOverrideCommand command,
            AssessmentIntervention intervention,
            SubmitOverrideCommandHandler sut
        )
        {
            //Arrange
            command.Status = status;
            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(Unit.Value, result);
            repo.Verify(x => x.GetSubsequentWorkflowInstances(intervention.AssessmentToolWorkflowInstance.WorkflowInstanceId), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_UpdatesRepositoryAndUpdatesLaterWorkflowInstancesToDelete_GivenAssessmentInterventionStatusIsApproved(
            [Frozen] Mock<IAssessmentRepository> repo,
            SubmitOverrideCommand command,
            AssessmentIntervention intervention,
            List<AssessmentToolWorkflowInstance> allWorkflowInstances,
            SubmitOverrideCommandHandler sut
        )
        {
            //Arrange
            allWorkflowInstances.First().Id = intervention.AssessmentToolWorkflowInstanceId;
            var allWorkflowInstancesCount = allWorkflowInstances.Count();
            command.Status = InterventionStatus.Approved;
            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            repo.Setup(x => x.GetSubsequentWorkflowInstances(intervention
                .AssessmentToolWorkflowInstance.WorkflowInstanceId)).ReturnsAsync(allWorkflowInstances);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(Unit.Value, result);
            Assert.All(allWorkflowInstances, instance => Assert.Equal(AssessmentToolWorkflowInstanceConstants.Deleted, instance.Status));
            Assert.DoesNotContain(allWorkflowInstances, x => x.Id == intervention.AssessmentToolWorkflowInstanceId);
            Assert.Equal(allWorkflowInstances.Count, allWorkflowInstancesCount - 1);
            repo.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_UpdatesRepositoryAndUpdatesNextWorkflowToNotStartedIfItExists_GivenAssessmentInterventionStatusIsApproved(
            [Frozen] Mock<IAssessmentRepository> repo,
            SubmitOverrideCommand command,
            AssessmentIntervention intervention,
            List<AssessmentToolWorkflowInstance> allWorkflowInstances,
            AssessmentToolInstanceNextWorkflow nextWorkflow,
            SubmitOverrideCommandHandler sut
        )
        {
            //Arrange
            nextWorkflow.IsStarted = true;
            allWorkflowInstances.First().Id = intervention.AssessmentToolWorkflowInstanceId;
            command.Status = InterventionStatus.Approved;
            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            repo.Setup(x => x.GetSubsequentWorkflowInstances(intervention
                .AssessmentToolWorkflowInstance.WorkflowInstanceId)).ReturnsAsync(allWorkflowInstances);
            repo.Setup(x => x.GetAssessmentToolInstanceNextWorkflow(intervention.AssessmentToolWorkflowInstanceId,
                intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId)).ReturnsAsync(nextWorkflow);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(Unit.Value, result);
            Assert.False(nextWorkflow.IsStarted);
            repo.Verify(x => x.SaveChanges(), Times.Exactly(2));
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_UpdatesRepositoryAndCreatesNextWorkflowIfItDoesNotExist_GivenAssessmentInterventionStatusIsApproved(
            [Frozen] Mock<IAssessmentRepository> repo,
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
            repo.Setup(x => x.GetSubsequentWorkflowInstances(intervention
                .AssessmentToolWorkflowInstance.WorkflowInstanceId)).ReturnsAsync(allWorkflowInstances);
            repo.Setup(x => x.GetPreviousAssessmentToolWorkflowInstances(intervention
                .AssessmentToolWorkflowInstance.WorkflowInstanceId)).ReturnsAsync(previousWorkflowInstances);
            repo.Setup(x => x.GetAssessmentToolInstanceNextWorkflow(intervention.AssessmentToolWorkflowInstanceId,
                intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId)).ReturnsAsync((AssessmentToolInstanceNextWorkflow?)null);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(Unit.Value, result);

            var nextWorkflowPredicate = (AssessmentToolInstanceNextWorkflow x) => x.IsStarted == false &&
                                                               x.AssessmentId == lastWorkflowInstance.AssessmentId &&
                                                               x.AssessmentToolWorkflowInstanceId ==
                                                               lastWorkflowInstance.Id && x.NextWorkflowDefinitionId ==
                                                               intervention.TargetAssessmentToolWorkflow!.WorkflowDefinitionId;

            repo.Verify(x => x.CreateAssessmentToolInstanceNextWorkflows(
                It.Is<List<AssessmentToolInstanceNextWorkflow>>(y => y.Count == 1 && nextWorkflowPredicate(y.Single())
                )));
            repo.Verify(x => x.SaveChanges(), Times.Exactly(1));
            repo.Verify(x => x.DeleteSubsequentNextWorkflows(It.Is<AssessmentToolInstanceNextWorkflow?>(y => nextWorkflowPredicate(y!))), Times.Exactly(1));
        }


    }
}
