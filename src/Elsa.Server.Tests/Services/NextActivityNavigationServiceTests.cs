using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Helpers;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Services
{
    public class NextActivityNavigationServiceTests
    {
        [Theory]
        [AutoMoqData]
        public async Task CreateNextActivityNavigation_ShouldCreateCustomActivityNavigation_GivenNextActivityRecordIsNull(
            [Frozen] Mock<IElsaCustomModelHelper> elsaCustomModelHelper,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string previousActivityId,
            IActivityBlueprint nextActivity,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            NextActivityNavigationService sut)
        {
            //Arrange
            elsaCustomModelHelper.Setup(x => x.CreateNextCustomActivityNavigation(previousActivityId,
                    ActivityTypeConstants.QuestionScreen, nextActivity.Id, nextActivity.Type, workflowInstance))
                .Returns(customActivityNavigation);

            //Act
            await sut.CreateNextActivityNavigation(previousActivityId, null, nextActivity,
                workflowInstance, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.CreateCustomActivityNavigationAsync(customActivityNavigation, CancellationToken.None), Times.Once);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateNextActivityNavigation_ShouldCreateQuestionScreenAnswers_GivenQuestionScreenActivityType(
            [Frozen] Mock<IElsaCustomModelHelper> elsaCustomModelHelper,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string previousActivityId,
            IActivityBlueprint nextActivity,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            List<QuestionScreenAnswer> questionScreenAnswers,
            NextActivityNavigationService sut)
        {
            //Arrange
            customActivityNavigation.ActivityType = ActivityTypeConstants.QuestionScreen;
            elsaCustomModelHelper.Setup(x => x.CreateNextCustomActivityNavigation(previousActivityId,
                    ActivityTypeConstants.QuestionScreen, nextActivity.Id, nextActivity.Type, workflowInstance))
                .Returns(customActivityNavigation);

            elsaCustomModelHelper.Setup(x => x.CreateQuestionScreenAnswers(nextActivity.Id, workflowInstance))
                .Returns(questionScreenAnswers);

            //Act
            await sut.CreateNextActivityNavigation(previousActivityId, null, nextActivity,
                workflowInstance, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.CreateQuestionScreenAnswersAsync(questionScreenAnswers, CancellationToken.None), Times.Once);
        }

        [Theory]
        [InlineAutoMoqData(ActivityTypeConstants.CheckYourAnswersScreen)]
        [InlineAutoMoqData(ActivityTypeConstants.ConfirmationScreen)]
        [InlineAutoMoqData(ActivityTypeConstants.SinglePipelineDataSource)]
        public async Task CreateNextActivityNavigation_ShouldNotCreateQuestionScreenAnswers_GivenOtherActivityTypes(
            string activityType,
            [Frozen] Mock<IElsaCustomModelHelper> elsaCustomModelHelper,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            string previousActivityId,
            IActivityBlueprint nextActivity,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            NextActivityNavigationService sut)
        {
            //Arrange
            customActivityNavigation.ActivityType = activityType;
            elsaCustomModelHelper.Setup(x => x.CreateNextCustomActivityNavigation(previousActivityId,
                    activityType, nextActivity.Id, nextActivity.Type, workflowInstance))
                .Returns(customActivityNavigation);

            //Act
            await sut.CreateNextActivityNavigation(previousActivityId, null, nextActivity,
                workflowInstance, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(
                x => x.CreateQuestionScreenAnswersAsync(It.IsAny<List<QuestionScreenAnswer>>(), CancellationToken.None), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task CreateNextActivityNavigation_ShouldUpdateCustomActivityNavigation_GivenNextActivityRecordExists(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
            string previousActivityId,
            CustomActivityNavigation nextActivityRecord,
            IActivityBlueprint nextActivity,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation customActivityNavigation,
            DateTime date,
            NextActivityNavigationService sut)
        {
            //Arrange
            dateTimeProvider.Setup(x => x.UtcNow()).Returns(date);

            //Act
            await sut.CreateNextActivityNavigation(previousActivityId, nextActivityRecord, nextActivity,
                workflowInstance, CancellationToken.None);

            //Assert
            Assert.Equal(date, nextActivityRecord.LastModifiedDateTime);
            elsaCustomRepository.Verify(
                x => x.CreateQuestionScreenAnswersAsync(It.IsAny<List<QuestionScreenAnswer>>(), CancellationToken.None), Times.Never);
            elsaCustomRepository.Verify(
                x => x.CreateCustomActivityNavigationAsync(customActivityNavigation, CancellationToken.None), Times.Never);
            elsaCustomRepository.Verify(
                x => x.UpdateCustomActivityNavigation(nextActivityRecord, CancellationToken.None), Times.Once);
        }
    }
}
