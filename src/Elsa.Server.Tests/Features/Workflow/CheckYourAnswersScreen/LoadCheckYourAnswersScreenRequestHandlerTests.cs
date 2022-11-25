using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen;
using Elsa.Server.Features.Workflow.LoadQuestionScreen;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.CheckYourAnswersScreen;

public class LoadCheckYourAnswersScreenRequestHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenCustomActivityNavigationDoesNotExist(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadCheckYourAnswersRequest loadCheckYourAnswersRequest,
        LoadCheckYourAnswersScreenRequestHandler sut)
    {
        //Arrange
        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadCheckYourAnswersRequest.ActivityId,
                loadCheckYourAnswersRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync((CustomActivityNavigation?)null);

        //Act
        var result = await sut.Handle(loadCheckYourAnswersRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.QuestionScreenAnswers);
        Assert.Equal(
            $"Unable to find activity navigation with Workflow Id: {loadCheckYourAnswersRequest.WorkflowInstanceId} and Activity Id: {loadCheckYourAnswersRequest.ActivityId} in Elsa Custom database",
            result.ErrorMessages.Single());
        elsaCustomRepository.Verify(
            x => x.GetQuestionScreenAnswers(It.IsAny<string>(), CancellationToken.None), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsOperationResultWithErrors_GivenExceptionIsThrown(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadCheckYourAnswersRequest loadCheckYourAnswersRequest,
        Exception exception,
        LoadCheckYourAnswersScreenRequestHandler sut)
    {
        //Arrange
        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadCheckYourAnswersRequest.ActivityId,
                loadCheckYourAnswersRequest.WorkflowInstanceId, CancellationToken.None))
            .ThrowsAsync(exception);

        //Act
        var result = await sut.Handle(loadCheckYourAnswersRequest, CancellationToken.None);

        //Assert
        Assert.Null(result.Data!.QuestionScreenAnswers);
        Assert.Equal(exception.Message, result.ErrorMessages.Single());
        elsaCustomRepository.Verify(
            x => x.GetQuestionScreenAnswers(It.IsAny<string>(), CancellationToken.None), Times.Never);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnQuestionScreenAnswers_GivenActivityIsQuestionScreenAndNoErrorsEncountered(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        LoadCheckYourAnswersRequest loadCheckYourAnswersRequest,
        CustomActivityNavigation customActivityNavigation,
        List<QuestionScreenAnswer> questionScreenAnswers,
        LoadCheckYourAnswersScreenRequestHandler sut)
    {
        //Arrange
        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(loadCheckYourAnswersRequest.ActivityId,
                loadCheckYourAnswersRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(loadCheckYourAnswersRequest.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(questionScreenAnswers);

        //Act
        var result = await sut.Handle(loadCheckYourAnswersRequest, CancellationToken.None);

        //Assert
        Assert.NotNull(result.Data);
        Assert.Equal(questionScreenAnswers, result.Data!.QuestionScreenAnswers);
        Assert.Equal(loadCheckYourAnswersRequest.ActivityId, result.Data.ActivityId);
        Assert.Equal(loadCheckYourAnswersRequest.WorkflowInstanceId, result.Data.WorkflowInstanceId);
        Assert.Equal(ActivityTypeConstants.CheckYourAnswersScreen, result.Data.ActivityType);
        Assert.Equal(customActivityNavigation.PreviousActivityId, result.Data.PreviousActivityId);
        Assert.Equal(customActivityNavigation.PreviousActivityType, result.Data.PreviousActivityType);
        Assert.Empty(result.ErrorMessages);
    }
}