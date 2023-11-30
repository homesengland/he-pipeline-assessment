using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Amendment.LoadAmendmentCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment;
using He.PipelineAssessment.UI.Features.Intervention;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Amendment.LoadAmendmentCheckYourAnswers
{
    public class LoadAmendmentCheckYourAnswersRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenInterventionNotFound(
                   LoadAmendmentCheckYourAnswersRequest request,
                   LoadAmendmentCheckYourAnswersRequestHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to load amendment check your answers. InterventionId: {request.InterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturn(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            AssessmentInterventionCommand command,
            AssessmentIntervention intervention,
            LoadAmendmentCheckYourAnswersRequest request,
            LoadAmendmentCheckYourAnswersRequestHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(request.InterventionId))
                .ReturnsAsync(intervention);

            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention))
                .Returns(command);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SubmitAmendmentCommand>(result);
        }
    }
}
