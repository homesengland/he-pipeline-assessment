using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using He.PipelineAssessment.UI.Features.Amendment.CreateAmendment;
using He.PipelineAssessment.UI.Services;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment.CreateAmendment
{
    public class CreateAmendmentRequestMapperTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldIgnoreError_GivenInterventionServiceThrowsException(
            [Frozen] Mock<IInterventionService> interventionService,
            CreateAmendmentRequest request,
            CreateAmendmentRequestHandler sut,
            Exception e)
        {
            //Arrange
            interventionService.Setup(x => x.CreateInterventionRequest(request)).ThrowsAsync(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnAssessmentInterventionDto(
            [Frozen] Mock<IInterventionService> interventionService,
            CreateAmendmentRequest request,
            AssessmentInterventionDto dto,
            CreateAmendmentRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.CreateInterventionRequest(request)).ReturnsAsync(dto);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AssessmentInterventionDto>(result);

        }
    }
}
