using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Amendment.EditAmendment;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment.EditAmendment
{
    public class EditAmendmentRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldIgnoreError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
                        [Frozen] Mock<IInterventionService> interventionService,
            EditAmendmentRequest request,
            Exception e,
            EditAmendmentRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.EditInterventionRequest(request))
                .ThrowsAsync(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnInt_GivenSuccessfulEdit(
            [Frozen] Mock<IInterventionService> interventionService,
            EditAmendmentRequest request,
            AssessmentInterventionDto dto,
            EditAmendmentRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.EditInterventionRequest(request)).ReturnsAsync(dto);
            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(dto, result);
            Assert.IsType<AssessmentInterventionDto>(result);
        }
    }
}
