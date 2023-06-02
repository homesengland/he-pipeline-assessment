using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using Moq;
using Xunit;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditOverride;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.EditOverride
{
    public class EditOverrideCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenRepoThrowsError(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          AssessmentIntervention intervention,
          EditOverrideCommand command,
          Exception exception,
          EditOverrideCommandHandler sut
              )
        {
            //Arrange

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).Throws(exception);

            //Act

            //Assert
            Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsInvalidId_GivenNullResponseFromRepo(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          AssessmentIntervention intervention,
          EditOverrideCommand command,
          EditOverrideCommandHandler sut
)
        {

            //Arrange
            intervention = null;
            int expectedId = -1;
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(expectedId, result);

        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsValidId_GivenNoRepositoryErrors(
                  [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                  AssessmentIntervention intervention,
                  EditOverrideCommand command,
                  EditOverrideCommandHandler sut
        )
        {

            //Arrange

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(intervention.Id, result);

        }
    }
}
