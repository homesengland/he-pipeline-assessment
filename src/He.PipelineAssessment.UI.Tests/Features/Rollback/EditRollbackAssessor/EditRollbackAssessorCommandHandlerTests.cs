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
using He.PipelineAssessment.UI.Features.Rollback.EditRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.EditRollbackAssessor
{
    public class EditRollbackAssessorCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
            EditRollbackAssessorCommand command,
            EditRollbackAssessorCommandHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {command.AssessmentInterventionId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturn(
            [Frozen] Mock<IAssessmentRepository> repository,
            AssessmentIntervention intervention,
            EditRollbackAssessorCommand command,
            EditRollbackAssessorCommandHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
                .ReturnsAsync(intervention);


            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(intervention.Id, result);
            repository.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
