using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Commands
{
    public class UpdateAssessmentToolWorkflowCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowException_GivenDependencyThrows
        (
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            UpdateAssessmentToolWorkflowCommand updateAssessmentToolWorkflowCommand,
            Exception exception,
            UpdateAssessmentToolWorkflowCommandHandler sut
        )
        {
            //Arrange
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowById(updateAssessmentToolWorkflowCommand.Id))
                .Throws(exception);

            //Act
            var result =
                await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(updateAssessmentToolWorkflowCommand, CancellationToken.None));

            //Assert          
            Assert.Equal($"Unable to update assessment tool workflow. AssessmentToolWoirkflowId: {updateAssessmentToolWorkflowCommand.Id}", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowException_GivenAssessmentToolWorkflowNotFound
        (
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            UpdateAssessmentToolWorkflowCommand updateAssessmentToolWorkflowCommand,
            UpdateAssessmentToolWorkflowCommandHandler sut
        )
        {
            //Arrange
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowById(updateAssessmentToolWorkflowCommand.Id))
                .ReturnsAsync((AssessmentToolWorkflow?)null);

            //Act
            var result =
                await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(updateAssessmentToolWorkflowCommand, CancellationToken.None));

            //Assert          
            //Assert          
            Assert.Equal($"Unable to update assessment tool workflow. AssessmentToolWoirkflowId: {updateAssessmentToolWorkflowCommand.Id}", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_CallsRepositoryWithCorrectValues_GivenNoErrors
        (
            [Frozen] Mock<IAdminAssessmentToolWorkflowRepository> adminAssessmentToolWorkflowRepository,
            UpdateAssessmentToolWorkflowCommand updateAssessmentToolWorkflowCommand,
            AssessmentToolWorkflow assessmentToolWorkflow,
            UpdateAssessmentToolWorkflowCommandHandler sut
        )
        {
            //Arrange
            adminAssessmentToolWorkflowRepository.Setup(x => x.GetAssessmentToolWorkflowById(updateAssessmentToolWorkflowCommand.Id))
                .ReturnsAsync(assessmentToolWorkflow);
            adminAssessmentToolWorkflowRepository.Setup(x => x.UpdateAssessmentToolWorkflow(assessmentToolWorkflow))
                .ReturnsAsync(2);

            //Act
            var result = await sut.Handle(updateAssessmentToolWorkflowCommand, CancellationToken.None);

            //Assert          
            adminAssessmentToolWorkflowRepository.Verify(
                x => x.UpdateAssessmentToolWorkflow(It.Is<AssessmentToolWorkflow>(y =>
                    y.WorkflowDefinitionId == updateAssessmentToolWorkflowCommand.WorkflowDefinitionId &&
                    y.IsFirstWorkflow == updateAssessmentToolWorkflowCommand.IsFirstWorkflow &&
                    y.Name == updateAssessmentToolWorkflowCommand.Name && 
                    y.IsEconomistWorkflow == updateAssessmentToolWorkflowCommand.IsEconomistWorkflow &&
                    y.IsVariation == updateAssessmentToolWorkflowCommand.IsVariation
                    )), Times.Once);
            Assert.Equal(2, result);
        }
    }
}
