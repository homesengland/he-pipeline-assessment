using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Services
{
    public class DeleteChangedWorkflowPathServiceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task DeleteChangedWorkflowPath_ShouldDeleteCustomNavigationAndQuestions_GivenChangedPathNavigationIsNotNull(
            [Frozen] Mock<IWorkflowPathProvider> workflowPathProvider,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            string activityId,
            IActivityBlueprint nextActivity,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<string> previousPathActivities,
            DeleteChangedWorkflowPathService sut)
        {
            //Arrange
            workflowPathProvider
                .Setup(x => x.GetChangedPathCustomNavigation(workflowInstanceId,
                    activityId, nextActivity.Id, CancellationToken.None))
                .ReturnsAsync(customActivityNavigation);

            workflowPathProvider
                .Setup(x => x.GetPreviousPathActivities(workflowInstance.DefinitionId,
                    customActivityNavigation.ActivityId, CancellationToken.None))
                .ReturnsAsync(previousPathActivities);

            //Act
            await sut.DeleteChangedWorkflowPath(workflowInstanceId, activityId, nextActivity, workflowInstance,
                CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.DeleteCustomNavigations(previousPathActivities, workflowInstanceId, CancellationToken.None), Times.Once);
            elsaCustomRepository.Verify(
                x => x.DeleteQuestionScreenAnswers(customActivityNavigation.WorkflowInstanceId, previousPathActivities, CancellationToken.None), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task DeleteChangedWorkflowPath_ShouldNotDeleteCustomNavigationAndQuestions_GivenChangedPathNavigationIsNull(
            [Frozen] Mock<IWorkflowPathProvider> workflowPathProvider,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string workflowInstanceId,
            string activityId,
            IActivityBlueprint nextActivity,
            WorkflowInstance workflowInstance,
            DeleteChangedWorkflowPathService sut)
        {
            //Arrange
            workflowPathProvider
                .Setup(x => x.GetChangedPathCustomNavigation(workflowInstanceId,
                    activityId, nextActivity.Id, CancellationToken.None))
                .ReturnsAsync((CustomActivityNavigation?)null);

            //Act
            await sut.DeleteChangedWorkflowPath(workflowInstanceId, activityId, nextActivity, workflowInstance,
                CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.DeleteCustomNavigations(It.IsAny<List<string>>(), workflowInstanceId, CancellationToken.None), Times.Never);
            elsaCustomRepository.Verify(
                x => x.DeleteQuestionScreenAnswers(workflowInstanceId,
                    It.IsAny<List<string>>(), CancellationToken.None), Times.Never);
        }
    }
}
