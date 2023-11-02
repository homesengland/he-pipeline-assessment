using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Amendment.DeleteAmendment;
using He.PipelineAssessment.UI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment.DeleteAmendment
{
    public class DeleteAmendmentCommandHandlerTests
    {

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldIgnoreError_GivenInterventionServiceThrowsException(
            [Frozen] Mock<IInterventionService> interventionService,
            DeleteAmendmentCommand command,
            Exception e,
            DeleteAmendmentCommandHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.DeleteIntervention(command))
                .ThrowsAsync(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnInt_GivenSuccessfulDelete(
            [Frozen] Mock<IInterventionService> interventionService,
            DeleteAmendmentCommand command,
            DeleteAmendmentCommandHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.DeleteIntervention(command)).ReturnsAsync(1);
            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(1, result);
            Assert.IsType<int>(result);
        }
    }
}
