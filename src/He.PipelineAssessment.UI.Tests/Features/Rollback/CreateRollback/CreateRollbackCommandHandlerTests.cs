using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using Moq;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.CreateRollback
{
    public class CreateRollbackCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
            CreateRollbackCommand command,
            CreateRollbackCommandHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Assessment Tool Workflow Instance with Id {command.WorkflowInstanceId} not found", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentNotAuthorised(
            [Frozen]Mock<IAssessmentRepository> repository,
            [Frozen]Mock<IRoleValidation> roleValidation,
            CreateRollbackCommand command,
            AssessmentToolWorkflowInstance instance,
            CreateRollbackCommandHandler sut)
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
            CreateRollbackCommand command,
            AssessmentToolWorkflowInstance instance,
            CreateRollbackCommandHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId))
                .ReturnsAsync(instance);

            roleValidation.Setup(x => x.ValidateRole(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(false);

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to create  for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Should_GivenAssessmentToolWorkflowInstanceIsNotLatestSubmitted(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            CreateRollbackCommand command,
            AssessmentToolWorkflowInstance instance,
            CreateRollbackCommandHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId))
                .ReturnsAsync(instance);

            roleValidation.Setup(x => x.ValidateRole(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(true);

            //Act
            var result = sut.Handle(command, CancellationToken.None);

            //Assert
            repository.Verify(x=>x.CreateAssessmentIntervention(It.IsAny<AssessmentIntervention>()),Times.Once);
            Assert.NotNull(result);
        }
    }
}
