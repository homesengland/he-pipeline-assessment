using AutoFixture.Xunit2;
using Elsa.Client.Models;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.ReturnToActivity;
using Elsa.Server.Providers;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.Services.Workflows;
using He.PipelineAssessment.Tests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;
using ActivityBlueprint = Elsa.Services.Models.ActivityBlueprint;
using VersionOptions = Elsa.Models.VersionOptions;
using WorkflowBlueprint = Elsa.Services.Models.WorkflowBlueprint;

namespace Elsa.Server.Tests.Features.Workflow.ReturnToActivity
{
    public class ReturnToActivityCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task
            Handle_ShouldReturnSuccessfulOperationResultAndCallAllDependencies_WhenSuccessful(
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            ReturnToActivityCommandHandler sut,
            QuestionWorkflowInstance workflowInstance
        )
        {
            //Setup
            var returnToActivityCommand = new ReturnToActivityCommand()
            {
                ActivityId = "123",
                WorkflowInstanceId = "456",
            };
            var dictionary = new Dictionary<string, object?>()
            {
                { "ActivityName", "MyActivity" },
            };
            WorkflowBlueprint workflowBlueprint = new WorkflowBlueprint();
            workflowBlueprint.Activities.Add(new ActivityBlueprint()
            {
                Name = "MyActivity",
                Id="555",
                Type = "QuestionScreen"
            });

            activityDataProvider.Setup(x => x.GetActivityData( "456", "123", CancellationToken.None)).ReturnsAsync(dictionary);
            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance("456", CancellationToken.None)).ReturnsAsync(workflowInstance);
            workflowRegistry.Setup(x => x.FindAsync(workflowInstance.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.Handle(returnToActivityCommand, CancellationToken.None);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal("555", result.Data!.ActivityId);
            Assert.Equal("QuestionScreen", result.Data!.ActivityType);
            Assert.Empty(result.ErrorMessages);
            Assert.Null(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task
            Handle_ShouldReturnErrors_WhenWorkflowInstanceIdIsNull(
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            ReturnToActivityCommandHandler sut
        )
        {
            //Setup
            var returnToActivityCommand = new ReturnToActivityCommand()
            {
                ActivityId = "123",
                WorkflowInstanceId = null
            };

            //Act
            var result = await sut.Handle(returnToActivityCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(x => x.GetQuestionWorkflowInstance(It.IsAny<String>(), CancellationToken.None), Times.Never);
            activityDataProvider.Verify(x => x.GetActivityData(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None), Times.Never);
            workflowRegistry.Verify(x => x.FindAsync(It.IsAny<String>(), VersionOptions.Published, It.IsAny<String>(), CancellationToken.None), Times.Never);
            Assert.Equal(string.Empty, result.Data!.ActivityType);
            Assert.Equal(String.Empty, result.Data!.ActivityId);
            Assert.Equal(
                $"Workflow instance id or activity id is null. WorkflowInstanceIde: {returnToActivityCommand.WorkflowInstanceId} WorkflowDefinitionId: {returnToActivityCommand.ActivityId}",
                result.ErrorMessages.Single());
        }


        [Theory]
        [AutoMoqData]
        public async Task
            Handle_ShouldReturnErrors_WhenWorkflowInstanceIsNull(
            [Frozen] Mock<IActivityDataProvider> activityDataProvider,
            [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
            [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
            ReturnToActivityCommandHandler sut
        )
        {
            //Setup
            var returnToActivityCommand = new ReturnToActivityCommand()
            {
                ActivityId = "123",
                WorkflowInstanceId = "456"
            };

            var dictionary = new Dictionary<string, object?>()
            {
                { "ActivityName", "MyActivity" },
            };

            activityDataProvider.Setup(x => x.GetActivityData("456", "123", CancellationToken.None)).ReturnsAsync(dictionary);

            //Act
            var result = await sut.Handle(returnToActivityCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(x => x.GetQuestionWorkflowInstance(It.IsAny<String>(), CancellationToken.None), Times.Once);
            activityDataProvider.Verify(x => x.GetActivityData(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None), Times.Once);
            workflowRegistry.Verify(x => x.FindAsync(It.IsAny<String>(), VersionOptions.Published, It.IsAny<String>(), CancellationToken.None), Times.Never);
            Assert.Equal(string.Empty, result.Data!.ActivityType);
            Assert.Equal(String.Empty, result.Data!.ActivityId);
            Assert.Equal("Failed to get workflow instance", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task
        Handle_ShouldReturnErrors_WhenWorkflowIsNull(
        [Frozen] Mock<IActivityDataProvider> activityDataProvider,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
        ReturnToActivityCommandHandler sut,
        QuestionWorkflowInstance workflowInstance
        )
        {
            //Setup
            var returnToActivityCommand = new ReturnToActivityCommand()
            {
                ActivityId = "123",
                WorkflowInstanceId = "456"
            };

            var dictionary = new Dictionary<string, object?>()
            {
                { "ActivityName", "MyActivity" },
            };

            activityDataProvider.Setup(x => x.GetActivityData("456", "123", CancellationToken.None)).ReturnsAsync(dictionary);
            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance("456", CancellationToken.None)).ReturnsAsync(workflowInstance);

            //Act
            var result = await sut.Handle(returnToActivityCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(x => x.GetQuestionWorkflowInstance(It.IsAny<String>(), CancellationToken.None), Times.Once);
            activityDataProvider.Verify(x => x.GetActivityData(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None), Times.Once);
            workflowRegistry.Verify(x => x.FindAsync(It.IsAny<String>(), VersionOptions.Published, It.IsAny<String>(), CancellationToken.None), Times.Once);
            Assert.Equal(string.Empty, result.Data!.ActivityType);
            Assert.Equal(String.Empty, result.Data!.ActivityId);
            Assert.Equal("Failed to get workflow.", result.ErrorMessages.Single());
        }


        [Theory]
        [AutoMoqData]
        public async Task
        Handle_ShouldReturnErrors_WhenActivityNotFound(
        [Frozen] Mock<IActivityDataProvider> activityDataProvider,
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
        ReturnToActivityCommandHandler sut,
        QuestionWorkflowInstance workflowInstance,
        IWorkflowBlueprint workflowBlueprint
        )
        {
            //Setup
            var returnToActivityCommand = new ReturnToActivityCommand()
            {
                ActivityId = "123",
                WorkflowInstanceId = "456"
            };

            var dictionary = new Dictionary<string, object?>()
            {
                { "ActivityName", "MyActivity" },
            };

            activityDataProvider.Setup(x => x.GetActivityData("456", "123", CancellationToken.None)).ReturnsAsync(dictionary);
            elsaCustomRepository.Setup(x => x.GetQuestionWorkflowInstance("456", CancellationToken.None)).ReturnsAsync(workflowInstance);
            workflowRegistry.Setup(x => x.FindAsync(workflowInstance.WorkflowDefinitionId, VersionOptions.Published, null, CancellationToken.None)).ReturnsAsync(workflowBlueprint);

            //Act
            var result = await sut.Handle(returnToActivityCommand, CancellationToken.None);

            //Assert
            elsaCustomRepository.Verify(x => x.GetQuestionWorkflowInstance(It.IsAny<String>(), CancellationToken.None), Times.Once);
            activityDataProvider.Verify(x => x.GetActivityData(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None), Times.Once);
            workflowRegistry.Verify(x => x.FindAsync(It.IsAny<String>(), VersionOptions.Published, It.IsAny<String>(), CancellationToken.None), Times.Once);
            Assert.Equal(string.Empty, result.Data!.ActivityType);
            Assert.Equal(String.Empty, result.Data!.ActivityId);
            Assert.Equal("Failed to find activity to return to. Activity Name: MyActivity", result.ErrorMessages.Single());
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsOperationResultWithErrors_GivenExceptionIsThrown(
        [Frozen] Mock<IElsaCustomRepository> elsaCustomRepository,
        [Frozen] Mock<IActivityDataProvider> activityDataProvider,
        [Frozen] Mock<IWorkflowRegistry> workflowRegistry,
        Exception exception,
        ReturnToActivityCommandHandler sut)
        {
            //Arrange
            var returnToActivityCommand = new ReturnToActivityCommand()
            {
                ActivityId = "123",
                WorkflowInstanceId = "456"
            };

            activityDataProvider.Setup(x => x.GetActivityData("456", "123", CancellationToken.None)).ThrowsAsync(exception);

            //Act
            var result = await sut.Handle(returnToActivityCommand, CancellationToken.None);

            //Assert
            Assert.Equal(String.Empty, result.Data!.ActivityId);
            Assert.Equal(exception.Message, result.ErrorMessages.Single());
            elsaCustomRepository.Verify(x => x.GetQuestionWorkflowInstance(It.IsAny<String>(), CancellationToken.None), Times.Never);
            workflowRegistry.Verify(x => x.FindAsync(It.IsAny<String>(), VersionOptions.Published, It.IsAny<String>(), CancellationToken.None), Times.Never);
        }
    }
}
