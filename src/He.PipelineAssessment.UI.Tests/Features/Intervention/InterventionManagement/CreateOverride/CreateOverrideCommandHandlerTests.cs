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

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenRepoThrowsError(
                  [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                  [Frozen] Mock<ICreateOverrideMapper> mapper,
                  AssessmentIntervention intervention,
                  CreateOverrideCommand command,
                  Exception exception,
                  CreateOverrideCommandHandler sut
              )
        {
            //Arrange
            mapper.Setup(x => x.CreateOverrideCommandToAssessmentIntervention(command)).Returns(intervention);

            assessmentRepository.Setup(x => x.CreateAssessmentIntervention(intervention)).Throws(exception);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(-1, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsValidId_GivenNoRepositoryErrors(
                  [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                  [Frozen] Mock<ICreateOverrideMapper> mapper,
                  AssessmentIntervention intervention,
                  CreateOverrideCommand command,
                  Exception exception,
                  CreateOverrideCommandHandler sut
        )
        {

            //Arrange
            mapper.Setup(x => x.CreateOverrideCommandToAssessmentIntervention(command)).Returns(intervention);

            assessmentRepository.Setup(x => x.CreateAssessmentIntervention(intervention)).Returns(Task.FromResult(1));

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(intervention.Id, result);

        }
    }
}
