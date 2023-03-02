using AutoFixture.Xunit2;
using Elsa.Server.Providers;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;
using VersionOptions = Elsa.Models.VersionOptions;
using WorkflowInstance = Elsa.Models.WorkflowInstance;

namespace Elsa.Server.Tests.Providers
{
    public class WorkflowRegistryProviderTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetNextActivity_ThrowsException_WhenOutputIsNull(
            WorkflowInstance workflowInstance,
            CancellationToken cancellationToken,
            WorkflowRegistryProvider sut)
        {
            //Arrange
            workflowInstance.Output = null;

            //Act
            var exception = await Assert.ThrowsAsync<Exception>(() => sut.GetNextActivity(workflowInstance, cancellationToken));

            //Assert
            Assert.Equal($"No output found for workflow instance id {workflowInstance.Id}", exception.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNextActivity_ThrowsException_WhenNextActivityIsNull(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistryMock,
            WorkflowInstance workflowInstance,
            CancellationToken cancellationToken,
            WorkflowBlueprint workflowBlueprint,
            WorkflowRegistryProvider sut)
        {
            //Arrange
            workflowRegistryMock.Setup(x => x.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, null, cancellationToken)).ReturnsAsync(workflowBlueprint);

            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = "",
                Name = "test"
            });

            //Act
            var exception = await Assert.ThrowsAsync<Exception>(() => sut.GetNextActivity(workflowInstance, cancellationToken));

            //Assert
            Assert.Equal($"Next activity not found for workflow instance id {workflowInstance.Id}.", exception.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetNextActivity_ReturnsNextActivity_WhenNextActivityIsFound(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistryMock,
            WorkflowInstance workflowInstance,
            CancellationToken cancellationToken,
            WorkflowBlueprint workflowBlueprint,
            WorkflowRegistryProvider sut)
        {
            //Arrange
            workflowRegistryMock.Setup(x => x.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, null, cancellationToken)).ReturnsAsync(workflowBlueprint);

            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Id = workflowInstance.Output!.ActivityId,
                Name = "test"
            });

            //Act
            var result = await sut.GetNextActivity(workflowInstance, cancellationToken);

            //Assert
            Assert.NotNull(result);
        }
    }
}
