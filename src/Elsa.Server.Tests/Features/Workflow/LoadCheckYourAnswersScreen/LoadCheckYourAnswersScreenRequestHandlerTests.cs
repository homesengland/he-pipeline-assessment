using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen;
using Elsa.Server.Providers;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.LoadCheckYourAnswersScreen;

public class LoadCheckYourAnswersScreenRequestHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenCustomActivityNavigationDoesNotExist(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadCheckYourAnswersScreenRequest loadCheckYourAnswersScreenRequest,
        LoadCheckYourAnswersScreenRequestHandler sut)
    {
        //Arrange
        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadCheckYourAnswersScreenRequest.ActivityId,
                loadCheckYourAnswersScreenRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync((CustomActivityNavigation?)null);

        //Act
        var result = await sut.Handle(loadCheckYourAnswersScreenRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.CheckQuestionScreenAnswers);
        Assert.Equal(
            $"Unable to find activity navigation with Workflow Id: {loadCheckYourAnswersScreenRequest.WorkflowInstanceId} and Activity Id: {loadCheckYourAnswersScreenRequest.ActivityId} in Elsa Custom database",
            result.ErrorMessages.Single());
        elsaCustomRepository.Verify(
            x => x.GetQuestionScreenAnswers(It.IsAny<string>(), CancellationToken.None), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenExceptionIsThrown(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadCheckYourAnswersScreenRequest loadCheckYourAnswersScreenRequest,
        Exception exception,
        LoadCheckYourAnswersScreenRequestHandler sut)
    {
        //Arrange
        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadCheckYourAnswersScreenRequest.ActivityId,
                loadCheckYourAnswersScreenRequest.WorkflowInstanceId, CancellationToken.None))
            .ThrowsAsync(exception);

        //Act
        var result = await sut.Handle(loadCheckYourAnswersScreenRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.CheckQuestionScreenAnswers);
        Assert.Equal(exception.Message, result.ErrorMessages.Single());
        elsaCustomRepository.Verify(
            x => x.GetQuestionScreenAnswers(It.IsAny<string>(), CancellationToken.None), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnQuestionScreenAnswers_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<IActivityDataProvider> activityDataProvider,
        LoadCheckYourAnswersScreenRequest request,
        CustomActivityNavigation customActivityNavigation,
        List<QuestionScreenAnswer> questionScreenAnswers,
        LoadCheckYourAnswersScreenRequestHandler sut)
    {
        //Arrange
        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(request.ActivityId,
                request.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(request.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(questionScreenAnswers);

        var dictionary = new Dictionary<string, object?>()
        {
            { "FooterText", "MyFooterText" },
            { "FooterTitle", "MyFooterTitle" },
            { "Title", "MyPageTitle" }
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
        Assert.Equal(ActivityTypeConstants.CheckYourAnswersScreen, result.Data.ActivityType);
        Assert.Equal(customActivityNavigation.PreviousActivityId, result.Data.PreviousActivityId);
        Assert.Equal(customActivityNavigation.PreviousActivityType, result.Data.PreviousActivityType);
        Assert.Equal("MyFooterText", result.Data.FooterText);
        Assert.Equal("MyFooterTitle", result.Data.FooterTitle);
        Assert.Equal("MyPageTitle", result.Data.PageTitle);
        Assert.Empty(result.ErrorMessages);
    }
}