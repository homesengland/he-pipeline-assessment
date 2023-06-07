using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentToolWorkflow;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Commands
{
    public class DeleteAssessmentToolWorkflowCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowException_GivenDependencyThrows
        (
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
          DeleteAssessmentToolWorkflowCommand deleteAssessmentToolWorkflowCommand,
          Exception exception,
          DeleteAssessmentToolWorkflowCommandHandler sut
        )
        {
            //Arrange
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowById(deleteAssessmentToolWorkflowCommand.Id))
                .Throws(exception);

            //Act
            var result = await Assert.ThrowsAsync<Exception>(() => sut.Handle(deleteAssessmentToolWorkflowCommand, CancellationToken.None));

            //Assert          
            Assert.Equal(exception.Message, result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_SetsAssessmentToolWorkflowStatusToDeleted
        (
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
          DeleteAssessmentToolWorkflowCommand deleteAssessmentToolWorkflowCommand,
          AssessmentToolWorkflow assessmentToolWorkflow,
          DeleteAssessmentToolWorkflowCommandHandler sut
        )
        {
            //Arrange
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowById(deleteAssessmentToolWorkflowCommand.Id))
               .ReturnsAsync(assessmentToolWorkflow);
            adminAssessmentToolWorkflowRepository.Setup(x => x.SaveChanges())
               .ReturnsAsync(34);

            //Act
            var result = await sut.Handle(deleteAssessmentToolWorkflowCommand, CancellationToken.None);

            //Assert          
            Assert.Equal(AssessmentToolStatus.Deleted, assessmentToolWorkflow.Status);
            adminAssessmentToolWorkflowRepository.Verify(x => x.SaveChanges(), Times.Once);
            Assert.Equal(34, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowException_GivenAssessmentToolNotFound
        (
          [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
           DeleteAssessmentToolWorkflowCommand deleteAssessmentToolWorkflowCommand,
            DeleteAssessmentToolWorkflowCommandHandler sut
        )
        {
            //Arrange
            string exceptionMessage = $"Value cannot be null. (Parameter 'Assessment Tool Workflow not found')";

            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowById(deleteAssessmentToolWorkflowCommand.Id))
                .ReturnsAsync((AssessmentToolWorkflow?)null);

            //Act
            var result = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(deleteAssessmentToolWorkflowCommand, CancellationToken.None));

            //Assert          
            Assert.Equal(exceptionMessage, result.Message);
        }
    }
}
