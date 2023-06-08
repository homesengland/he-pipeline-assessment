using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Override.CreateOverride;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_Throws_GivenMapperThrowsException(
            [Frozen] Mock<ICreateOverrideMapper> mapper,
            CreateOverrideCommand command,
            Exception exception,
            CreateOverrideCommandHandler sut
        )
        {
            //Arrange
            mapper.Setup(x => x.CreateOverrideCommandToAssessmentIntervention(command)).Throws(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(exception, ex);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Throws_GivenRepoThrowsException(
            [Frozen] Mock<IAssessmentRepository> repo,
            [Frozen] Mock<ICreateOverrideMapper> mapper,
            CreateOverrideCommand command,
            Exception exception,
            AssessmentIntervention intervention,
            CreateOverrideCommandHandler sut
        )
        {
            //Arrange
            mapper.Setup(x => x.CreateOverrideCommandToAssessmentIntervention(command)).Returns(intervention);
            repo.Setup(x => x.CreateAssessmentIntervention(intervention)).Throws(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(exception, ex);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsInterventionId_GivenSuccessfulRepoCall(
            [Frozen] Mock<IAssessmentRepository> repo,
            [Frozen] Mock<ICreateOverrideMapper> mapper,
            CreateOverrideCommand command,
            AssessmentIntervention intervention,
            CreateOverrideCommandHandler sut
        )
        {
            //Arrange
            mapper.Setup(x => x.CreateOverrideCommandToAssessmentIntervention(command)).Returns(intervention);
            repo.Setup(x => x.CreateAssessmentIntervention(intervention)).ReturnsAsync(1);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(intervention.Id, result);
        }
    }
}
