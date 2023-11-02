using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.SubmitRollback
{
    public class SubmitRollbackCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionServiceThrows(
            [Frozen] Mock<IInterventionService> interventionService,
            SubmitRollbackCommand command,
            Exception e,
            SubmitRollbackCommandHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.SubmitIntervention(command)).Throws(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }
    }
}
