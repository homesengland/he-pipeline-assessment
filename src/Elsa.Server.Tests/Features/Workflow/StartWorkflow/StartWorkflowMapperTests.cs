﻿using AutoFixture.Xunit2;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Providers;
using Elsa.Services.Models;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.StartWorkflow
{
    public class StartWorkflowMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void RunWorkflowResultToMultipleChoiceQuestionModel_ShouldReturnMultipleChoiceQuestionModel_WhenWorkflowInstanceIsNotNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            RunWorkflowResult runWorkflowResult
            )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);


            //Act
            var result = sut.RunWorkflowResultToMultipleChoiceQuestionModel(runWorkflowResult);

            //Assert
            Assert.IsType<MultipleChoiceQuestionModel>(result);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.LastExecutedActivityId, result!.ActivityId);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.Id, result.WorkflowInstanceId);
            Assert.Equal(runWorkflowResult.WorkflowInstance!.LastExecutedActivityId, result.PreviousActivityId);
            Assert.Equal(
                $"{runWorkflowResult.WorkflowInstance.Id}-{runWorkflowResult.WorkflowInstance.LastExecutedActivityId}",
                result.Id);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }

        [Theory]
        [AutoData]
        public void RunWorkflowResultToMultipleChoiceQuestionModel_ShouldReturnNull_WhenWorkflowInstanceNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider
            )
        {
            //Arrange
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);
            var runWorkflowResult = new RunWorkflowResult(null, null, null, false);

            //Act
            var result = sut.RunWorkflowResultToMultipleChoiceQuestionModel(runWorkflowResult);

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoData]
        public void RunWorkflowResultToMultipleChoiceQuestionModel_ShouldReturnNull_WhenWorkflowInstanceIdNull(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            //Arrange
            StartWorkflowMapper sut = new StartWorkflowMapper(mockDateTimeProvider.Object);

            var workflowInstace = new Elsa.Models.WorkflowInstance();
            var runWorkflowResult = new RunWorkflowResult(workflowInstace, null, null, false);

            //Act
            var result = sut.RunWorkflowResultToMultipleChoiceQuestionModel(runWorkflowResult);

            //Assert
            Assert.Null(result);
        }


    }
}
