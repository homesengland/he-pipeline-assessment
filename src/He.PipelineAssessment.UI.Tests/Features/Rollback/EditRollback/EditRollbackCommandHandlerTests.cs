using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Rollback.EditRollback;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.EditRollback
{
    public class EditRollbackCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionServiceErrors(
            [Frozen] Mock<IInterventionService> interventionService,
            Exception e,
            EditRollbackCommand command,
            EditRollbackCommandHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.EditIntervention(command)).Throws(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnInt_GivenSuccessfulCallToInterventionService(
            [Frozen] Mock<IInterventionService> interventionService,
            EditRollbackCommand command,
            EditRollbackCommandHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.EditIntervention(command))
                .ReturnsAsync(123);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(123, result);
        }
    }
}
