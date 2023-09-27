using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Features.Workflow.LoadConfirmationScreen;
using Elsa.Server.Providers;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenCustomActivityNavigationDoesNotExist(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadConfirmationScreenRequest request,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(request.ActivityId,
                    request.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync((CustomActivityNavigation?)null);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.CheckQuestions);
            Assert.Equal(
                $"Unable to find activity navigation with Workflow Id: {request.WorkflowInstanceId} and Activity Id: {request.ActivityId} in Elsa Custom database",
                result.ErrorMessages.Single());
            elsaCustomRepository.Verify(
                x => x.GetWorkflowInstanceQuestions(It.IsAny<string>(), CancellationToken.None), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenExceptionIsThrown(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            LoadConfirmationScreenRequest request,
            Exception exception,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(request.ActivityId,
                    request.WorkflowInstanceId, CancellationToken.None))
                .ThrowsAsync(exception);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Null(result.Data!.CheckQuestions);
            Assert.Equal(exception.Message, result.ErrorMessages.Single());
            elsaCustomRepository.Verify(
                x => x.GetWorkflowInstanceQuestions(It.IsAny<string>(), CancellationToken.None), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnQuestions_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            LoadConfirmationScreenRequest request,
            CustomActivityNavigation customActivityNavigation,
            List<Question> questions,
            GroupedTextModel groupedTextModel,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(request.ActivityId,
                    request.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(customActivityNavigation);

            elsaCustomRepository.Setup(x => x.GetWorkflowInstanceQuestions(request.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(questions);

            var dictionary = new Dictionary<string, object?>()
            {
                { "ConfirmationTitle", "MyConfirmationTitle" },
                { "ConfirmationText", "MyConfirmationText" },
                { "FooterText", "MyFooterText" },
                { "FooterTitle", "MyFooterTitle" },
                { "NextWorkflowDefinitionIds", "MyNextWorkflowDefinitionId" },
                { "Text", groupedTextModel}
            };

            activityDataProvider
                .Setup(x => x.GetActivityData(request.WorkflowInstanceId, request.ActivityId, CancellationToken.None))
                .ReturnsAsync(dictionary);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(questions, result.Data!.CheckQuestions);
            Assert.Equal(request.ActivityId, result.Data.ActivityId);
            Assert.Equal(request.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Equal(ActivityTypeConstants.ConfirmationScreen, result.Data.ActivityType);
            Assert.Equal("MyConfirmationTitle", result.Data.ConfirmationTitle);
            Assert.Equal("MyConfirmationText", result.Data.ConfirmationText);
            Assert.Equal("MyFooterText", result.Data.FooterText);
            Assert.Equal("MyFooterTitle", result.Data.FooterTitle);
            //Assert.Equal(3, result.Data.Text.Count());
            //Assert.Equal("1", result.Data.Text.FirstOrDefault());
            Assert.Equal("MyNextWorkflowDefinitionId", result.Data.NextWorkflowDefinitionIds);
            Assert.Equal(groupedTextModel.TextGroups.Count, result.Data.Text.Count);
            Assert.Empty(result.ErrorMessages);
        }

        [Theory]
        [AutoMoqData]

        public async Task InformationListFromTextGroups_MapsCorrectly(
            GroupedTextModel groupedTextModel,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange

            //Act
            var result = sut.InformationListFromTextGroups(groupedTextModel);

            //Assert
            Assert.Equal(groupedTextModel.TextGroups.Count, result.Count);
            for (var i = 0; i < result.Count; i++)
            {
                var expected = groupedTextModel.TextGroups[i];
                var actual = result[i];
                Assert.Equal(expected.Bullets, actual.IsBullets);
                Assert.Equal(expected.Collapsed, actual.IsCollapsed);
                Assert.Equal(expected.Guidance, actual.IsGuidance);
                Assert.Equal(expected.Title, actual.Title);
                for (var j = 0; j < expected.TextRecords.Count; j++)
                {
                    var expectedTextRecord = expected.TextRecords[j];
                    var actualInformationText = actual.InformationTextList[j];

                    Assert.Equal(expectedTextRecord.IsBold, actualInformationText.IsBold);
                    Assert.Equal(expectedTextRecord.IsHyperlink, actualInformationText.IsHyperlink);
                    Assert.Equal(expectedTextRecord.IsParagraph, actualInformationText.IsParagraph);
                    Assert.Equal(expectedTextRecord.Url, actualInformationText.Url);
                    Assert.Equal(expectedTextRecord.Text, actualInformationText.Text);
                }
            }
        }
    }
}
