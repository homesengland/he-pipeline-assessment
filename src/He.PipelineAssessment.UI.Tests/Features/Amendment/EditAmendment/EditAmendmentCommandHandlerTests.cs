using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Amendment.DeleteAmendment;
using He.PipelineAssessment.UI.Features.Amendment.EditAmendment;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment.EditAmendment
{
    public class EditAmendmentCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldIgnoreError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
                        [Frozen] Mock<IInterventionService> interventionService,
            EditAmendmentCommand command,
            Exception e,
            EditAmendmentCommandHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.EditIntervention(command))
                .ThrowsAsync(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnInt_GivenSuccessfulEdit(
            [Frozen] Mock<IInterventionService> interventionService,
            EditAmendmentCommand command,
            EditAmendmentCommandHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.EditIntervention(command)).ReturnsAsync(1);
            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(1, result);
            Assert.IsType<int>(result);
        }
    }
}
