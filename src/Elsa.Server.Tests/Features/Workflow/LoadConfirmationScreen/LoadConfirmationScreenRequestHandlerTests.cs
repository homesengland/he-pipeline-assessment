using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Features.Workflow.LoadConfirmationScreen;
using Elsa.Server.Providers;
using He.PipelineAssessment.Tests.Common;
using Moq;
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
            Assert.Null(result.Data!.CheckQuestionScreenAnswers);
            Assert.Equal(
                $"Unable to find activity navigation with Workflow Id: {request.WorkflowInstanceId} and Activity Id: {request.ActivityId} in Elsa Custom database",
                result.ErrorMessages.Single());
            elsaCustomRepository.Verify(
                x => x.GetQuestionScreenAnswers(It.IsAny<string>(), CancellationToken.None), Times.Never);
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
            Assert.Null(result.Data!.CheckQuestionScreenAnswers);
            Assert.Equal(exception.Message, result.ErrorMessages.Single());
            elsaCustomRepository.Verify(
                x => x.GetQuestionScreenAnswers(It.IsAny<string>(), CancellationToken.None), Times.Never);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnQuestionScreenAnswers_GivenNoErrorsEncountered(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            LoadConfirmationScreenRequest request,
            CustomActivityNavigation customActivityNavigation,
            List<QuestionScreenAnswer> questionScreenAnswers,
            LoadConfirmationScreenRequestHandler sut)
        {
            //Arrange
            elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(request.ActivityId,
                    request.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(customActivityNavigation);

            elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(request.WorkflowInstanceId, CancellationToken.None))
                .ReturnsAsync(questionScreenAnswers);

            var dictionary = new Dictionary<string, object?>()
            {
                { "ConfirmationTitle", "MyConfirmationTitle" },
                { "ConfirmationText", "MyConfirmationText" },
                { "FooterText", "MyFooterText" },
                { "FooterTitle", "MyFooterTitle" },
                {"Text", new List<string>()
                {
                    "1", "2", "3"
                } },
                { "NextWorkflowDefinitionIds", "MyNextWorkflowDefinitionId" }
            };
            activityDataProvider
                .Setup(x => x.GetActivityData(request.WorkflowInstanceId, request.ActivityId, CancellationToken.None))
                .ReturnsAsync(dictionary);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(questionScreenAnswers, result.Data!.CheckQuestionScreenAnswers);
            Assert.Equal(request.ActivityId, result.Data.ActivityId);
            Assert.Equal(request.WorkflowInstanceId, result.Data.WorkflowInstanceId);
            Assert.Equal(ActivityTypeConstants.ConfirmationScreen, result.Data.ActivityType);
            Assert.Equal("MyConfirmationTitle", result.Data.ConfirmationTitle);
            Assert.Equal("MyConfirmationText", result.Data.ConfirmationText);
            Assert.Equal("MyFooterText", result.Data.FooterText);
            Assert.Equal("MyFooterTitle", result.Data.FooterTitle);
            Assert.Equal(3, result.Data.Text.Count());
            Assert.Equal("1", result.Data.Text.FirstOrDefault());
            Assert.Equal("MyNextWorkflowDefinitionId", result.Data.NextWorkflowDefinitionIds);
            Assert.Empty(result.ErrorMessages);
        }
    }
}
