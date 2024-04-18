using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using AutoFixture.Xunit2;
using Moq;
using Xunit;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.CreateRollback
{
   
    public class CreateRollbackRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionServiceErrors(
            [Frozen] Mock<IInterventionService> interventionService,
            Exception e,
            CreateRollbackRequest request,
            CreateRollbackRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.CreateInterventionRequest(request)).Throws(e);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(e.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnDto_GivenSuccessfulCallToInterventionService(
            [Frozen] Mock<IInterventionService> interventionService,
            AssessmentInterventionDto dto,
            CreateRollbackRequest request,
            CreateRollbackRequestHandler sut)
        {
            //Arrange
            interventionService.Setup(x => x.CreateInterventionRequest(request)).ReturnsAsync(dto);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result, dto);
        }
    }
}
