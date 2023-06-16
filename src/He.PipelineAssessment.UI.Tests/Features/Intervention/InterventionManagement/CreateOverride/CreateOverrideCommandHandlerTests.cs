using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Override.CreateOverride;
using Moq;
using System;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_Throws_GivenMapperThrowsException(
            [Frozen] Mock<ICreateOverrideMapper> mapper,
            [Frozen] Mock<IAssessmentRepository> repo,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowHelper,
            CreateOverrideCommand command,
            AssessmentToolWorkflowInstance instance,
            Exception exception,
            CreateOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(instance);

            assessmentToolWorkflowHelper
                .Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(true);

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
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowHelper,
            CreateOverrideCommand command,
            Exception exception,
            AssessmentIntervention intervention,
            AssessmentToolWorkflowInstance instance,
            CreateOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(instance);

            assessmentToolWorkflowHelper
                .Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(true);

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
            [Frozen]Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowHelper,
            CreateOverrideCommand command,
            AssessmentIntervention intervention,
            AssessmentToolWorkflowInstance instance,
            CreateOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(instance);

            assessmentToolWorkflowHelper
                .Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(true);

            mapper.Setup(x => x.CreateOverrideCommandToAssessmentIntervention(command)).Returns(intervention);

            repo.Setup(x => x.CreateAssessmentIntervention(intervention)).ReturnsAsync(1);

           

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(intervention.Id, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenWorkflowInstanceNotFound(
            [Frozen] Mock<IAssessmentRepository> repo,
            [Frozen] Mock<ICreateOverrideMapper> mapper,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowHelper,
            CreateOverrideCommand command,
            AssessmentToolWorkflowInstance instance,
            CreateOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync((AssessmentToolWorkflowInstance?)null);

            var exception = new NotFoundException($"Assessment Tool Workflow Instance with Id {command.WorkflowInstanceId} not found");
            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenIsNotLatestSubmittedWorkflow(
            [Frozen] Mock<IAssessmentRepository> repo,
            [Frozen] Mock<ICreateOverrideMapper> mapper,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowHelper,
            CreateOverrideCommand command,
            AssessmentIntervention intervention,
            AssessmentToolWorkflowInstance instance,
            CreateOverrideCommandHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetAssessmentToolWorkflowInstance(command.WorkflowInstanceId)).ReturnsAsync(instance);

            assessmentToolWorkflowHelper
                .Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(false);

            mapper.Setup(x => x.CreateOverrideCommandToAssessmentIntervention(command)).Returns(intervention);

            repo.Setup(x => x.CreateAssessmentIntervention(intervention)).ReturnsAsync(1);

            var exception =  new Exception(
                $"Unable to create override for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment.");
            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }
    }
}
