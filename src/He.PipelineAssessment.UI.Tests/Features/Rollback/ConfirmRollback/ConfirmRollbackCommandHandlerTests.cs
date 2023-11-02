using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using Xunit;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using Moq;
using MediatR;
using He.PipelineAssessment.UI.Services;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.ConfirmRollback
{
    public class ConfirmRollbackCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenInterventionServiceThrowsException(
            [Frozen] Mock<IInterventionService> service,
            Exception e,
            ConfirmRollbackCommand command,
            ConfirmRollbackCommandHandler sut)
        {
            //Arrange
            service.Setup(x => x.ConfirmIntervention(command)).Throws(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Return_GivenNoExceptionThrownByInterventionService(
            [Frozen] Mock<IInterventionService> service,
            ConfirmRollbackCommand command,
            AssessmentIntervention intervention,
            ConfirmRollbackCommandHandler sut)
        {
            //Arrange
            service.Setup(x => x.ConfirmIntervention(command))
                .Returns(Task.CompletedTask);

            //Act

            await sut.Handle(command, CancellationToken.None);

            //Assert
            service.Verify(x=>x.ConfirmIntervention(command),Times.Once);
        }
    }
}
