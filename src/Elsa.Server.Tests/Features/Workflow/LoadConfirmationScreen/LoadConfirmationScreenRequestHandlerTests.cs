using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Features.Workflow.LoadConfirmationScreen;
using Elsa.Server.Mappers;
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
        public async Task Handle_ReturnConfirmationWithGroupedTextModel_GivenEnhancedTextExistsAndNoErrorsEncountered(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            [Frozen] Mock<ITextGroupMapper> textGroupMapper,
            LoadConfirmationScreenRequest request,
            CustomActivityNavigation customActivityNavigation,
            List<Question> questions,
            GroupedTextModel groupedTextModel,
            List<Information> informationList,
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
                { "EnhancedText", groupedTextModel}
            };

            activityDataProvider
                .Setup(x => x.GetActivityData(request.WorkflowInstanceId, request.ActivityId, CancellationToken.None))
                .ReturnsAsync(dictionary);
            textGroupMapper.Setup(x => x.InformationListFromGroupedTextModel(groupedTextModel)).Returns(informationList);

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
            Assert.Equal("MyNextWorkflowDefinitionId", result.Data.NextWorkflowDefinitionIds);
            Assert.Equal(informationList, result.Data.Text);
            Assert.Empty(result.ErrorMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnConfirmationWithTextModel_GivenTextExistsAndEnhancedTextDoesNotExistAndNoErrorsEncountered(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            [Frozen] Mock<ITextGroupMapper> textGroupMapper,
            LoadConfirmationScreenRequest request,
            CustomActivityNavigation customActivityNavigation,
            List<Question> questions,
            TextModel textModel,
            List<Information> informationList,
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
                { "Text", textModel}
            };

            activityDataProvider
                .Setup(x => x.GetActivityData(request.WorkflowInstanceId, request.ActivityId, CancellationToken.None))
                .ReturnsAsync(dictionary);
            textGroupMapper.Setup(x => x.InformationListFromTextModel(textModel)).Returns(informationList);

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
            Assert.Equal("MyNextWorkflowDefinitionId", result.Data.NextWorkflowDefinitionIds);
            Assert.Equal(informationList, result.Data.Text);
            Assert.Empty(result.ErrorMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnConfirmationWithGroupedTextModel_GivenBothTextAndEnhancedTextExistAndNoErrorsEncountered(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            [Frozen] Mock<ITextGroupMapper> textGroupMapper,
            LoadConfirmationScreenRequest request,
            CustomActivityNavigation customActivityNavigation,
            List<Question> questions,
            TextModel textModel,
            GroupedTextModel groupedTextModel,
            List<Information> informationList,
            List<Information> informationListForGroupedModel,
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
                { "EnhancedText", groupedTextModel},
                { "Text", textModel}
            };

            activityDataProvider
                .Setup(x => x.GetActivityData(request.WorkflowInstanceId, request.ActivityId, CancellationToken.None))
                .ReturnsAsync(dictionary);
            textGroupMapper.Setup(x => x.InformationListFromTextModel(textModel)).Returns(informationList);
            textGroupMapper.Setup(x => x.InformationListFromGroupedTextModel(groupedTextModel)).Returns(informationListForGroupedModel);

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
            Assert.Equal("MyNextWorkflowDefinitionId", result.Data.NextWorkflowDefinitionIds);
            Assert.Equal(informationListForGroupedModel, result.Data.Text);
            Assert.NotEqual(informationList, result.Data.Text);
            Assert.Empty(result.ErrorMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnConfirmationWithEmptyTextModel_GivenNoTextOrEnhancedTexExistAndNoErrorsEncountered(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            LoadConfirmationScreenRequest request,
            CustomActivityNavigation customActivityNavigation,
            List<Question> questions,
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
                { "NextWorkflowDefinitionIds", "MyNextWorkflowDefinitionId" }
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
            Assert.Equal("MyNextWorkflowDefinitionId", result.Data.NextWorkflowDefinitionIds);
            Assert.Empty(result.Data.Text);
            Assert.Empty(result.ErrorMessages);
        }
    }
}
