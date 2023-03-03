using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.QuestionScreenSaveAndContinue;

public class QuestionScreenSaveAndContinueCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ShouldReturnSuccessfulOperationResultAndCallAllDependencies_WhenSuccessful(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
            [Frozen] Mock<IDeleteChangedWorkflowPathService> deleteChangedWorkflowPathService,
            WorkflowBlueprint workflowBlueprint,
            ActivityBlueprint activityBlueprint,
            List<QuestionScreenAnswer> currentAssessmentQuestions,
            WorkflowInstance workflowInstance,
            CustomActivityNavigation nextAssessmentActivity,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            QuestionScreenSaveAndContinueCommandHandler sut)
    {
        //Arrange
        var expectedOrder = new List<string>() { "Delete", "Create" };
        var actualOrder = new List<string>();
        var workflowNextActivityModel = new WorkflowNextActivityModel
        {
            NextActivity = activityBlueprint
        };
        for (var i = 0; i < currentAssessmentQuestions.Count; i++)
        {
            var questionId = saveAndContinueCommand.Answers![i].Id;
            currentAssessmentQuestions[i].QuestionId = questionId;
        }

        activityBlueprint.Id = workflowInstance.Output!.ActivityId;
        workflowBlueprint.Activities.Add(activityBlueprint);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(currentAssessmentQuestions);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(nextAssessmentActivity);

        workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        workflowNextActivityProvider.Setup(x => x.GetNextActivity(saveAndContinueCommand.ActivityId, saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, ActivityTypeConstants.QuestionScreen, CancellationToken.None))
            .ReturnsAsync(workflowNextActivityModel);

        deleteChangedWorkflowPathService.Setup(
                x => x.DeleteChangedWorkflowPath(saveAndContinueCommand.WorkflowInstanceId, saveAndContinueCommand.ActivityId,
                    activityBlueprint, workflowInstance, CancellationToken.None))
            .Returns(Task.FromResult(true))
            .Callback(() => actualOrder.Add("Delete"));

        nextActivityNavigationService.Setup(
                x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId,
                    nextAssessmentActivity, activityBlueprint, workflowInstance, CancellationToken.None))
            .Returns(Task.FromResult(true))
            .Callback(() => actualOrder.Add("Create"));

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Once);

        nextActivityNavigationService.Verify(
            x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId,
                nextAssessmentActivity, activityBlueprint, workflowInstance, CancellationToken.None), Times.Once);

        deleteChangedWorkflowPathService.Verify(
            x => x.DeleteChangedWorkflowPath(saveAndContinueCommand.WorkflowInstanceId, saveAndContinueCommand.ActivityId,
                activityBlueprint, workflowInstance, CancellationToken.None), Times.Once);

        workflowInstanceProvider.Verify(
            x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Once);

        Assert.Equal(saveAndContinueCommand.Answers![0].AnswerText, currentAssessmentQuestions[0].Answer);
        Assert.Equal(saveAndContinueCommand.Answers![1].AnswerText, currentAssessmentQuestions[1].Answer);
        Assert.Equal(saveAndContinueCommand.Answers![2].AnswerText, currentAssessmentQuestions[2].Answer);

        Assert.Equal(activityBlueprint.Id, result.Data!.NextActivityId);
        Assert.Equal(saveAndContinueCommand.WorkflowInstanceId, result.Data.WorkflowInstanceId);
        Assert.Equal(activityBlueprint.Type, result.Data.ActivityType);
        Assert.Empty(result.ErrorMessages);
        Assert.Null(result.ValidationMessages);
        Assert.Equal(expectedOrder, actualOrder);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldReturnErrors_WhenAssessmentQuestionsNotFoundInDatabase(
        [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
        QuestionScreenSaveAndContinueCommandHandler sut)
    {
        //Arrange
        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(new List<QuestionScreenAnswer>());

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Never);
        elsaCustomRepository.Verify(
            x => x.CreateCustomActivityNavigationAsync(It.IsAny<CustomActivityNavigation>(), CancellationToken.None),
            Times.Never);
        workflowInstanceProvider.Verify(
            x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Never);
        Assert.Null(result.Data);
        Assert.Equal(
            $"Unable to find any questions for Question Screen activity Workflow Instance Id: {saveAndContinueCommand.WorkflowInstanceId} and Activity Id: {saveAndContinueCommand.ActivityId} in custom database",
            result.ErrorMessages.Single());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldReturnErrors_WhenADependencyThrows(
        [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        Exception exception,
        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
        QuestionScreenSaveAndContinueCommandHandler sut)
    {
        //Arrange
        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .Throws(exception);

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Never);
        elsaCustomRepository.Verify(
            x => x.CreateCustomActivityNavigationAsync(It.IsAny<CustomActivityNavigation>(), CancellationToken.None),
            Times.Never);
        workflowInstanceProvider.Verify(
            x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Never);
        Assert.Null(result.Data);
        Assert.Equal(exception.Message, result.ErrorMessages.Single());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ShouldNotCallDependencies_GivenWorkflowStatusIsFinished(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
        [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
        [Frozen] Mock<IDeleteChangedWorkflowPathService> deleteChangedWorkflowPathService,
        WorkflowBlueprint workflowBlueprint,
        ActivityBlueprint activityBlueprint,
        List<QuestionScreenAnswer> currentAssessmentQuestions,
        WorkflowInstance workflowInstance,
        CustomActivityNavigation nextAssessmentActivity,
        QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
        QuestionScreenSaveAndContinueCommandHandler sut
    )
    {
        //Arrange
        activityBlueprint.Id = workflowInstance.Output!.ActivityId;
        activityBlueprint.Type = ActivityTypeConstants.QuestionScreen;
        workflowBlueprint.Activities.Add(activityBlueprint);
        workflowInstance.WorkflowStatus = WorkflowStatus.Finished;

        nextAssessmentActivity.ActivityType = ActivityTypeConstants.QuestionScreen;

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(currentAssessmentQuestions);

        workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        //Act
        var result = await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert
        elsaCustomRepository.Verify(
            x => x.SaveChanges(CancellationToken.None), Times.Never);

        nextActivityNavigationService.Verify(
            x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId,
                nextAssessmentActivity, activityBlueprint, workflowInstance, CancellationToken.None), Times.Never());

        deleteChangedWorkflowPathService.Verify(
            x => x.DeleteChangedWorkflowPath(saveAndContinueCommand.WorkflowInstanceId, saveAndContinueCommand.ActivityId,
                activityBlueprint, workflowInstance, CancellationToken.None), Times.Never());
        workflowInstanceProvider.Verify(
            x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None), Times.Once);

        Assert.Null(result.Data);
        Assert.Equal(
            $"Unable to save answers. Workflow status is: {workflowInstance.WorkflowStatus}",
            result.ErrorMessages.Single());
        Assert.Null(result.ValidationMessages);
    }

    [Theory]
    [AutoMoqData]
    public async Task
        Handle_ShouldCallServicesWithCorrectWorkflowInstance(
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProvider,
            [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
            [Frozen] Mock<IDeleteChangedWorkflowPathService> deleteChangedWorkflowPathService,
            WorkflowBlueprint workflowBlueprint,
            ActivityBlueprint activityBlueprint,
            List<QuestionScreenAnswer> currentAssessmentQuestions,
            WorkflowInstance workflowInstance,
            WorkflowInstance anotherWorkflowInstance,
            CustomActivityNavigation nextAssessmentActivity,
            QuestionScreenSaveAndContinueCommand saveAndContinueCommand,
            QuestionScreenSaveAndContinueCommandHandler sut)
    {
        //Arrange
        var workflowNextActivityModel = new WorkflowNextActivityModel
        {
            NextActivity = activityBlueprint,
            WorkflowInstance = anotherWorkflowInstance
        };
        for (var i = 0; i < currentAssessmentQuestions.Count; i++)
        {
            var questionId = saveAndContinueCommand.Answers![i].Id;
            currentAssessmentQuestions[i].QuestionId = questionId;
        }

        activityBlueprint.Id = workflowInstance.Output!.ActivityId;
        workflowBlueprint.Activities.Add(activityBlueprint);

        elsaCustomRepository.Setup(x => x.GetQuestionScreenAnswers(saveAndContinueCommand.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(currentAssessmentQuestions);

        elsaCustomRepository.Setup(x => x.GetCustomActivityNavigation(workflowInstance.Output.ActivityId,
                saveAndContinueCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(nextAssessmentActivity);

        workflowInstanceProvider.Setup(x => x.GetWorkflowInstance(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(workflowInstance);

        workflowNextActivityProvider.Setup(x => x.GetNextActivity(saveAndContinueCommand.ActivityId, saveAndContinueCommand.WorkflowInstanceId, currentAssessmentQuestions, ActivityTypeConstants.QuestionScreen, CancellationToken.None))
            .ReturnsAsync(workflowNextActivityModel);

        //Act
        await sut.Handle(saveAndContinueCommand, CancellationToken.None);

        //Assert

        nextActivityNavigationService.Verify(
            x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId,
                nextAssessmentActivity, activityBlueprint, anotherWorkflowInstance, CancellationToken.None), Times.Once);
        deleteChangedWorkflowPathService.Verify(
            x => x.DeleteChangedWorkflowPath(saveAndContinueCommand.WorkflowInstanceId, saveAndContinueCommand.ActivityId,
                activityBlueprint, anotherWorkflowInstance, CancellationToken.None), Times.Once);
        nextActivityNavigationService.Verify(
            x => x.CreateNextActivityNavigation(saveAndContinueCommand.ActivityId,
                nextAssessmentActivity, activityBlueprint, workflowInstance, CancellationToken.None), Times.Never);
        deleteChangedWorkflowPathService.Verify(
            x => x.DeleteChangedWorkflowPath(saveAndContinueCommand.WorkflowInstanceId, saveAndContinueCommand.ActivityId,
                activityBlueprint, workflowInstance, CancellationToken.None), Times.Never);
    }
}