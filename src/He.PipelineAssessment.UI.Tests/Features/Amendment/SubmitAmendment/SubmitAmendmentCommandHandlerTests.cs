using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Amendment.CreateAmendment;
using He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment.SubmitAmendment
{
    public class SubmitAmendmentCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnErrorMessage_GivenInterventionRecordCannotBeFound(
       [Frozen] Mock<IInterventionService> interventionService,
            SubmitAmendmentCommand command,
            Exception e,
            SubmitAmendmentCommandHandler sut)
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
