using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Server.Providers;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Providers
{
    public class WorkflowPathProviderTests
    {
        [Theory]
        [AutoMoqData]
        public async Task GetChangedPathCustomNavigation_ShouldReturnCustomNavigationObjectFromRepository(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            string currentActivityId,
            string nextActivityId,
            CustomActivityNavigation customActivityNavigation,
            WorkflowPathProvider sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetChangedPathNavigation(workflowInstanceId,
                    currentActivityId, nextActivityId, CancellationToken.None))
                .ReturnsAsync(customActivityNavigation);

            //Act
            var result = await sut.GetChangedPathCustomNavigation(workflowInstanceId, currentActivityId, nextActivityId,
                CancellationToken.None);

            //Assert
            Assert.Equal(customActivityNavigation, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPreviousPathActivities_ShouldReturnOnlyChangedPathActivityId_GivenNoActivityConnectionsAvailable(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            IWorkflowBlueprint workflowBlueprint,
            string definitionId,
            string changedPathActivityId,
            WorkflowPathProvider sut
        )
        {
            //Arrange
            workflowRegistry
                .Setup(x => x.FindAsync(definitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            //Act
            var result =
                await sut.GetPreviousPathActivities(definitionId, changedPathActivityId, CancellationToken.None);

            //Assert
            Assert.Equal(changedPathActivityId, result.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPreviousPathActivities_ShouldReturnTwoPreviousPathActivities_GivenOneChildConnection(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            WorkflowBlueprint workflowBlueprint,
            ActivityBlueprint parentActivityBlueprint,
            ActivityBlueprint childActivityBlueprint,
            string definitionId,
            string changedPathActivityId,
            WorkflowPathProvider sut
        )
        {
            //Arrange
            parentActivityBlueprint.Id = changedPathActivityId;
            var parentConnection = new Connection(new SourceEndpoint(parentActivityBlueprint, "Test"),
                new TargetEndpoint(childActivityBlueprint));
            workflowBlueprint.Connections.Add(parentConnection);
            workflowRegistry
                .Setup(x => x.FindAsync(definitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            //Act
            var result =
                await sut.GetPreviousPathActivities(definitionId, changedPathActivityId, CancellationToken.None);

            //Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(changedPathActivityId, result);
            Assert.Contains(childActivityBlueprint.Id, result);
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPreviousPathActivities_ShouldReturnItselfAndAllPreviousPathChildActivities_GivenAChainOfChildConnections(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            WorkflowBlueprint workflowBlueprint,
            string definitionId,
            string changedPathActivityId,
            WorkflowPathProvider sut
        )
        {
            //Arrange
            workflowBlueprint.Connections = new List<IConnection>();
            var parentActivityBlueprintId = changedPathActivityId;
            var listToAssert = new List<string>() { parentActivityBlueprintId };
            for (int i = 0; i < 10; i++)
            {
                var parentActivityBlueprint = new ActivityBlueprint()
                {
                    Id = parentActivityBlueprintId
                };
                var childActivityBlueprint = new ActivityBlueprint()
                {
                    Id = Guid.NewGuid().ToString()
                };
                var parentConnection = new Connection(new SourceEndpoint(parentActivityBlueprint, "Test"),
                    new TargetEndpoint(childActivityBlueprint));
                workflowBlueprint.Connections.Add(parentConnection);

                listToAssert.Add(childActivityBlueprint.Id);
                parentActivityBlueprintId = childActivityBlueprint.Id;
            }

            workflowRegistry
                .Setup(x => x.FindAsync(definitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            //Act
            var result =
                await sut.GetPreviousPathActivities(definitionId, changedPathActivityId, CancellationToken.None);

            //Assert
            Assert.Equal(11, result.Count);
            foreach (var id in listToAssert)
            {
                Assert.Contains(id, result);
            }
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPreviousPathActivities_ShouldReturnDistinctPreviousPathChildActivities_GivenConnectionsWithDuplicates(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            WorkflowBlueprint workflowBlueprint,
            string definitionId,
            string changedPathActivityId,
            WorkflowPathProvider sut
        )
        {
            //Arrange
            workflowBlueprint.Connections = new List<IConnection>();
            var parentActivityBlueprintId = changedPathActivityId;
            var listToAssert = new List<string>() { parentActivityBlueprintId };
            for (int i = 0; i < 10; i++)
            {
                var parentActivityBlueprint = new ActivityBlueprint()
                {
                    Id = parentActivityBlueprintId
                };
                var childActivityBlueprint = new ActivityBlueprint()
                {
                    Id = Guid.NewGuid().ToString()
                };
                var parentConnection = new Connection(new SourceEndpoint(parentActivityBlueprint, "Test"),
                    new TargetEndpoint(childActivityBlueprint));
                workflowBlueprint.Connections.Add(parentConnection);
                workflowBlueprint.Connections.Add(parentConnection);

                listToAssert.Add(childActivityBlueprint.Id);
                parentActivityBlueprintId = childActivityBlueprint.Id;
            }

            workflowRegistry
                .Setup(x => x.FindAsync(definitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            //Act
            var result =
                await sut.GetPreviousPathActivities(definitionId, changedPathActivityId, CancellationToken.None);

            //Assert
            Assert.Equal(11, result.Count);
            foreach (var id in listToAssert)
            {
                Assert.Contains(id, result);
            }
        }

        [Theory]
        [AutoMoqData]
        public async Task GetPreviousPathActivities_ShouldReturnDistinctActivities_GivenConnectionsLinkingToThemselves(
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            WorkflowBlueprint workflowBlueprint,
            string definitionId,
            string changedPathActivityId,
            WorkflowPathProvider sut
        )
        {
            //Arrange
            workflowBlueprint.Connections = new List<IConnection>();
            var parentActivityBlueprintId = changedPathActivityId;
            for (int i = 0; i < 5; i++)
            {
                var parentActivityBlueprint = new ActivityBlueprint()
                {
                    Id = parentActivityBlueprintId
                };

                var parentConnection = new Connection(new SourceEndpoint(parentActivityBlueprint, "Test"),
                    new TargetEndpoint(parentActivityBlueprint));
                workflowBlueprint.Connections.Add(parentConnection);
            }

            workflowRegistry
                .Setup(x => x.FindAsync(definitionId, VersionOptions.Published, null, CancellationToken.None))
                .ReturnsAsync(workflowBlueprint);

            //Act
            var result =
                await sut.GetPreviousPathActivities(definitionId, changedPathActivityId, CancellationToken.None);

            //Assert
            Assert.Equal(changedPathActivityId, result.Single());
        }
    }
}
