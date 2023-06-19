using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using Xunit;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using Moq;
using MediatR;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.ConfirmRollback
{
    public class ConfirmRollbackCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorMessage_GivenInterventionRecordCannotBeFound(
           
            ConfirmRollbackCommand command,
            ConfirmRollbackCommandHandler sut)
        {
            //Arrange

            //Act

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {command.AssessmentInterventionId} not found", ex.Message);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Return_GivenInterventionRecordCanBeFound(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            ConfirmRollbackCommand command,
            AssessmentIntervention intervention,
            ConfirmRollbackCommandHandler sut)
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
                .ReturnsAsync(intervention);

            //Act

            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            assessmentRepository.Verify(x=>x.UpdateAssessmentIntervention(It.IsAny<AssessmentIntervention>()),Times.Once);
            Assert.Equal(Unit.Value,result);

        }
    }
}
