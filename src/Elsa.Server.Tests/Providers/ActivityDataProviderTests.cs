using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.Models;
using Elsa.Server.Features.Workflow.CheckYourAnswersSaveAndContinue;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.Server.Tests.Providers
{
    public class ActivityDataProviderTests
    {

        [Theory]
        [AutoMoqData]
        public async Task
            GetActivityData_ShouldThrowException_WhenActivityDataCannotBeFound(
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProviderMock,
            string workflowInstanceId,
            string activityId,
            CancellationToken cancellationToken,
            WorkflowInstance workflowInstance,
            ActivityDataProvider sut)
        {
            // Arrange
            workflowInstanceProviderMock.Setup(x => x.GetWorkflowInstance(workflowInstanceId, cancellationToken)).ReturnsAsync(workflowInstance);
            var exceptionMessage = string.Empty;

            //Act

            var exception = await Assert.ThrowsAsync<Exception>(() => sut.GetActivityData(workflowInstanceId, activityId, cancellationToken));

            //Assert
            Assert.Equal($"Cannot find activity data with id {activityId}.", exception.Message);
        }


        [Theory]
        [AutoMoqData]
        public async Task
            GetActivityData_ReturnsActivity_WhenActivityDataFound(
            [Frozen] Mock<IWorkflowInstanceProvider> workflowInstanceProviderMock,
            string workflowInstanceId,
            string activityId,
            CancellationToken cancellationToken,
            WorkflowInstance workflowInstance,
            ActivityDataProvider sut)
        {
            // Arrange
            workflowInstanceProviderMock.Setup(x => x.GetWorkflowInstance(workflowInstanceId, cancellationToken)).ReturnsAsync(workflowInstance);
            var exceptionMessage = string.Empty;
            IDictionary<string, object?> activityData = new Dictionary<string, object?>();

            var assessmentQuestionsDictionary = new Dictionary<string, object?>();
            //AssessmentQuestions? elsaAssessmentQuestions = null;
            assessmentQuestionsDictionary.Add("title", "test");

            workflowInstance.ActivityData.Add(activityId, assessmentQuestionsDictionary);

            //Act
            activityData = await sut.GetActivityData(workflowInstanceId, activityId, cancellationToken);

            //Assert
            Assert.Equal(1, activityData.Count);
        }
    }
}
