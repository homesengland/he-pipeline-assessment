using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Override.CreateOverride;
using He.PipelineAssessment.UI.Services;
using Moq;
using System;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotHandleThrownException_GivenInterventionServiceThrowsException(
            [Frozen] Mock<IInterventionService> interventionService,
            CreateOverrideCommand command,
            Exception exception,
            CreateOverrideCommandHandler sut
        )
        {
            //Arrange
            interventionService.Setup(x => x.CreateAssessmentIntervention(command)).ThrowsAsync(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsExpectedValue_GivenInterventionServiceDoesNotThrowExceptions(
            [Frozen] Mock<IInterventionService> interventionService,
            CreateOverrideCommand command,
            CreateOverrideCommandHandler sut
)
        {
            //Arrange
            interventionService.Setup(x => x.CreateAssessmentIntervention(command)).ReturnsAsync(1);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(1, result);
        }
    }
}
