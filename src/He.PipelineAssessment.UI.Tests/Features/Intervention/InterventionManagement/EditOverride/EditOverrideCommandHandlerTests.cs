using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Override.EditOverride;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.EditOverride
{
    public class EditOverrideCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_Throws_GivenGetAssessmentInterventionThrowsException(
            [Frozen] Mock<IAssessmentRepository> repo,
            EditOverrideCommand command,
            Exception exception,
            EditOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).Throws(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(exception, ex);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Throws_GivenAssessmentInterventionNotFound(
            [Frozen] Mock<IAssessmentRepository> repo,
            EditOverrideCommand command,
            EditOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync((AssessmentIntervention?)null);

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Assessment Intervention with Id {command.AssessmentInterventionId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Throws_GivenSaveChangesThrows(
            [Frozen] Mock<IAssessmentRepository> repo,
            EditOverrideCommand command,
            Exception exception,
            AssessmentIntervention intervention,
            EditOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);
            repo.Setup(x => x.SaveChanges()).Throws(exception);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(exception, ex);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsInterventionId_GivenSuccessfulUpdate(
            [Frozen] Mock<IAssessmentRepository> repo,
            EditOverrideCommand command,
            AssessmentIntervention intervention,
            EditOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId)).ReturnsAsync(intervention);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(intervention.Id, result);
            Assert.Equal(command.SignOffDocument, intervention.SignOffDocument);
            Assert.Equal(command.AdministratorRationale, intervention.AdministratorRationale);
            Assert.Equal(command.TargetWorkflowId, intervention.TargetAssessmentToolWorkflowId);
        }
    }
}
