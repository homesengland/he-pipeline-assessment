using He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor;
using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.EditRollbackAssessor
{
    public class EditRollbackAssessorRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
            EditRollbackAssessorRequest request,
            EditRollbackAssessorRequestHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to edit rollback. InterventionId: {request.InterventionId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturn(
            [Frozen]Mock<IAssessmentInterventionMapper> mapper,
            [Frozen]Mock<IAssessmentRepository> repository,
            AssessmentIntervention intervention,
            EditRollbackAssessorRequest request,
            AssessmentInterventionCommand command,
            EditRollbackAssessorRequestHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(command,result.AssessmentInterventionCommand);
     
        }
    }
}
