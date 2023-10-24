using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Ammendment.CreateAmmendment;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Ammendment.CreateAmmendment
{
    public class CreateAmmendmentCommandHandlerTests
    {
            [Theory]
            [AutoMoqData]
            public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
                CreateAmmendmentCommand command,
                CreateAmmendmentCommandHandler sut)
            {
                //Act
                var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(command, CancellationToken.None));

                //Assert
                Assert.Equal($"Unable to create ammendment. WorkflowInstanceId: {command.WorkflowInstanceId}", ex.Message);
            }

            [Theory]
            [AutoMoqData]
            public async Task Handle_ShouldError_GivenAssessmentNotAuthorised(
                [Frozen] Mock<IAssessmentRepository> repository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                CreateAmmendmentCommand command,
                AssessmentToolWorkflowInstance instance,
                CreateAmmendmentCommandHandler sut)
            {
                //Arrange
                repository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId))
                    .ReturnsAsync(instance);

                roleValidation.Setup(x => x.ValidateRole(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

                //Act
                var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(command, CancellationToken.None));

                //Assert
                Assert.Equal($"You do not have permission to access this resource.", ex.Message);
            }

            [Theory]
            [AutoMoqData]
            public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceIsNotLatestSubmitted(
                [Frozen] Mock<IAssessmentRepository> repository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
                CreateAmmendmentCommand command,
                AssessmentToolWorkflowInstance instance,
                CreateAmmendmentCommandHandler sut)
            {
                //Arrange
                repository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId))
                    .ReturnsAsync(instance);

                roleValidation.Setup(x => x.ValidateRole(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

                assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(false);

                //Act
                var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(command, CancellationToken.None));

                //Assert
                Assert.Equal($"Unable to create ammendment. WorkflowInstanceId: {command.WorkflowInstanceId}", ex.Message);
            }

            [Theory]
            [AutoMoqData]
            public async Task Handle_Should_GivenAssessmentToolWorkflowInstanceIsNotLatestSubmitted(
                [Frozen] Mock<IAssessmentRepository> repository,
                [Frozen] Mock<IRoleValidation> roleValidation,
                [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
                CreateAmmendmentCommand command,
                AssessmentToolWorkflowInstance instance,
                CreateAmmendmentCommandHandler sut)
            {
                //Arrange
                repository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId))
                    .ReturnsAsync(instance);

                roleValidation.Setup(x => x.ValidateRole(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

                assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(true);

                //Act
                var result = await sut.Handle(command, CancellationToken.None);

                //Assert
                repository.Verify(x => x.CreateAssessmentIntervention(It.IsAny<AssessmentIntervention>()), Times.Once);
            }
    }
}
