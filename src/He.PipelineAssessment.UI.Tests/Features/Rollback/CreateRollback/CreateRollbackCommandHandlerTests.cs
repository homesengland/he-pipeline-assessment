using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.CreateRollback
{
    public class CreateRollbackCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionServiceErrors(
            [Frozen] Mock<IInterventionService> interventionService,
            Exception e,
            CreateRollbackCommand command,
            CreateRollbackCommandHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.CreateAssessmentIntervention(command)).Throws(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnInt_GivenSuccessfulCallToInterventionService(
            [Frozen] Mock<IInterventionService> interventionService,
            CreateRollbackCommand command,
            CreateRollbackCommandHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.CreateAssessmentIntervention(command))
                .ReturnsAsync(123);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(123, result);
        }
    }
}
