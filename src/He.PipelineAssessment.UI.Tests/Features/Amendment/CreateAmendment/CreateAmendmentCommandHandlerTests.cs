using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Amendment.CreateAmendment;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment.CreateAmendment
{
    public class CreateAmendmentCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldNotHandleError_GivenInterventionServiceThrowsException(
            [Frozen] Mock<IInterventionService> interventionService,
            CreateAmendmentCommand command,
            Exception e,
            CreateAmendmentCommandHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.CreateAssessmentIntervention(command)).ThrowsAsync(e);

                //Act
                var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
            [AutoMoqData]
            public async Task Handle_ShouldReturnWithoutError_GivenNoErrorsThrownInInterventionService(
                [Frozen] Mock<IInterventionService> interventionService,
                CreateAmendmentCommand command,
                CreateAmendmentCommandHandler sut)
            {
            //Arrange
            interventionService.Setup(x => x.CreateAssessmentIntervention(command)).ReturnsAsync(1);

             //Act
             var result = await sut.Handle(command, CancellationToken.None);

             //Assert
             interventionService.Verify(x => x.CreateAssessmentIntervention(command), Times.Once);
            }
    }
}
