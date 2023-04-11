using AutoFixture.Xunit2;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Features.Workflow.SetWorkflowResult;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Moq;
using System;
using Xunit;
using WorkflowInstance = Elsa.Models.WorkflowInstance;

namespace Elsa.Server.Tests.Features.Workflow.SetWorkflowResult;
public class SetWorkflowResultHandlerTests
{
    [Theory]
    [AutoMoqData]

    public async Task Handle_ShouldReturnSuccessfulOperationResult_WhenSuccessful(

         [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
         [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
         [Frozen] Mock<INextActivityNavigationService> nextActivityNavigationService,
         SetWorkflowResultCommand setWorkflowResultCommand,
         WorkflowBlueprint workflowBlueprint,
         ActivityBlueprint activityBlueprint,
         RunWorkflowResult runWorkflowResult,
         WorkflowInstance workflowInstance,
         CustomActivityNavigation customActivityNavigation,
         SetWorkflowResultHandler sut

        )
    {
        // Arrange

        var workflowNextActivityModel = new WorkflowNextActivityModel
        {
            NextActivity = activityBlueprint,
            WorkflowInstance = workflowInstance
        };

        activityBlueprint.Id = runWorkflowResult.WorkflowInstance!.LastExecutedActivityId!;
        workflowBlueprint.Activities.Add(activityBlueprint);

        workflowNextActivityProvider
            .Setup(x => x.GetNextActivity(setWorkflowResultCommand.ActivityId, setWorkflowResultCommand.WorkflowInstanceId, null, ActivityTypeConstants.CheckYourAnswersScreen, CancellationToken.None))
            .ReturnsAsync(workflowNextActivityModel);

        elsaCustomRepository
            .Setup(x => x.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id, setWorkflowResultCommand.WorkflowInstanceId, CancellationToken.None))
            .ReturnsAsync(customActivityNavigation);

        // Act
        var result = await sut.Handle(setWorkflowResultCommand, CancellationToken.None);

        // Assert

        nextActivityNavigationService.Verify(
                x => x.CreateNextActivityNavigation(setWorkflowResultCommand.ActivityId, customActivityNavigation,
                    workflowNextActivityModel.NextActivity, workflowNextActivityModel.WorkflowInstance, CancellationToken.None), Times.Once);

        Assert.Equal(setWorkflowResultCommand.WorkflowInstanceId, result.Data!.WorkflowInstanceId);
        Assert.Equal(workflowNextActivityModel.NextActivity.Id, result.Data!.NextActivityId);
        Assert.Equal(workflowNextActivityModel.NextActivity.Type, result.Data.ActivityType);
        Assert.Empty(result.ErrorMessages);
        Assert.Null(result.ValidationMessages);

    }

    [Theory]
    [AutoMoqData]

    public async Task Handle_ReturnErrorOperationResult_SetworkflowReturnsNullWorkflowInstance(

         [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
         [Frozen] Mock<IWorkflowNextActivityProvider> workflowNextActivityProvider,
         SetWorkflowResultCommand setWorkflowResultCommand,
         Exception exception,
         SetWorkflowResultHandler sut
        )
    {

        // Arrange

        workflowNextActivityProvider
            .Setup(x => x.GetNextActivity(setWorkflowResultCommand.ActivityId, setWorkflowResultCommand.WorkflowInstanceId, null, ActivityTypeConstants.CheckYourAnswersScreen, CancellationToken.None))
            .Throws(exception);

        // Act

        var result = await sut.Handle(setWorkflowResultCommand, CancellationToken.None);

        // Assert

        elsaCustomRepository.Verify(x => x.CreateCustomActivityNavigationAsync(It.IsAny<CustomActivityNavigation>(), CancellationToken.None), Times.Never);

        Assert.Null(result.Data);
        Assert.Equal(exception.Message, result.ErrorMessages.Single());

    }
}
