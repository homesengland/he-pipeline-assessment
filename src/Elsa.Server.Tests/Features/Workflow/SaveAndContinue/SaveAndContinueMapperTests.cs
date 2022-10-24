using AutoFixture.Xunit2;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Server.Features.Workflow.SaveAndContinue;
using Elsa.Server.Features.Workflow.SubmitAssessmentStage;
using Elsa.Server.Providers;
using Elsa.Services.Models;
using He.PipelineAssessment.Common.Tests;
using Moq;
using Xunit;

namespace Elsa.Server.Tests.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void SaveAndContinueCommandToNextAssessmentQuestion_ShouldReturnAssessmentQuestion(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider,
            SubmitAssessmentStageCommand saveAndContinueCommand,
            WorkflowInstance workflowInstance,
            ActivityBlueprint activityBlueprint,
            string workflowName,
            SaveAndContinueMapper sut
        )
        {
            //Arrange
            var currentTimeUtc = DateTime.UtcNow;
            mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(currentTimeUtc);

            //Act
            var result = sut.SaveAndContinueCommandToNextAssessmentQuestion(saveAndContinueCommand.ActivityId, workflowInstance, activityBlueprint, workflowName);

            //Assert
            Assert.IsType<AssessmentQuestion>(result);
            Assert.Equal(activityBlueprint.Id, result!.ActivityId);
            Assert.Equal(activityBlueprint.Type, result!.ActivityType);
            Assert.Equal(activityBlueprint.Name, result!.ActivityName);
            Assert.Null(result.Answer);
            Assert.Equal(workflowInstance.Id, result.WorkflowInstanceId);
            Assert.Equal(workflowInstance.DefinitionId, result.WorkflowDefinitionId);
            Assert.Equal(workflowName, result.WorkflowName);
            Assert.Equal(workflowInstance.Version, result.Version);
            Assert.Equal(workflowInstance.CorrelationId, result.CorrelationId);
            Assert.Equal(saveAndContinueCommand.ActivityId, result.PreviousActivityId);
            Assert.Equal(currentTimeUtc, result.CreatedDateTime);
        }
    }
}
