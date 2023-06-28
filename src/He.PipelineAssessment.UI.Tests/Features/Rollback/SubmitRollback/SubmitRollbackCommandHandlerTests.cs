using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor;
using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using MediatR;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.SubmitRollback
{
    public class SubmitRollbackCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionNotFound(
            SubmitRollbackCommand command,
            SubmitRollbackCommandHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {command.AssessmentInterventionId} not found", ex.Message);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturn_GivenStatusNotApproved(
            [Frozen]Mock<IAssessmentRepository> repository,
            [Frozen]Mock<IDateTimeProvider> dateTimeProvider,
            AssessmentIntervention intervention,
            SubmitRollbackCommand command,
            SubmitRollbackCommandHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
                .ReturnsAsync(intervention);

            dateTimeProvider.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(Unit.Value,result);
            repository.Verify(x=>x.UpdateAssessmentIntervention(intervention),Times.Once);
            repository.Verify(x => x.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                intervention.TargetAssessmentToolWorkflow!.AssessmentTool.Order), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturn_GivenStatusApproved(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            AssessmentIntervention intervention,
            List<AssessmentToolWorkflowInstance> workflowsToDelete,
            SubmitRollbackCommand command,
            SubmitRollbackCommandHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
                .ReturnsAsync(intervention);

            dateTimeProvider.Setup(x => x.UtcNow()).Returns(DateTime.UtcNow);

            command.Status = InterventionStatus.Approved;

            repository.Setup(x =>
                    x.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                        intervention.TargetAssessmentToolWorkflow!.AssessmentTool.Order))
                .ReturnsAsync(workflowsToDelete);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(Unit.Value, result);
            repository.Verify(x => x.UpdateAssessmentIntervention(intervention), Times.Once);
            repository.Verify(x=> x.GetWorkflowInstancesToDeleteForRollback(intervention.AssessmentToolWorkflowInstance.AssessmentId,
                intervention.TargetAssessmentToolWorkflow!.AssessmentTool.Order),Times.Once);
            repository.Verify(x=>x.SaveChanges(),Times.Once);
        }
    }
}
