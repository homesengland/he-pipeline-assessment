using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Override.EditOverride;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.EditOverride
{
    public class EditOverrideCommandHandlerTests
    {

        [Theory]
        [AutoMoqData]
        public async Task Handle_IgnoresThrownException_GivenInterventionServiceThrowsException(
            [Frozen] Mock<IInterventionService> interventionService,
            EditOverrideCommand command,
            Exception e,
            EditOverrideCommandHandler sut
        )
        {
            //Arrange
            interventionService.Setup(x => x.EditIntervention(command)).ThrowsAsync(e);

            //Act
            var ex =  await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsInterventionId_GivenSuccessfulUpdate(
            [Frozen] Mock<IInterventionService> interventionService,
            EditOverrideCommand command,
            AssessmentIntervention intervention,
            EditOverrideCommandHandler sut
        )
        {
            //Arrange
            interventionService.Setup(x => x.EditIntervention(command)).ReturnsAsync(intervention.Id);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(intervention.Id, result);
        }
    }
}
